using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Regions;
using Tron.AdminClient.ViewModels;

namespace Tron.AdminClient.Views
{
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class LobbyView : UserControl
    {
        public LobbyView(LobbyViewModel lobbyViewModel)
        {
            InitializeComponent();

            DataContext = lobbyViewModel;
        }

        private void OnGameDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item == null)
                return;

            ((LobbyViewModel)DataContext).OpenGameCommand.Command.Execute(item.DataContext);
        }
    }
}
