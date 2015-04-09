using Microsoft.Practices.Prism.Mvvm;

namespace Tron.DebugClient.ViewModel
{
    public class CellViewModel : BindableBase
    {
        public const double CellSize = 20;

        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                SetProperty(ref _x, value);
                OnPropertyChanged(() => Left);
            }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                SetProperty(ref _y, value);
                OnPropertyChanged(() => Top);
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public double Left
        {
            get { return _x*CellSize; }
        }

        public double Top
        {
            get { return _y*CellSize; }
        }
    }
}
