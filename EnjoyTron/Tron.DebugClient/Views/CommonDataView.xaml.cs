using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
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
