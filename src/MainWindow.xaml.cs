using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BeatSaverSharp;

namespace BeatLibrary
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BeatmapsListBox.ItemsSource = Database.Instance.GetAllBeatmaps();
        }

        private void BeatmapsListbox_DoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var win = new BeatmapDetailWindow();
            win.SetBeatmap((Beatmap)BeatmapsListBox.SelectedItem);
            win.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new ScanWindow(this);
            win.Show();
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow();
            win.Show();
        }
    }
}