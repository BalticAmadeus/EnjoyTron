using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Prism.Mvvm;

namespace Tron.AdminClient.ViewModels
{
    public class AutoRefreshOperation : BindableBase
    {
        private readonly Func<Task> _action;
        private readonly DispatcherTimer _timer;
        
        public AutoRefreshOperation(Func<Task> action, int initialInterval)
        {
            _action = async () =>
            {
                if (!IsAutoRefreshEnabled)
                {
                    IsExecuting = false;
                    return;
                }

                IsExecuting = true;

                await action();
            };

            _timer = new DispatcherTimer();
            _timer.Tick += async (sender, args) => await _action();
            _timer.Interval = TimeSpan.FromMilliseconds(initialInterval);

            IsAutoRefreshEnabled = true;
        }

        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            private set { SetProperty(ref _isExecuting, value); }
        }

        private bool _isAutoRefreshEnabled;
        public bool IsAutoRefreshEnabled
        {
            get { return _isAutoRefreshEnabled; }
            set { SetProperty(ref _isAutoRefreshEnabled, value); }
        }

        public async void Resume()
        {
            await _action();

            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }
    }
}
