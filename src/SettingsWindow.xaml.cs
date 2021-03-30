using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BeatLibrary
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            GamePathBox.Text = Settings.Instance.Gamepath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Instance.Gamepath = GamePathBox.Text;
            Settings.Instance.Save();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}