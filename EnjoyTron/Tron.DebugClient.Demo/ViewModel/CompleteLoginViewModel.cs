using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Tron.DebugClient.Demo.ClientService;
using Tron.DebugClient.Demo.Infrastructure;
using Tron.DebugClient.Demo.Utilites;

namespace Tron.DebugClient.Demo.ViewModel
{
    public class CompleteLoginViewModel : ServiceCallViewModel
    {
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public CompleteLoginViewModel(
            ICommonDataManager commonDataManager, 
            IServiceCallInvoker serviceCallInvoker,
            IMessageBoxDialogService messageBoxDialogService)
            : base(commonDataManager, serviceCallInvoker)
        {
            _messageBoxDialogService = messageBoxDialogService;
            
            CommonDataManager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Challenge")
                    ChallengeString = CommonDataManager.Challenge;
            };

            ChallengeString = CommonDataManager.Challenge;
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

                            var req = new CompleteLoginReq
                            {
                                Auth = new ReqAuth
                                {
                                    TeamName = TeamName,
                                    AuthCode = AuthCode,
                                    ClientName = Username,
                                    SequenceNumber = SequenceNumber,
                                    SessionId = SessionId,
                                },
                                ChallengeResponse = Challenge,
                            };

                            var completeLoginResp =
                                await ServiceCallInvoker.InvokeAsync(req, clientService.CompleteLoginAsync);
                            if (!completeLoginResp.IsOk())
                                return;

                            CommonDataManager.SequenceNumber++;
                            CommonDataManager.SessionId = completeLoginResp.SessionId;
                        }
                        catch (Exception e)
                        {
                            _messageBoxDialogService.OpenDialog(e.Message, "Exception occurred");
                        }
                    });
                });
            }
        }

        private string _challenge;
        public string Challenge
        {
            get { return _challenge; }
            set { SetProperty(ref _challenge, value); }
        }

        private string _challengeString;
        public string ChallengeString
        {
            get { return _challengeString; }
            set { SetProperty(ref _challengeString, value); }
        }

        public override string Title
        {
            get { return "Complete Login"; }
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Challenge")
                return;

            Challenge = AuthCodeManager.GetAuthCode(string.Format("{0}{1}",
                AuthCodeManager.GetAuthCode(string.Format("{0}{1}", ChallengeString, Password)), Password));

            base.OnPropertyChanged(sender, args);
        }
    }
}