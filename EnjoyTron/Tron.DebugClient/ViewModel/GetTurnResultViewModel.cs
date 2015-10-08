using System;
using System.Globalization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Tron.DebugClient.ClientService;
using Tron.DebugClient.Infrastructure;

namespace Tron.DebugClient.ViewModel
{
    public class GetTurnResultViewModel : ServiceCallViewModel
    {
        private readonly IMapService _mapService;
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public GetTurnResultViewModel(
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
                            var clientService = new ClientServiceClient(
                                new BasicHttpBinding(),
                                new EndpointAddress(ServiceUrl));

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
                });
            }
        }

        private int _playerId;
        public int PlayerId
        {
            get { return _playerId; }
            set { SetProperty(ref _playerId, value); }
        }

        public override string Title
        {
            get { return "Get Turn Result"; }
        }
    }
}
