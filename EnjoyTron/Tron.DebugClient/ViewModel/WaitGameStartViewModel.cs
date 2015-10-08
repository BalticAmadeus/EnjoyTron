using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Tron.DebugClient.ClientService;
using Tron.DebugClient.Infrastructure;

namespace Tron.DebugClient.ViewModel
{
    public class WaitGameStartViewModel : ServiceCallViewModel
    {
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public WaitGameStartViewModel(
            ICommonDataManager commonDataManager,
            IServiceCallInvoker serviceCallInvoker,
            IMessageBoxDialogService messageBoxDialogService)
            : base(commonDataManager, serviceCallInvoker)
        {
            _messageBoxDialogService = messageBoxDialogService;
            CommonDataManager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "PlayerId")
                    PlayerId = CommonDataManager.PlayerId;
            };

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
                            var clientService = new ClientServiceClient(new BasicHttpBinding(),
                                new EndpointAddress(ServiceUrl));

                            var req = new WaitGameStartReq
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

                            var waitGameStartResp =
                                await ServiceCallInvoker.InvokeAsync(req, clientService.WaitGameStartAsync);
                            if (!waitGameStartResp.IsOk())
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

        public override string Title
        {
            get { return "Wait Game Start"; }
        }
    }
}
