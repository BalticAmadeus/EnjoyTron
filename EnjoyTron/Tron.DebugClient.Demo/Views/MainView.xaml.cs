using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel;

namespace Tron.DebugClient.Demo.Views
{
    public partial class MainView : UserControl
    {
        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = mainViewModel;
        }
    }
}
