using System.Windows.Controls;
using Tron.DebugClient.ViewModel;

namespace Tron.DebugClient.Views
{
    public partial class LoggerView : UserControl
    {
        public LoggerView(LoggerViewModel loggerViewModel)
        {
            InitializeComponent();

            DataContext = loggerViewModel;
        }
    }
}
