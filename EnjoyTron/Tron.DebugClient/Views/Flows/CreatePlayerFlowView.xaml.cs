using System.Windows.Controls;
using Tron.DebugClient.ViewModel.Flows;

namespace Tron.DebugClient.Views.Flows
{
    public partial class CreatePlayerFlowView : UserControl
    {
        public CreatePlayerFlowView(CreatePlayerFlowViewModel createPlayerFlowViewModel)
        {
            InitializeComponent();

            DataContext = createPlayerFlowViewModel;
        }
    }
}
