using System.Reflection;
using System.Windows;

namespace Tron.DebugClient.Demo
{
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();

            Title = string.Format("EnjoyTron Debug Client - ver. {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
