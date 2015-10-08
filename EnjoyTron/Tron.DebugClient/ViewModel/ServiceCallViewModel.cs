using System.ComponentModel;
using Prism.Mvvm;
using Tron.DebugClient.Infrastructure;
using Tron.DebugClient.Utilites;

namespace Tron.DebugClient.ViewModel
{
    public abstract class ServiceCallViewModel : BindableBase
    {
        protected readonly ICommonDataManager CommonDataManager;
        protected readonly IServiceCallInvoker ServiceCallInvoker;

        protected ServiceCallViewModel(ICommonDataManager commonDataManager, IServiceCallInvoker serviceCallInvoker)
        {
            CommonDataManager = commonDataManager;
            ServiceCallInvoker = serviceCallInvoker;

            PropertyChanged += OnPropertyChanged;

            CommonDataManager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "ServerUrl")
                    ServiceUrl = CommonDataManager.ServerUrl;

                if (args.PropertyName == "Username")
                    Username = CommonDataManager.Username;

                if (args.PropertyName == "TeamName")
                    TeamName = CommonDataManager.TeamName;

                if (args.PropertyName == "Password")
                    Password = CommonDataManager.Password;

                if (args.PropertyName == "SessionId")
                    SessionId = CommonDataManager.SessionId;

                if (args.PropertyName == "SequenceNumber")
                    SequenceNumber = CommonDataManager.SequenceNumber;
            };

            ServiceUrl = CommonDataManager.ServerUrl;
            Username = CommonDataManager.Username;
            TeamName = CommonDataManager.TeamName;
            Password = CommonDataManager.Password;
            SessionId = CommonDataManager.SessionId;
            SequenceNumber = CommonDataManager.SequenceNumber;
        }

        public abstract string Title { get; }

        private string _serviceUrl;
        public string ServiceUrl
        {
            get { return _serviceUrl; }
            set { SetProperty(ref _serviceUrl, value); }
        }
        
        private string _username;
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        private string _teamName;
        public string TeamName
        {
            get { return _teamName; }
            set { SetProperty(ref _teamName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private int _sessionId;
        public int SessionId
        {
            get { return _sessionId; }
            set { SetProperty(ref _sessionId, value); }
        }

        private int _sequenceNumber;
        public int SequenceNumber
        {
            get { return _sequenceNumber; }
            set { SetProperty(ref _sequenceNumber, value); }
        }

        private string _authCode;
        public string AuthCode
        {
            get { return _authCode; }
            set { SetProperty(ref _authCode, value); }
        }

        private string _authCodeString;
        public string AuthCodeString
        {
            get { return _authCodeString; }
            set { SetProperty(ref _authCodeString, value); }
        }

        protected void CalculateAuthCode()
        {
            AuthCodeString = string.Format("{0}:{1}:{2}:{3}{4}", TeamName, Username, SessionId, SequenceNumber, Password);
            AuthCode = AuthCodeManager.GetAuthCode(AuthCodeString); 
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "AuthCode")
                return;

            CalculateAuthCode();
        }
    }
}