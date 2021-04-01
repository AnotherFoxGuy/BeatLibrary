using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaverSharp;

namespace BeatLibrary
{

    public class Beatmap
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Uploader { get; set; }
        public DateTime Uploaded { get; set; }
        public Metadata Metadata { get; set; }
        public Stats Stats { get; set; }
        public string CoverURL { get; set; }
        public string Hash { get; set; }

        public bool AvailableOnBeatSaver { get; set; }

        public static implicit operator Beatmap(BeatSaverSharp.Beatmap bm) =>
            new Beatmap()
            {
                Key = bm.Key,
                Name = bm.Name,
                Description = bm.Description,
                Uploader = bm.Uploader,
                Uploaded = bm.Uploaded,
                Metadata = bm.Metadata,
                Stats = bm.Stats,
                CoverURL = bm.CoverURL.Contains("http") ? bm.CoverURL : $"https://beatsaver.com{bm.CoverURL}",
                AvailableOnBeatSaver = true
            };
    }


    public class Metadata
    {
        public string SongName { get; set; }
        public string SongSubName { get; set; }
        public string SongAuthorName { get; set; }
        public string LevelAuthorName { get; set; }
        public long Duration { get; set; }
        public float BPM { get; set; }
        public string Automapper { get; set; }
        public Difficulties Difficulties { get; set; }
        public List<BeatmapCharacteristic> Characteristics { get; set; }

        public static implicit operator Metadata(BeatSaverSharp.Metadata bm) =>
            new Metadata()
            {
                SongName = bm.SongName,
                SongSubName = bm.SongSubName,
                SongAuthorName = bm.SongAuthorName,
                LevelAuthorName = bm.LevelAuthorName,
                Duration = bm.Duration,
                BPM = bm.BPM,
                Automapper = bm.Automapper,
                Difficulties = bm.Difficulties,
                Characteristics = bm.Characteristics.Select(ch => (BeatmapCharacteristic) ch).ToList()
            };
    }

    public class Difficulties
    {
        public bool Easy { get; set; }
        public bool Expert { get; set; }
        public bool ExpertPlus { get; set; }
        public bool Hard { get; set; }
        public bool Normal { get; set; }

        public static implicit operator Difficulties(BeatSaverSharp.Difficulties d) =>
            new Difficulties()
            {
                Easy = d.Easy,
                Hard = d.Hard,
                Normal = d.Normal,
                Expert = d.Expert,
                ExpertPlus = d.ExpertPlus
            };
    }

    public class BeatmapCharacteristic
    {
        public string Name { get; set; }
        public Dictionary<string, BeatmapCharacteristicDifficulty> Difficulties { get; set; }

        public static implicit operator BeatmapCharacteristic(BeatSaverSharp.BeatmapCharacteristic bm) =>
            new BeatmapCharacteristic()
            {
                Name = bm.Name,
                Difficulties = bm.Difficulties.ToDictionary(x => x.Key, y => y.Value)
            };
    }

}