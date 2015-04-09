using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel.Flows;

namespace Tron.DebugClient.Demo.Views.Flows
{
    public partial class CompleteLoginFlowView : UserControl
    {
        public CompleteLoginFlowView(CompleteLoginFlowViewModel completeLoginFlowViewModel)
        {
            InitializeComponent();

            DataContext = completeLoginFlowViewModel;
        }
    }
}
