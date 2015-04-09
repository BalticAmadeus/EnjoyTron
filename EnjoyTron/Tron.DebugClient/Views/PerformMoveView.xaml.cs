using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class PerformMoveView : UserControl
    {
        public PerformMoveView(PerformMoveViewModel performMoveViewModel)
        {
            InitializeComponent();

            DataContext = performMoveViewModel;
        }
    }
}
