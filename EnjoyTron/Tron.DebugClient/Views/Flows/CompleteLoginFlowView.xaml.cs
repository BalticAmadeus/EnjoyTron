using System.Windows.Controls;
using Tron.DebugClient.ViewModel.Flows;

namespace Tron.DebugClient.Views.Flows
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
