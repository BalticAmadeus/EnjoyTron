using Microsoft.Practices.Prism.Mvvm;

namespace Tron.AdminClient.ViewModels
{
    public class PlayerStateViewModel : BindableBase
    {
        private PlayerViewModel _player;
        public PlayerViewModel Player
        {
            get { return _player; }
            set { SetProperty(ref _player, value); }
        }

        private int _colorId;
        public int ColorId
        {
            get { return _colorId; }
            set { SetProperty(ref _colorId, value); }
        }

        private string _condition;
        public string Condition
        {
            get { return _condition; }
            set { SetProperty(ref _condition, value); }
        }

        public override int GetHashCode()
        {
            return Player.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var playerStateViewModel = obj as PlayerStateViewModel;
            if (playerStateViewModel == null)
                return false;

            return GetHashCode() == playerStateViewModel.GetHashCode();
        }
    }
}
