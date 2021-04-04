using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeatLibrary
{
    internal class Scanner
    {
        private readonly IProgress<int> _progress;

        Regex rgx = new Regex(@"([\w\d]+) \(.*\)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Scanner()
        {
        }

        public Scanner(IProgress<int> sw)
        {
            _progress = sw;
        }

        internal static int CountBeatmaps()
        {
            return Directory.Exists(Settings.Instance.GamePath)
                ? Directory.GetDirectories(Settings.Instance.GamePath).Length
                : -1;
        }

        internal async Task<List<Beatmap>> ScanAll()
        {
            if (!Directory.Exists(Settings.Instance.GamePath))
                return null;
            
            var i = 0;

            var maps = Directory.GetDirectories(Settings.Instance.GamePath);
            var beatmaps = new List<Beatmap>();

            foreach (var map in maps)
            {
                var bm = await Scan(map);
                if (bm != null)
                    beatmaps.Add(bm);
                _progress?.Report(i++);
            }

            return beatmaps;
        }

        internal async Task<Beatmap> Scan(string path)
        {
            if (!Directory.Exists(path))
                return null;
            
            var x = rgx.Match(path);
            if (x.Success && x.Groups.Count > 1)
                return await App.Instance.BeatSaverApi.Key(x.Groups[1].Value);


            if (!File.Exists($"{path}\\info.dat")) return null;

            var dat = File.ReadAllText($"{path}\\info.dat");
            var lbm = JsonConvert.DeserializeObject<LocalBeatmap>(dat);
            var hash = HashBeatmap(path, lbm);

            return new Beatmap
            {
                Name = lbm.SongName,
                Description = lbm.SongSubName,
                AvailableOnBeatSaver = false,
                CoverURL = $"{path}\\{lbm.CoverImageFilename}",
                Hash = hash,
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
            };
        }

        internal string HashBeatmap(string path, LocalBeatmap beatmap)
        {
            try
            {
                var infoDatStr = new List<byte>(File.ReadAllBytes($"{path}\\info.dat"));

                var binary = infoDatStr;
                foreach (var diffSet in beatmap.DifficultyBeatmapSets)
                foreach (var d in diffSet.DifficultyBeatmaps)
                {
                    binary.AddRange(File.ReadAllBytes(($"{path}\\{d.Filename}")));
                }

                var hash = new SHA1Managed().ComputeHash(binary.ToArray());
                return string.Concat(hash.Select(b => b.ToString("X2")));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #region Localbm

        public class LocalBeatmap
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

        public class LocalBeatmapCustomData
        {
            [JsonProperty("_contributors")] public object[] Contributors { get; set; }
        }

        public class DifficultyBeatmapSet
        {
            [JsonProperty("_beatmapCharacteristicName")]
            public string Name { get; set; }

            [JsonProperty("_difficultyBeatmaps")] public DifficultyBeatmap[] DifficultyBeatmaps { get; set; }
        }

        public class DifficultyBeatmap
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