using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Game : IDisposable
    {
        public int GameId { get; private set; }
        public Team Owner { get; private set; }

        private GameState _state;
        public GameState State
        {
            get
            {
                return _state;
            }

            private set
            {
                _state = value;
                if (_proto != null)
                    _proto.LogGameState(_state);
            }
        }

        public string Label
        {
            get
            {
                return string.Format("Game #{0}: {1}", GameId, Owner);
            }
        }

        private List<Player> _players;
        private List<ObserverQueue> _observers;
        private MapData _map;
        private GameProtocol _proto;

        private object _liveLock;
        private Dictionary<int, int> _indexes;
        private PlayerState[] _pstates;
        private int _gameTurnEnded;
        private int _gameTurnStarted;
        private DateTime _turnStart;
        private DateTime _turnEnd;
        private int _turnDuration;

        public Game(int gameId, Team owner)
        {
            GameId = gameId;
            Owner = owner;
            State = GameState.SETUP;
            _players = new List<Player>();
            _observers = new List<ObserverQueue>();
            _proto = new GameProtocol(this);
            _turnDuration = Settings.DefaultGameTurnDurationMillis;
        }

        public void Dispose()
        {
            _proto.Dispose();
        }

        private void checkSetupState()
        {
            if (State != GameState.SETUP)
                throw new ApplicationException("Game must be in SETUP state");
        }

        public void checkRunState()
        {
            if (State == GameState.SETUP)
                throw new ApplicationException("Game must be started");
        }

        public void checkPlayState()
        {
            if (State != GameState.PLAY && State != GameState.PAUSE)
                throw new ApplicationException("Game must be playing");
        }

        public void CheckGameDeletable()
        {
            if (State == GameState.SETUP)
                return;
            lock (_liveLock)
            {
                if (State != GameState.FINISH)
                    throw new ApplicationException("Game must be finished");
                if (_pstates.Any(p => p.IsPresent)) // Should never happen but just as a precaution
                    throw new ApplicationException("All players must leave");
            }
        }

        public void AddPlayer(Player player)
        {
            checkSetupState();
            if (player.Game != null)
                throw new ApplicationException("Player already in a game");
            _players.Add(player);
            player.Game = this;
        }

        public void RemovePlayer(Player player)
        {
            checkSetupState();
            if (player.Game != this)
                throw new ApplicationException("Player is not in this game");
            _players.Remove(player);
            player.Game = null;
        }

        public IEnumerable<Player> ListPlayers()
        {
            return _players;
        }

        public void SetMap(MapData mapData)
        {
            checkSetupState();
            _map = mapData;
        }

        public void Start()
        {
            checkSetupState();
            if (_players.Count < 1 || _players.Count > 8)
                throw new ApplicationException("Number of players in the game must be 1 to 8");
            if (_map == null)
                throw new ApplicationException("Map is not loaded");
            
            _indexes = _players.Select((p, i) => Tuple.Create(p.PlayerId, i)).ToDictionary(k => k.Item1, v => v.Item2);

            // Find all starting positions and clean the map
            Dictionary<int, Point> starts = new Dictionary<int,Point>();
            for (int row = 0; row < _map.Height; row++) {
                for (int col = 0; col < _map.Width; col++) {
                    switch (_map.Tiles[row, col]) {
                        case TileType.EMPTY:
                        case TileType.BLOCK:
                            break;
                        case TileType.BODY0:
                        case TileType.BODY1:
                        case TileType.BODY2:
                        case TileType.BODY3:
                        case TileType.BODY4:
                        case TileType.BODY5:
                        case TileType.BODY6:
                        case TileType.BODY7:
                            _map.Tiles[row, col] = TileType.EMPTY;
                            break;
                        case TileType.HEAD0:
                        case TileType.HEAD1:
                        case TileType.HEAD2:
                        case TileType.HEAD3:
                        case TileType.HEAD4:
                        case TileType.HEAD5:
                        case TileType.HEAD6:
                        case TileType.HEAD7:
                            int p = _map.Tiles[row, col] - TileType.HEAD0;
                            if (starts.ContainsKey(p)) {
                                _map.Tiles[row, col] = TileType.EMPTY;
                                break;
                            }
                            starts[p] = new Point(row, col);
                            break;
                        default:
                            _map.Tiles[row, col] = TileType.BLOCK;
                            break;
                    }
                }
            }

            // Setup player initial states
            _pstates = new PlayerState[_players.Count];
            for (int p = 0; p < _pstates.Length; p++)
            {
                Point startingPosition;
                if (!starts.TryGetValue(p, out startingPosition))
                    throw new ApplicationException(string.Format("The map does not specify starting position for player {0}", p));
                _pstates[p] = new PlayerState
                {
                    Index = p,
                    PlayerId = _players[p].PlayerId,
                    Condition = PlayerCondition.PLAY,
                    IsPresent = true,
                    Head = startingPosition,
                    TurnPosition = startingPosition,
                    TurnFinTime = default(DateTime),
                    PenaltyPoints = 0,
                    BonusPoints = 0,
                    OvertimeTurnMsec = 0,
                    OvertimeTurnTurn = -1,
                    PenaltyThresholdReachedTurn = -1
                };
                starts.Remove(p);
            }

            // Remove additional players from the map
            foreach (Point p in starts.Values) {
                _map.Tiles[p.Row, p.Col] = TileType.EMPTY;
            }

            _gameTurnEnded = 0;
            _gameTurnStarted = 0;
            prepareTurnTimings();
            _liveLock = new object();

            // Start paused
            State = GameState.PAUSE;
            _proto.LogGameStart(this);
        }

        private void prepareTurnTimings()
        {
            _turnStart = DateTime.Now;
            _turnEnd = _turnStart.AddMilliseconds(_turnDuration);
        }

        private int findPlayer(int playerId)
        {
            int index;
            if (!_indexes.TryGetValue(playerId, out index))
                throw new ApplicationException("Player is not in this game");
            return index;
        }

        private ObserverQueue findObserver(int observerId)
        {
            ObserverQueue q = _observers.FirstOrDefault(p => p.Observer.ObserverId == observerId);
            if (q == null)
                throw new ApplicationException("This observer does not watch this game");
            return q;
        }

        public GameViewInfo GetGameView(int playerId)
        {
            lock (_liveLock)
            {
                var gv = new GameViewInfo();
                if (playerId > 0)
                {
                    int p = findPlayer(playerId);
                    if (!_pstates[p].IsActive || _pstates[p].TurnCompleted >= _gameTurnStarted)
                        throw new WaitException();
                    gv.PlayerIndex = p;
                }
                gv.GameState = State;
                gv.Turn = _gameTurnStarted;
                gv.PlayerStates = _pstates.Select(s => new PlayerStateInfo(s)).ToArray();
                gv.Map = (MapData)_map.Clone();
                return gv;
            }
        }

        public void PerformMove(int playerId, Point position)
        {
            lock (_liveLock)
            {
                checkPlayState();
                int p = findPlayer(playerId);
                if (!_pstates[p].IsActive)
                    throw new ApplicationException("You cannot make more moves");
                if (_pstates[p].TurnCompleted >= _gameTurnStarted)
                    throw new WaitException();
                // Just record the move
                _pstates[p].TurnPosition = position;
                _proto.LogMove(p, playerId, position);
            }
        }

        private bool playerExitTest(WaitTurnInfo wi)
        {
            int p = wi.PlayerIndex;
            if (!_pstates[p].IsActive)
            {
                wi.TurnComplete = true;
                wi.GameFinished = true;
                wi.FinishCondition = _pstates[p].Condition;
                wi.FinishComment = _pstates[p].Comment;
                return true;
            }
            return false;
        }

        public WaitTurnInfo CompletePlayerTurn(int playerId, int refTurn)
        {
            lock (_liveLock)
            {
                WaitTurnInfo wi = new WaitTurnInfo();
                int p = findPlayer(playerId);
                if (!_pstates[p].IsPresent)
                    throw new ApplicationException("Player was dropped from the game");
                wi.PlayerIndex = p;
                if (refTurn == 0)
                {
                    // Crash recovery logic
                    if (_pstates[p].TurnCompleted < _gameTurnStarted)
                    {
                        wi.TurnComplete = true;
                        return wi;
                    }
                    else
                    {
                        wi.Turn = _pstates[p].TurnCompleted;
                        return wi;
                    }
                }
                if (_pstates[p].TurnCompleted == refTurn)
                {
                    wi.Turn = refTurn;
                    return wi;
                }
                if (playerExitTest(wi))
                    return wi;
                if (_pstates[p].TurnCompleted != refTurn - 1)
                    throw new ApplicationException(string.Format("Player is confusing turns: completed={0} refTurn={1}", _pstates[p].TurnCompleted, refTurn));
                if (refTurn > _gameTurnStarted)
                    throw new ApplicationException(string.Format("Player skipping ahead of game progress: gameTurnStarted={0} refTurn={1}", _gameTurnStarted, refTurn));
                _pstates[p].TurnCompleted = _gameTurnStarted;
                _pstates[p].TurnFinTime = DateTime.Now;
                int totalMsec = (int)(_pstates[p].TurnFinTime - _turnStart).TotalMilliseconds;
                if (totalMsec > 1000)
                    _pstates[p].PenaltyPoints += (totalMsec - 1000) / 100;
                else
                    _pstates[p].BonusPoints += (1000 - totalMsec) / 100;
                if (totalMsec > Settings.TurnResponseThresholdMsec)
                {
                    _pstates[p].OvertimeTurnMsec = totalMsec;
                    _pstates[p].OvertimeTurnTurn = _pstates[p].TurnCompleted;
                }
                if (_pstates[p].PenaltyPoints > Settings.PenaltyPointsThreshold && _pstates[p].PenaltyThresholdReachedTurn < 0)
                    _pstates[p].PenaltyThresholdReachedTurn = _pstates[p].TurnCompleted;
                _proto.LogPlayerTurnComplete(_pstates[p], _turnStart);
                completeTurn();
                if (playerExitTest(wi))
                    return wi;
                wi.Turn = refTurn;
                return wi;
            }
        }

        private int makeTimingPoints(int time)
        {
            return time / 100;
        }

        private bool startNextTurnMaybe()
        {
            if (_gameTurnStarted > _gameTurnEnded)
                return true;
            if (State != GameState.PLAY)
                return false;
            if (DateTime.Now < _turnEnd)
                return false;
            _gameTurnStarted = _gameTurnEnded + 1;
            prepareTurnTimings();
            Monitor.PulseAll(_liveLock);
            _proto.LogGameTurnStart(_gameTurnStarted);
            return true;
        }

        private void changeTile(int row, int col, TileType val, List<MapChange> changeLog)
        {
            _map.Tiles[row, col] = val;
            changeLog.Add(new MapChange(row, col, val));
        }

        private void completeTurn()
        {
            if (!startNextTurnMaybe())
                return;
            if (_pstates.Any(t => t.IsActive && t.TurnCompleted < _gameTurnStarted))
                return;
            // Move all heads to new positions
            Point[] necks = _pstates.Select(p => p.Head).ToArray();
            for (int p = 0; p < _pstates.Length; p++)
            {
                if (!_pstates[p].IsActive)
                    continue;
                Point oldHead = _pstates[p].Head;
                Point newHead = _pstates[p].TurnPosition;
                bool good = true;
                string reason = "OK";
                do
                {
                    if (newHead.Equals(oldHead))
                    {
                        good = false;
                        reason = "Did not move";
                        break;
                    }
                    if (newHead.Row != oldHead.Row
                        && newHead.Row != (oldHead.Row + 1) % _map.Height
                        && newHead.Row != (oldHead.Row - 1 + _map.Height) % _map.Height)
                    {
                        good = false;
                        reason = "Invalid row change";
                        break;
                    }
                    if (newHead.Col != oldHead.Col
                        && newHead.Col != (oldHead.Col + 1) % _map.Width
                        && newHead.Col != (oldHead.Col - 1 + _map.Width) % _map.Width)
                    {
                        good = false;
                        reason = "Invalid col change";
                        break;
                    }
                    if (newHead.Row != oldHead.Row
                        && newHead.Col != oldHead.Col)
                    {
                        good = false;
                        reason = "Invalid move";
                        break;
                    }
                    if (_map.Tiles[newHead.Row, newHead.Col] != TileType.EMPTY)
                    {
                        good = false;
                        reason = "Move to non-empty space";
                        break;
                    }
                } while (false);
                if (!good)
                {
                    _pstates[p].Condition = PlayerCondition.DRAW;
                    _pstates[p].Comment = reason;
                    _proto.LogPlayerCondition(_pstates[p]);
                    continue;
                }
                _pstates[p].Head = newHead;
            }
            // Check for head-to-head collisions
            for (int i = 0; i < _pstates.Length - 1; i++)
            {
                if (!_pstates[i].IsActive)
                    continue;
                for (int j = i + 1; j < _pstates.Length; j++)
                {
                    if (!_pstates[j].IsActive)
                        continue;

                    if (!_pstates[i].Head.Equals(_pstates[j].Head))
                        continue;

                    _pstates[i].Condition = PlayerCondition.DRAW;
                    _pstates[i].Comment = "Head-to-head collision";
                    _proto.LogPlayerCondition(_pstates[i]);

                    _pstates[j].Condition = PlayerCondition.DRAW;
                    _pstates[j].Comment = "Head-to-head collision";
                    _proto.LogPlayerCondition(_pstates[j]);
                }

            }
            // Update map
            List<MapChange> mapChanges = new List<MapChange>();
            for (int p = 0; p < _pstates.Length; p++)
            {
                Point head = _pstates[p].Head;
                Point neck = necks[p];
                if (head.Equals(neck))
                    continue;
                changeTile(head.Row, head.Col, TileType.HEAD0 + (byte)p, mapChanges);
                changeTile(neck.Row, neck.Col, TileType.BODY0 + (byte)p, mapChanges);
            }
            // Check game finish condition
            int activeCount = _pstates.Count(p => p.IsActive);
            if (activeCount > 1)
            {
                // Game continues
                foreach (PlayerState p in _pstates)
                {
                    if (p.Condition == PlayerCondition.DRAW)
                    {
                        p.Condition = PlayerCondition.LOST;
                        _proto.LogPlayerCondition(p);
                    }
                }
            }
            else
            {
                // Game finishes
                State = GameState.FINISH;
                if (activeCount == 1)
                {
                    // We have a winner
                    PlayerState winner = _pstates.First(p => p.IsActive);
                    winner.Condition = PlayerCondition.WON;
                    _proto.LogPlayerCondition(winner);
                    foreach (PlayerState p in _pstates)
                    {
                        if (p.Condition == PlayerCondition.DRAW)
                        {
                            p.Condition = PlayerCondition.LOST;
                            _proto.LogPlayerCondition(p);
                        }
                    }
                }
            }
            // Complete the turn
            _gameTurnEnded = _gameTurnStarted;
            _proto.LogGameTurnEnd(_gameTurnEnded);
            // Notify observers
            ObservedTurnInfo ot = new ObservedTurnInfo
            {
                Turn = _gameTurnEnded,
                GameState = State,
                PlayerStates = _pstates.Select(p => new PlayerStateInfo(p)).ToArray(),
                MapChanges = mapChanges.ToArray()
            };
            bool haveObservers = false;
            foreach (ObserverQueue queue in _observers)
            {
                if (queue.Push(ot))
                    haveObservers = true;
            }
            if (haveObservers)
                Monitor.PulseAll(_liveLock);
            // Continue
            startNextTurnMaybe();
        }

        public void WaitNextTurn(WaitTurnInfo wi)
        {
            lock (_liveLock)
            {
                if (playerExitTest(wi))
                    return;
                if (_gameTurnStarted > wi.Turn)
                {
                    wi.TurnComplete = true;
                    return;
                }
                checkRunState();
                DateTime dtNow = DateTime.Now;
                if (_turnEnd > dtNow) {
                    int waitMillis = (int) (_turnEnd - dtNow).TotalMilliseconds;
                    if (waitMillis > Settings.NextTurnPollTimeoutMillis)
                        waitMillis = Settings.NextTurnPollTimeoutMillis;
                    else if (waitMillis < Settings.MinimumSleepMillis)
                        waitMillis = Settings.MinimumSleepMillis;
                    Monitor.Wait(_liveLock, waitMillis);
                }
                else
                {
                    Monitor.Wait(_liveLock, Settings.NextTurnPollTimeoutMillis);
                }

                startNextTurnMaybe();

                if (playerExitTest(wi))
                    return;
                if (_gameTurnStarted > wi.Turn)
                    wi.TurnComplete = true;
            }
        }

        public GameResultInfo GetTurnResult()
        {
            lock (_liveLock)
            {
                return new GameResultInfo
                {
                    Turn = _gameTurnStarted,
                    GameState = State,
                    PlayerStates = _pstates.Select(p => new PlayerStateInfo(p)).ToArray()
                };
            }
        }

        public void DropPlayer(Player player, string reason)
        {
            lock (_liveLock)
            {
                checkRunState();
                int p = findPlayer(player.PlayerId);
                if (!_pstates[p].IsPresent)
                    return; // It can't happen but we should be ok anyway
                if (_pstates[p].IsActive)
                {
                    _pstates[p].Condition = PlayerCondition.LOST;
                    _pstates[p].Comment = string.Format("Dropped from the game ({0})", reason);
                    _proto.LogPlayerCondition(_pstates[p]);
                    completeTurn();
                }
                _pstates[p].IsPresent = false;
                _proto.LogPlayerDrop(p, player.PlayerId, reason);
                player.Game = null;
            }
        }

        public void Pause()
        {
            lock (_liveLock)
            {
                checkPlayState();
                State = GameState.PAUSE;
            }
        }

        public void Resume()
        {
            lock (_liveLock)
            {
                checkPlayState();
                State = GameState.PLAY;
                Monitor.PulseAll(_liveLock);
            }
        }

        private void removeObserver(int observerId)
        {
            _observers.RemoveAll(q => q.Observer.ObserverId == observerId);
        }

        private GameViewInfo addObserver(Observer observer)
        {
            _observers.Add(new ObserverQueue(observer));
            return GetGameView(-1);
        }

        public GameViewInfo StartObserving(Observer observer)
        {
            lock (_liveLock)
            {
                checkRunState();
                removeObserver(observer.ObserverId);
                return addObserver(observer);
            }
        }

        public ObservedGameInfo ObserveNextTurn(Observer observer)
        {
            lock (_liveLock)
            {
                checkRunState();
                ObserverQueue q = findObserver(observer.ObserverId);
                ObservedTurnInfo ot = q.Pop();
                if (ot == null && q.IsLive)
                {
                    Monitor.Wait(_liveLock, Settings.ObserverPollTimeoutMillis);
                    ot = q.Pop();
                }
                ObservedGameInfo gi = new ObservedGameInfo
                {
                    GameId = GameId,
                    GameState = State,
                    QueuedTurns = (q.IsLive) ? q.Count : -1,
                    TurnInfo = ot
                };
                return gi;
            }
        }

        public GameLiveInfo GetLiveInfo()
        {
            lock (_liveLock)
            {
                checkRunState();
                GameLiveInfo gi = new GameLiveInfo
                {
                    GameState = State,
                    Turn = _gameTurnStarted,
                    TurnStartTime = _turnStart
                };
                gi.PlayerStates = _pstates.Select(p => new PlayerLiveInfo
                {
                    PlayerId = p.PlayerId,
                    Team = _players[p.Index].Team,
                    Name = _players[p.Index].Name,
                    Condition = p.Condition,
                    Comment = p.Comment,
                    TurnCompleted = p.TurnCompleted,
                    TurnFinTime = p.TurnFinTime,
                    PenaltyPoints = p.PenaltyPoints,
                    BonusPoints = p.BonusPoints,
                    OvertimeTurnMsec = p.OvertimeTurnMsec,
                    OvertimeTurnTurn = p.OvertimeTurnTurn,
                    PenaltyThresholdReachedTurn = p.PenaltyThresholdReachedTurn
                }).ToArray();
                return gi;
            }
        }
    }

    public enum GameState
    {
        SETUP, PLAY, PAUSE, FINISH
    }

    public class PlayerState
    {
        public int Index;
        public int PlayerId;
        public PlayerCondition Condition;
        public bool IsActive { get { return Condition == PlayerCondition.PLAY; } }
        public bool IsPresent;
        public string Comment;
        public Point Head;
        public int TurnCompleted;
        public Point TurnPosition;
        
        public DateTime TurnFinTime;
        public int PenaltyPoints;
        public int BonusPoints;
        public int OvertimeTurnMsec;
        public int OvertimeTurnTurn;
        public int PenaltyThresholdReachedTurn;
    }

    public enum PlayerCondition
    {
        PLAY, WON, LOST, DRAW
    }

}
