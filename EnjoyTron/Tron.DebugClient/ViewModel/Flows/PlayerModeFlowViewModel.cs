using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Tron.DebugClient.ClientService;
using Tron.DebugClient.Infrastructure;

namespace Tron.DebugClient.ViewModel.Flows
{
    public class PlayerModeFlowViewModel: ServiceCallViewModel
    {
        private readonly IMapService _mapService;
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public PlayerModeFlowViewModel(
            ICommonDataManager commonDataManager, 
            IServiceCallInvoker serviceCallInvoker, 
            IMapService mapService,
            IMessageBoxDialogService messageBoxDialogService)
            : base(commonDataManager, serviceCallInvoker)
        {
            _mapService = mapService;
            _messageBoxDialogService = messageBoxDialogService;

            PlayerId = CommonDataManager.PlayerId;
            Turn = CommonDataManager.Turn;

            CommonDataManager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Turn")
                    Turn = CommonDataManager.Turn;

                if (args.PropertyName == "PlayerId")
                    PlayerId = CommonDataManager.PlayerId;
            };
            
            _mapService.MapChanged += (sender, args) =>
            {
                var cellViewModels = new List<CellViewModel>();

                for (int i = 0; i < _mapService.Map.Length; i++)
                {
                    for (int j = 0; j < _mapService.Map[i].Length; j++)
                    {
                        cellViewModels.Add(new CellViewModel { X = j, Y = i, State = Convert.ToString(_mapService.Map[i][j]), });
                    }
                }

                CellCollection = new ObservableCollection<CellViewModel>(cellViewModels);
                PlayerCollection = new ObservableCollection<PlayerViewModel>(_mapService.Players.Select(p => new PlayerViewModel
                {
                    Condition = p.Condition,
                    Index = p.Index,
                }));
            };

            _mapService.CellChanged += (sender, args) =>
            {
                int index = args.Y * _mapService.Map.First().Length + args.X;

                CellCollection[index].State = args.State;

                PlayerCollection = new ObservableCollection<PlayerViewModel>(_mapService.Players.Select(p => new PlayerViewModel
                {
                    Condition = p.Condition,
                    Index = p.Index,
                }));
            };

            if (_mapService.Map != null)
            {
                var cellViewModels = new List<CellViewModel>();

                for (int i = 0; i < _mapService.Map.Length; i++)
                {
                    for (int j = 0; j < _mapService.Map[i].Length; j++)
                    {
                        cellViewModels.Add(new CellViewModel { X = j, Y = i, State = Convert.ToString(_mapService.Map[i][j]), });
                    }
                }

                CellCollection = new ObservableCollection<CellViewModel>(cellViewModels);
                PlayerCollection = new ObservableCollection<PlayerViewModel>(_mapService.Players.Select(p => new PlayerViewModel
                {
                    Condition = p.Condition,
                    Index = p.Index,
                }));
            }
            else
            {
                CellCollection = new ObservableCollection<CellViewModel>();
                PlayerCollection = new ObservableCollection<PlayerViewModel>();
            }

            _isExecuting = false;
        }

