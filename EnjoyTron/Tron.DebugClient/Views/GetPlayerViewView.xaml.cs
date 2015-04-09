using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class GetPlayerViewView : UserControl
    {
        public GetPlayerViewView(GetPlayerViewViewModel getPlayerViewViewModel)
        {
            InitializeComponent();

            DataContext = getPlayerViewViewModel;
        }
    }
}
