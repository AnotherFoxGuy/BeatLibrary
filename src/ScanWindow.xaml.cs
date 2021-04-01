using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using BeatSaverSharp;

namespace BeatLibrary
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class ScanWindow : Window
    {
        private readonly MainWindow _mainWindow;

        public ScanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            TextLabel.Content = "1/1";
            _mainWindow = mainWindow;
        }

        async void ScanAll()
        {
            var maps = Directory.GetDirectories(Settings.Instance.Gamepath);

            var rgx = new Regex(@"([\w\d]+) \(.*\)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var beatmaps = new List<Beatmap>();

            foreach (var map in maps)
            {
                var x = rgx.Match(map);
                if (x.Success)
                {
                    var bm = await App.Instance.BeatSaverApi.Key(x.Groups[1].Value);
                    if (bm != null) beatmaps.Add(bm);
                }
                else
                {
                    
                }
            }

            Database.Instance.SaveBeatmaps(beatmaps);

            _mainWindow.BeatmapsListBox.ItemsSource = beatmaps;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScanAll();
            //Close();
        }
    }
}