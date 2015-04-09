using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Regions;
using Tron.AdminClient.ViewModels;

namespace Tron.AdminClient.Views
{
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class GamePreviewView : UserControl
    {
        public GamePreviewView(GamePreviewViewModel gamePreviewViewModel)
        {
            InitializeComponent();

            DataContext = gamePreviewViewModel;
        }

        private void GamePreviewViewLoaded(object sender, RoutedEventArgs e)
        {
            Zoombox.FitToBounds();
        }

        private void OnPlayerDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item == null)
                return;

            ((GamePreviewViewModel)DataContext).DropPlayerCommand.Command.Execute(item.DataContext);
        }
    }
}
