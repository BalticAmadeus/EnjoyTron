using System.Windows.Controls;
using Prism.Regions;
using Tron.AdminClient.ViewModels;

namespace Tron.AdminClient.Views
{
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class LoginView : UserControl
    {
        public LoginView(LoginViewModel loginViewModel)
        {
            InitializeComponent();

            DataContext = loginViewModel;
        }
    }
}
