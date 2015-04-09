using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Tron.DebugClient.ClientService;
using Tron.DebugClient.Infrastructure;
using Tron.DebugClient.Utilites;

namespace Tron.DebugClient.ViewModel.Flows
{
    public class WaitGameStartFlowViewModel : ServiceCallViewModel
    {
        private readonly IMessageBoxDialogService _messageBoxDialogService;

        public WaitGameStartFlowViewModel(
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

                if (args.PropertyName == "PlayerId")
                    PlayerId = CommonDataManager.PlayerId;
            };

            ChallengeString = CommonDataManager.Challenge;
            PlayerId = CommonDataManager.PlayerId;

            _isExecuting = false;
        }

        private bool _isExecuting;

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
                            CommonDataManager.SessionId = 0;
                            CommonDataManager.SequenceNumber = 0;

                            var clientService = new ClientServiceClient(new BasicHttpBinding(),
                                new EndpointAddress(ServiceUrl));

                            var initLoginReq = new InitLoginReq
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

                            var initLoginResp =
                                await ServiceCallInvoker.InvokeAsync(initLoginReq, clientService.InitLoginAsync);
                            if (!initLoginResp.IsOk())
                                return;

                            CommonDataManager.Challenge = initLoginResp.Challenge;

                            var completeLoginReq = new CompleteLoginReq
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
                                await ServiceCallInvoker.InvokeAsync(completeLoginReq, clientService.CompleteLoginAsync);
                            if (!completeLoginResp.IsOk())
                                return;

                            CommonDataManager.SequenceNumber++;
                            CommonDataManager.SessionId = completeLoginResp.SessionId;

                            var createPlayerReq = new CreatePlayerReq
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
                                await ServiceCallInvoker.InvokeAsync(createPlayerReq, clientService.CreatePlayerAsync);
                            if (!createPlayerResp.IsOk())
                                return;

                            CommonDataManager.SequenceNumber++;
                            CommonDataManager.PlayerId = createPlayerResp.PlayerId;

                            _isExecuting = true;

                            int gameId = -1;
                            while (gameId == -1 && _isExecuting)
                            {
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
                                gameId = waitGameStartResp.GameId;
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

        private int _playerId;
        public int PlayerId
        {
            get { return _playerId; }
            set { SetProperty(ref _playerId, value); }
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
            get { return "Init Login & Complete Login & Create Player & Wait Game Start"; }
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
