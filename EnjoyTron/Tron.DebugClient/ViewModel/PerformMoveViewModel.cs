using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Tron.DebugClient.ClientService;
using Tron.DebugClient.Infrastructure;
using Tron.DebugClient.ViewModel.Flows;

namespace Tron.DebugClient.ViewModel
{
    public class PerformMoveViewModel : ServiceCallViewModel
    {
        private readonly IMapService _mapService;
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public PerformMoveViewModel(
            ICommonDataManager commonDataManager, 
            IServiceCallInvoker serviceCallInvoker, 
            IMapService mapService,
            IMessageBoxDialogService messageBoxDialogService)
            : base(commonDataManager, serviceCallInvoker)
        {
            CommonDataManager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "PlayerId")
                    PlayerId = CommonDataManager.PlayerId;
            };

            _mapService = mapService;
            _messageBoxDialogService = messageBoxDialogService;
            PlayerId = CommonDataManager.PlayerId;
            
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
            };

            _mapService.CellChanged += (sender, args) =>
            {
                int index = args.Y*_mapService.Map.First().Length + args.X;

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
        }

        public ICommand ExecuteCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await Task.Run(async () =>
                    {
                        try
                        {
                            var clientService = new ClientServiceClient(new BasicHttpBinding(),
                                new EndpointAddress(ServiceUrl));

                            var req = new PerformMoveReq
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
                                await ServiceCallInvoker.InvokeAsync(req, clientService.PerformMoveAsync);
                            if (!performMoveResp.IsOk())
                                return;

                            CommonDataManager.SequenceNumber++;
                        }
                        catch (Exception e)
                        {
                            _messageBoxDialogService.OpenDialog(e.Message, "Exception occurred");
                        }
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

        private CellViewModel _selectedCell;
        public CellViewModel SelectedCell
        {
            get { return _selectedCell; }
            set
            {
                SetProperty(ref _selectedCell, value);
                if (SelectedCell != null)
                {
                    Row = SelectedCell.Y;
                    Column = SelectedCell.X;
                } 
            }
        }

        private ObservableCollection<PlayerViewModel> _playerCollection;
        public ObservableCollection<PlayerViewModel> PlayerCollection
        {
            get { return _playerCollection; }
            set { SetProperty(ref _playerCollection, value); }
        }

        private ObservableCollection<CellViewModel> _cellCollection;
        public ObservableCollection<CellViewModel> CellCollection 
        {
            get { return _cellCollection; }
            set { SetProperty(ref _cellCollection, value); }
        }

        public override string Title
        {
            get { return "Perform Move"; }
        }
    }
}
