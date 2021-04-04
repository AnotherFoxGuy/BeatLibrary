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
            GamePathBox.Text = Settings.Instance.GamePath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Instance.GamePath = GamePathBox.Text;
            Settings.Instance.Save();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}