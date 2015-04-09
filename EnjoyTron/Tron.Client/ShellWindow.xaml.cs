using System;
using System.Reflection;
using System.Windows;

namespace Tron.AdminClient
{
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();

            Title = string.Format("EnjoyTron Admin Client - ver. {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
