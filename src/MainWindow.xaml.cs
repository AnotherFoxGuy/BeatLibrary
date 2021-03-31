using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

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
            win.SetBeatmap((PocoBeatmap) BeatmapsListBox.SelectedItem);
            win.Show();
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var maps = Directory.GetDirectories(Settings.Instance.Gamepath);

            var rgx = new Regex(@"([\w\d]+) \(.*\)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (var map in maps)
            {
                var x = rgx.Match(map);
                if (!x.Success) continue;

                var bm = (PocoBeatmap) await App.Instance.BeatSaverApi.Key(x.Groups[1].Value);
                Database.Instance.SaveBeatmap(bm);
            }

            BeatmapsListBox.ItemsSource = Database.Instance.GetAllBeatmaps();
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow();
            win.Show();
        }
    }
}