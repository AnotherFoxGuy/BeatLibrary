using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ScanWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            TextLabel.Content = "";
            _mainWindow = mainWindow;
        }

        async void ScanAll()
        {
            var i = 0;
            var rgx = new Regex(@"([\w\d]+) \(.*\)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);


            var maps = Directory.GetDirectories(Settings.Instance.Gamepath);
            var beatmaps = new List<Beatmap>();

            foreach (var map in maps)
            {
                TextLabel.Content = $"{i++}/{maps.Length}";
                var x = rgx.Match(map);
                if (x.Success && x.Groups.Count > 1)
                {
                    beatmaps.Add(await App.Instance.BeatSaverApi.Key(x.Groups[1].Value));
                }
                else if (File.Exists($"{map}\\info.dat"))
                {
                    var dat = File.ReadAllText($"{map}\\info.dat");
                    var lbm = JsonConvert.DeserializeObject<LocalBeatmap>(dat);

                    beatmaps.Add(new Beatmap
                    {
                        Name = lbm.SongName,
                        Description = lbm.SongSubName,
                        AvailableOnBeatSaver = false,
                        Metadata = new Metadata
                        {
                            Difficulties = new Difficulties
                            {
                                Easy = lbm.DifficultyBeatmapSets.Any(it => it.Name == "Easy"),
                                Hard = lbm.DifficultyBeatmapSets.Any(it => it.Name == "Hard"),
                                Normal = lbm.DifficultyBeatmapSets.Any(it => it.Name == "Normal"),
                                Expert = lbm.DifficultyBeatmapSets.Any(it => it.Name == "Expert"),
                                ExpertPlus = lbm.DifficultyBeatmapSets.Any(it => it.Name == "ExpertPlus")
                            },
                            BPM = lbm.BeatsPerMinute,
                            LevelAuthorName = lbm.LevelAuthorName,
                            SongAuthorName = lbm.SongAuthorName,
                            SongName = lbm.SongName,
                            SongSubName = lbm.SongSubName
                        }
                    });
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

        #region Localbm

        public partial class LocalBeatmap
        {
            [JsonProperty("_version")] public string Version { get; set; }

            [JsonProperty("_songName")] public string SongName { get; set; }

            [JsonProperty("_songSubName")] public string SongSubName { get; set; }

            [JsonProperty("_songAuthorName")] public string SongAuthorName { get; set; }

            [JsonProperty("_levelAuthorName")] public string LevelAuthorName { get; set; }

            [JsonProperty("_beatsPerMinute")] public long BeatsPerMinute { get; set; }

            [JsonProperty("_shuffle")] public long Shuffle { get; set; }

            [JsonProperty("_shufflePeriod")] public double ShufflePeriod { get; set; }

            [JsonProperty("_previewStartTime")] public long PreviewStartTime { get; set; }

            [JsonProperty("_previewDuration")] public long PreviewDuration { get; set; }

            [JsonProperty("_songFilename")] public string SongFilename { get; set; }

            [JsonProperty("_coverImageFilename")] public string CoverImageFilename { get; set; }

            [JsonProperty("_environmentName")] public string EnvironmentName { get; set; }

            [JsonProperty("_songTimeOffset")] public long SongTimeOffset { get; set; }

            [JsonProperty("_customData")] public LocalBeatmapCustomData CustomData { get; set; }

            [JsonProperty("_difficultyBeatmapSets")]
            public DifficultyBeatmapSet[] DifficultyBeatmapSets { get; set; }
        }

        public partial class LocalBeatmapCustomData
        {
            [JsonProperty("_contributors")] public object[] Contributors { get; set; }
        }

        public partial class DifficultyBeatmapSet
        {
            [JsonProperty("_beatmapCharacteristicName")]
            public string Name { get; set; }

            [JsonProperty("_difficultyBeatmaps")] public DifficultyBeatmap[] DifficultyBeatmaps { get; set; }
        }

        public partial class DifficultyBeatmap
        {
            [JsonProperty("_difficulty")] public string Difficulty { get; set; }

            [JsonProperty("_difficultyRank")] public long DifficultyRank { get; set; }

            [JsonProperty("_beatmapFilename")] public string Filename { get; set; }

            [JsonProperty("_noteJumpMovementSpeed")]
            public long NoteJumpMovementSpeed { get; set; }

            [JsonProperty("_noteJumpStartBeatOffset")]
            public long NoteJumpStartBeatOffset { get; set; }
        }

        #endregion
    }
}