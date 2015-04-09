using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tron.DebugClient.Demo.Infrastructure
{
    public class CommonDataManager : ICommonDataManager
    {
        private readonly ISettingsManager _settingsManager;

        public CommonDataManager(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string TeamName
        {
            get { return _settingsManager.TeamName; }
            set
            {
                _settingsManager.TeamName = value;
                RaisePropertyChanged();
            }
        }

        public string Username
        {
            get { return _settingsManager.Username; }
            set
            {
                _settingsManager.Username = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get { return _settingsManager.Password; }
            set
            {
                _settingsManager.Password = value;
                RaisePropertyChanged();
            }
        }

        public string ServerUrl
        {
            get { return _settingsManager.ServerUrl; }
            set
            {
                _settingsManager.ServerUrl = value;
                RaisePropertyChanged();
            }
        }

        private int _sessionId;
        public int SessionId
        {
            get { return _sessionId; }
            set
            {
                _sessionId = value;
                RaisePropertyChanged();
            }
        }

        private int _sequenceNumber;
        public int SequenceNumber
        {
            get
            {
                return _sequenceNumber;
            }
            set
            {
                _sequenceNumber = value;
                RaisePropertyChanged();
            }
        }

        private string _challenge;
        public string Challenge
        {
            get
            {
                return _challenge;
            }
            set
            {
                _challenge = value;
                RaisePropertyChanged();
            }
        }

        private int _playerId;
        public int PlayerId
        {
            get { return _playerId; }
            set
            {
                _playerId = value;
                RaisePropertyChanged();
            }
        }

        private int _turn;
        public int Turn
        {
            get { return _turn; }
            set
            {
                _turn = value;
                RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, args);
        }
    }
}