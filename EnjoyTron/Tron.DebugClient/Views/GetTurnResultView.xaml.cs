using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class GetTurnResultView : UserControl
    {
        public GetTurnResultView(GetTurnResultViewModel getTurnResultViewModel)
        {
            InitializeComponent();

            DataContext = getTurnResultViewModel;
        }
    }
}
