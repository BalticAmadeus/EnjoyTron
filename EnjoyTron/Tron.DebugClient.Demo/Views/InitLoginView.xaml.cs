using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel;

namespace Tron.DebugClient.Demo.Views
{
    public partial class InitLoginView : UserControl
    {
        public InitLoginView(InitLoginViewModel initLoginViewModel)
        {
            InitializeComponent();

            DataContext = initLoginViewModel;
        }
    }
}
