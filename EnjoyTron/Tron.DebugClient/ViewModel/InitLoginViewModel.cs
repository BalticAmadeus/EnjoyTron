using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Tron.DebugClient.ClientService;
using Tron.DebugClient.Infrastructure;

namespace Tron.DebugClient.ViewModel
{
    public class InitLoginViewModel : ServiceCallViewModel
    {
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public InitLoginViewModel(
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

                            var req = new InitLoginReq
                            {
                                Auth = new ReqAuth
                                {
                                    TeamName = TeamName,
                                    AuthCode = AuthCode,
                                    ClientName = Username,
                                    SequenceNumber = SequenceNumber,
                                    SessionId = SessionId,
                                }
                            };

                            var initLoginResp = await ServiceCallInvoker.InvokeAsync(req, clientService.InitLoginAsync);
                            if (!initLoginResp.IsOk())
                                return;

                            CommonDataManager.Challenge = initLoginResp.Challenge;
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
            get { return "Init Login"; }
        }
    }
}
