using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BeatLibrary
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class BeatmapDetailWindow : Window
    {
        public BeatmapDetailWindow(Beatmap beatmap)
        {
            InitializeComponent();
            DataContext = beatmap;
        }
    }
}