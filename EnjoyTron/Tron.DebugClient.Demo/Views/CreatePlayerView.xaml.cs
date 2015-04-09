using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel;

namespace Tron.DebugClient.Demo.Views
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
