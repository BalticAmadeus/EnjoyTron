using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameLogic
{
    /// <summary>
    /// Locking strategy for live game access (e.g. by players): _gameLock -> _liveLock or _gameLock, _liveLock
    /// </summary>
    public class GameManager
    {
        private Dictionary<int, Game> _games;
        private Dictionary<int, Player> _players;
        private Dictionary<int, Observer> _observers;
        private object _gameLock;
        private int nextGameId;
        private int nextPlayerId;
        private int nextObserverId;

        public GameManager()
        {
            _games = new Dictionary<int, Game>();
            _players = new Dictionary<int, Player>();
            _observers = new Dictionary<int, Observer>();
            _gameLock = new object();
            nextGameId = 1;
            nextPlayerId = 1;
            nextObserverId = 1;
        }

        public GameInfo[] ListGames(Team team)
        {
            lock (_gameLock)
            {
                return _games.Values.Where(g => team.PowerTeam || g.Owner.Equals(team)).Select(g => new GameInfo(g)).ToArray();
            }
        }

        public GameInfo CreateGame(Team owner)
        {
            lock (_gameLock)
            {
                if (!owner.PowerTeam)
                {
                    int count = _games.Values.Count(g => g.Owner.Equals(owner));
                    if (count >= Settings.MaxGamesPerTeam)
                        throw new ApplicationException("Maximum limit of games per owner team has been reached");
                }
                var game = new Game(nextGameId++, owner);
                _games[game.GameId] = game;
                return new GameInfo(game);
            }
        }

        public GameDetails GetGameDetails(int gameId, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                return new GameDetails(game);
            }
        }

        public PlayerInfo[] ListPlayers(Team team)
        {
            lock (_gameLock)
            {
                return _players.Values.Where(p => team.PowerTeam || p.Team.Equals(team)).Select(p => new PlayerInfo(p)).ToArray();
            }
        }

        public PlayerInfo CreatePlayer(Team team, string name)
        {
            lock (_gameLock)
            {
                Player player = _players.Values.FirstOrDefault(p => p.Name == name && p.Team.Equals(team));
                if (player == null)
                {
                    player = new Player(nextPlayerId++, team, name);
                    _players[player.PlayerId] = player;
                }
                return new PlayerInfo(player);
            }
        }

        public ObserverInfo CreateObserver(Team team, string name)
        {
            lock (_gameLock)
            {
                Observer observer = _observers.Values.FirstOrDefault(p => p.Name == name && p.Team.Equals(team));
                if (observer == null)
                {
                    observer = new Observer(nextObserverId++, team, name);
                    _observers[observer.ObserverId] = observer;
                }
                return new ObserverInfo(observer);
            }
        }

        public GameInfo WaitGameStart(int playerId, ClientCode clientCode)
        {
            lock (_gameLock)
            {
                for (int i = 0; i < 2; i++)
                {
                    Player player = getPlayer(playerId);
                    checkPlayerAccess(player, clientCode);
                    Game game = player.Game;
                    if (game != null && game.State != GameState.SETUP)
                        return new GameInfo(game);
                    if (i == 0)
                        Monitor.Wait(_gameLock, Settings.GameStartPollTimeoutMillis);
                }
                return null;
            }
        }

        public void AddGamePlayer(int gameId, int playerId, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                Player player = getPlayer(playerId);
                checkPlayerAccess(player, team);
                game.AddPlayer(player);
            }
        }

        public void RemoveGamePlayer(int gameId, int playerId, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                Player player = getPlayer(playerId);
                game.RemovePlayer(player);
            }
        }

        private Game getGame(int gameId)
        {
            Game game;
            if (!_games.TryGetValue(gameId, out game))
                throw new ApplicationException("Game not found");
            return game;
        }

        private Player getPlayer(int playerId)
        {
            Player player;
            if (!_players.TryGetValue(playerId, out player))
                throw new ApplicationException("Player not found");
            return player;
        }

        private Observer getObserver(int observerId, ClientCode clientCode)
        {
            Observer observer;
            if (!_observers.TryGetValue(observerId, out observer))
                throw new ApplicationException("Observer not found");
            if (observer.Name != clientCode.ClientName || observer.Team.Name != clientCode.TeamName)
                throw new UnauthorizedAccessException();
            return observer;
        }

        private void checkGameAccess(Game game, Team team)
        {
            if (!team.PowerTeam && !game.Owner.Equals(team))
                throw new UnauthorizedAccessException();
        }

        private void checkPlayerAccess(Player player, ClientCode clientCode)
        {
            if (player.Name != clientCode.ClientName || player.Team.Name != clientCode.TeamName)
                throw new UnauthorizedAccessException();
        }

        private void checkPlayerAccess(Player player, Team team)
        {
            if (!team.PowerTeam && !player.Team.Equals(team))
                throw new UnauthorizedAccessException();
        }

        public void SetGameMap(int gameId, MapData mapData, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                game.SetMap(mapData);
            }
        }

        public void StartGame(int gameId, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                game.Start();
                Monitor.PulseAll(_gameLock);
            }
        }

        private Game accessLiveGame(int playerId, ClientCode clientCode)
        {
            lock (_gameLock)
            {
                Player player = getPlayer(playerId);
                checkPlayerAccess(player, clientCode);
                Game game = player.Game;
                if (game == null)
                    throw new ApplicationException("Player is not in a game");
                game.checkRunState();
                return game;
            }
        }

        private void accessObservedGame(int observerId, int gameId, ClientCode clientCode, Team team, out Observer observer, out Game game)
        {
            lock (_gameLock)
            {
                observer = getObserver(observerId, clientCode);
                game = getGame(gameId);
                if (team != null)
                    checkGameAccess(game, team);
                game.checkRunState();
            }
        }

        public GameViewInfo GetPlayerView(int playerId, ClientCode clientCode)
        {
            Game game = accessLiveGame(playerId, clientCode);
            return game.GetGameView(playerId);
        }

        public void PerformMove(int playerId, Point position, ClientCode clientCode)
        {
            Game game = accessLiveGame(playerId, clientCode);
            game.PerformMove(playerId, position);
        }

        public WaitTurnInfo WaitNextTurn(int playerId, int refTurn, ClientCode clientCode)
        {
            Game game = accessLiveGame(playerId, clientCode);
            WaitTurnInfo wi = game.CompletePlayerTurn(playerId, refTurn);
            if (!wi.TurnComplete)
                game.WaitNextTurn(wi);
            if (wi.GameFinished)
                Thread.Sleep(Settings.LastWaitNextTurnSleepMillis);
            return wi;
        }

        public GameResultInfo GetTurnResultForPlayer(int playerId, ClientCode clientCode)
        {
            Game game = accessLiveGame(playerId, clientCode);
            return game.GetTurnResult();
        }

        public void DropPlayer(int playerId, int gameId, ClientCode clientCode, Team team)
        {
            lock (_gameLock)
            {
                Player player = getPlayer(playerId);
                if (team != null)
                    checkPlayerAccess(player, team);
                else
                    checkPlayerAccess(player, clientCode);
                Game game = player.Game;
                if (game == null)
                    throw new ApplicationException("Player is not in a game");
                if (gameId > 0 && game.GameId != gameId)
                    throw new ApplicationException("Player is not in the specified game");
                game.DropPlayer(player, clientCode.ToString());
            }
        }

        public void PauseGame(int gameId, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                game.Pause();
            }
        }

        public void ResumeGame(int gameId, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                game.Resume();
            }
        }

        public GameViewInfo StartObserving(int observerId, int gameId, ClientCode clientCode, Team team)
        {
            Observer observer;
            Game game;
            accessObservedGame(observerId, gameId, clientCode, team, out observer, out game);
            return game.StartObserving(observer);
        }

        public ObservedGameInfo ObserveNextTurn(int observerId, int gameId, ClientCode clientCode)
        {
            Observer observer;
            Game game;
            accessObservedGame(observerId, gameId, clientCode, null, out observer, out game);
            return game.ObserveNextTurn(observer);
        }

        public void DeleteGame(int gameId, Team team)
        {
            lock (_gameLock)
            {
                Game game = getGame(gameId);
                checkGameAccess(game, team);
                game.CheckGameDeletable();
                _games.Remove(game.GameId);
                game.Dispose();
            }
        }

        public GameLiveInfo GetLiveInfo(int gameId, Team team)
        {
            Game game;
            lock (_gameLock)
            {
                game = getGame(gameId);
                checkGameAccess(game, team);
            }
            return game.GetLiveInfo();
        }

    }
}
