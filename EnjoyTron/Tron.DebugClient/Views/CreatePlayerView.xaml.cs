using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class CreatePlayerView : UserControl
    {
        public CreatePlayerView(CreatePlayerViewModel initLoginViewModel)
        {
            InitializeComponent();

            DataContext = initLoginViewModel;
        }
    }
}
