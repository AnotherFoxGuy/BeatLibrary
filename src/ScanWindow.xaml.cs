using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using BeatSaverSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeatLibrary
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class ScanWindow : Window
    {
        private readonly MainWindow _mainWindow;
        private Progress<int> _progress;

        private int max;

        public ScanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            TextLabel.Content = "";
            _mainWindow = mainWindow;

            _progress = new Progress<int>(i =>
            {
                TextLabel.Content = $"{i}/{max}";
                ProgressBar.Value = i;
            });

            max = Scanner.CountBeatmaps();
            ProgressBar.Maximum = max;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = new Scanner(_progress);
            var beatmaps = await s.ScanAll();

            Database.Instance.SaveBeatmaps(beatmaps);
            _mainWindow.BeatmapsListBox.ItemsSource = beatmaps;
        }
    }
}