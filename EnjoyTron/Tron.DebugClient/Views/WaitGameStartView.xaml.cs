using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class WaitGameStartView : UserControl
    {
        public WaitGameStartView(WaitGameStartViewModel waitGameStartViewModel)
        {
            InitializeComponent();

            DataContext = waitGameStartViewModel;
        }
    }
}