        private bool _isExecuting;

        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await Task.Run(() =>
                    {
                        _isExecuting = false;
                    });
                });
            }
        }

        private int _playerId;
        public int PlayerId
        {
            get { return _playerId; }
            set { SetProperty(ref _playerId, value); }
        }

        private int _turn;
        public int Turn
        {
            get { return _turn; }
            set { SetProperty(ref _turn, value); }
        }

        private int _column;
        public int Column
        {
            get { return _column; }
            set { SetProperty(ref _column, value); }
        }

        private int _row;
        public int Row
        {
            get { return _row; }
            set { SetProperty(ref _row, value); }
        }

        private ObservableCollection<PlayerViewModel> _playerCollection;
        public ObservableCollection<PlayerViewModel> PlayerCollection
        {
            get { return _playerCollection; }
            set { SetProperty(ref _playerCollection, value); }
        }

        private CellViewModel _selectedCell;
        public CellViewModel SelectedCell
        {
            get { return _selectedCell; }
            set
            {
                SetProperty(ref _selectedCell, value);
                if (SelectedCell == null) 
                    return;

                Row = SelectedCell.Y;
                Column = SelectedCell.X;

                Task.Run(async () =>
                {
                    try
                    {
                        var clientService = new ClientServiceClient(new BasicHttpBinding(),
                            new EndpointAddress(ServiceUrl));

                        var performMoveReq = new PerformMoveReq
                        {
                            Auth = new ReqAuth
                            {
                                TeamName = TeamName,
                                AuthCode = AuthCode,
                                ClientName = Username,
                                SequenceNumber = SequenceNumber,
                                SessionId = SessionId,
                            },
                            PlayerId = PlayerId,
                            Position = new EnPoint
                            {
                                Col = Column,
                                Row = Row
                            }
                        };

                        var performMoveResp =
                            await ServiceCallInvoker.InvokeAsync(performMoveReq, clientService.PerformMoveAsync);
                        if (!performMoveResp.IsOk())
                            return;

                        CommonDataManager.SequenceNumber++;

                        _isExecuting = true;
                        bool isTurnComplete = false;
                        while (!isTurnComplete && _isExecuting)
                        {
                            var waitNextTurnReq = new WaitNextTurnReq
                            {
                                Auth = new ReqAuth
                                {
                                    TeamName = TeamName,
                                    AuthCode = AuthCode,
                                    ClientName = Username,
                                    SequenceNumber = SequenceNumber,
                                    SessionId = SessionId,
                                },
                                PlayerId = PlayerId,
                                RefTurn = Turn,
                            };

                            var waitNextTurnResp = await ServiceCallInvoker.InvokeAsync(waitNextTurnReq,
                                clientService.WaitNextTurnAsync);
                            if (!waitNextTurnResp.IsOk())
                                return;

                            CommonDataManager.SequenceNumber++;
                            isTurnComplete = waitNextTurnResp.TurnComplete;
                        }

                        _isExecuting = false;

                        var req = new GetTurnResultReq
                        {
                            Auth = new ReqAuth
                            {
                                TeamName = TeamName,
                                AuthCode = AuthCode,
                                ClientName = Username,
                                SequenceNumber = SequenceNumber,
                                SessionId = SessionId,
                            },
                            PlayerId = PlayerId,
                        };

                        var getTurnResultResp =
                            await ServiceCallInvoker.InvokeAsync(req, clientService.GetTurnResultAsync);
                        if (!getTurnResultResp.IsOk())
                            return;

                        CommonDataManager.SequenceNumber++;
                        CommonDataManager.Turn = getTurnResultResp.Turn;

                        for (int index = 0; index < getTurnResultResp.PlayerStates.Length; index++)
                        {
                            var playerState = getTurnResultResp.PlayerStates[index];

                            var player = new Player
                            {
                                Index = index,
                                Condition = getTurnResultResp.PlayerStates[index].Condition
                            };
                            _mapService.UpdateCell(playerState.Position.Col, playerState.Position.Row,
                                index.ToString(CultureInfo.InvariantCulture), player);
                        }

                    }
                    catch (Exception e)
                    {
                        _messageBoxDialogService.OpenDialog(e.Message, "Exception occurred");
                    }
                });
            }
        }

        private ObservableCollection<CellViewModel> _cellCollection;
        public ObservableCollection<CellViewModel> CellCollection
        {
            get { return _cellCollection; }
            set { SetProperty(ref _cellCollection, value); }
        }

        public override string Title
        {
            get { return "Player Mode"; }
        }
    }

    public class PlayerViewModel : BindableBase
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        private string _condition;
        public string Condition
        {
            get { return _condition; }
            set { SetProperty(ref _condition, value); }
        }
    }
}
