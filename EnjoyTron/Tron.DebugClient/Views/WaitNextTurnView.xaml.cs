using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class WaitNextTurnView : UserControl
    {
        public WaitNextTurnView(WaitNextTurnViewModel waitNextTurnViewModel)
        {
            InitializeComponent();

            DataContext = waitNextTurnViewModel;
        }
    }
}
