using System.Windows.Controls;
using Tron.DebugClient.ViewModel.Flows;

namespace Tron.DebugClient.Views.Flows
{
    public partial class WaitGameStartFlowView : UserControl
    {
        public WaitGameStartFlowView(WaitGameStartFlowViewModel waitGameStartFlowViewModel)
        {
            InitializeComponent();

            DataContext = waitGameStartFlowViewModel;
        }
    }
}
