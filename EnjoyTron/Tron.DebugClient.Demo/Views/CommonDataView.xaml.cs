using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel;

namespace Tron.DebugClient.Demo.Views
{
    public partial class CommonDataView : UserControl
    {
        public CommonDataView(CommonDataViewModel commonDataViewModel)
        {
            InitializeComponent();

            DataContext = commonDataViewModel;
        }
    }
}
