using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class CompleteLoginView : UserControl
    {
        public CompleteLoginView(CompleteLoginViewModel initLoginViewModel)
        {
            InitializeComponent();

            DataContext = initLoginViewModel;
        }
    }
}
