using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel;

namespace Tron.DebugClient.Demo.Views
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
