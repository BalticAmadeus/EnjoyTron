using System.Windows.Controls;
using Tron.DebugClient.ViewModel.Flows;

namespace Tron.DebugClient.Views.Flows
{
    public partial class WaitNextTurnLoopView : UserControl
    {
        public WaitNextTurnLoopView(WaitNextTurnLoopViewModel waitNextTurnViewModel)
        {
            InitializeComponent();

            DataContext = waitNextTurnViewModel;
        }
    }
}
