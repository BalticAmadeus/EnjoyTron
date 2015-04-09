using System.Windows.Controls;
using Tron.DebugClient.ViewModel.Flows;

namespace Tron.DebugClient.Views.Flows
{
    public partial class PlayerModeFlowView : UserControl
    {
        public PlayerModeFlowView(PlayerModeFlowViewModel playerModeFlowViewModel)
        {
            InitializeComponent();

            DataContext = playerModeFlowViewModel;
        }
    }
}
