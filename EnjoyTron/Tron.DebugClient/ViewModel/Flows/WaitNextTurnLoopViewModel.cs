using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Tron.DebugClient.ClientService;
using Tron.DebugClient.Infrastructure;

namespace Tron.DebugClient.ViewModel.Flows
{
    public class WaitNextTurnLoopViewModel : ServiceCallViewModel
    {
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public WaitNextTurnLoopViewModel(
            ICommonDataManager commonDataManager,
            IServiceCallInvoker serviceCallInvoker,
            IMessageBoxDialogService messageBoxDialogService)
            : base(commonDataManager, serviceCallInvoker)
        {
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
        }

        private bool _isExecuting;

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
                        }
                        catch (Exception e)
                        {
                            _messageBoxDialogService.OpenDialog(e.Message, "Exception occurred");
                        }
                    });
                });
            }
        }


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

        public override string Title
        {
            get { return "Wait Next Turn Loop"; }
        }
    }
}
