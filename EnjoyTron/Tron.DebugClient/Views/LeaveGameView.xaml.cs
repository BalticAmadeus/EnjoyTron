using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
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
