using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Tron.DebugClient.Demo.ClientService;
using Tron.DebugClient.Demo.Infrastructure;

namespace Tron.DebugClient.Demo.ViewModel
{
    public class CreatePlayerViewModel : ServiceCallViewModel
    {
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public CreatePlayerViewModel(
            ICommonDataManager commonDataManager, 
            IServiceCallInvoker serviceCallInvoker,
            IMessageBoxDialogService messageBoxDialogService) 
            : base(commonDataManager, serviceCallInvoker)
        {
            _messageBoxDialogService = messageBoxDialogService;
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

                            var req = new CreatePlayerReq
                            {
                                Auth = new ReqAuth
                                {
                                    TeamName = TeamName,
                                    AuthCode = AuthCode,
                                    ClientName = Username,
                                    SequenceNumber = SequenceNumber,
                                    SessionId = SessionId,
                                },
                            };

                            var createPlayerResp =
                                await ServiceCallInvoker.InvokeAsync(req, clientService.CreatePlayerAsync);
                            if (!createPlayerResp.IsOk())
                                return;

                            CommonDataManager.SequenceNumber++;
                            CommonDataManager.PlayerId = createPlayerResp.PlayerId;
                        }
                        catch (Exception e)
                        {
                            _messageBoxDialogService.OpenDialog(e.Message, "Exception occurred");
                        }
                    });
                });
            }
        }

        public override string Title
        {
            get { return "Create Player"; }
        }
    }
}
