using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel;

namespace Tron.DebugClient.Demo.Views
{
    public partial class LeaveGameView : UserControl
    {
        public LeaveGameView(LeaveGameViewModel leaveGameViewModel)
        {
            InitializeComponent();

            DataContext = leaveGameViewModel;
        }
    }
}
