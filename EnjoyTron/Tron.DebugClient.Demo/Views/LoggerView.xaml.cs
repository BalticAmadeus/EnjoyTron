using System.Windows.Controls;
using Tron.DebugClient.Demo.ViewModel;

namespace Tron.DebugClient.Demo.Views
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
