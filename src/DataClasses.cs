using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BeatSaverSharp;

namespace BeatLibrary
{
    public sealed record PocoBeatmap
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Uploader { get; set; }
        public DateTime Uploaded { get; set; }
        public PocoMetadata Metadata { get; set; }
        public Stats Stats { get; set; }
        public string CoverURL { get; set; }
        public string Hash { get; set; }

        public static explicit operator PocoBeatmap(Beatmap bm) =>
            new()
            {
                Key = bm.Key,
                Name = bm.Name,
                Description = bm.Description,
                Uploader = bm.Uploader,
                Uploaded = bm.Uploaded,
                Metadata = (PocoMetadata) bm.Metadata,
                Stats = bm.Stats,
                CoverURL = bm.CoverURL,
            };
    }


    public sealed record PocoMetadata
    {
        public string SongName { get; set; } = null!;
        public string SongSubName { get; set; } = null!;
        public string SongAuthorName { get; set; } = null!;
        public string LevelAuthorName { get; set; } = null!;
        public long Duration { get; set; }
        public float BPM { get; set; }
        public string Automapper { get; set; }
        public Difficulties Difficulties { get; set; } = null!;
        public Collection<PocoBeatmapCharacteristic> Characteristics { get; set; } = null!;

        public static explicit operator PocoMetadata(Metadata bm) =>
            new()
            {
                SongName = bm.SongName,
                SongSubName = bm.SongSubName,
                SongAuthorName = bm.SongAuthorName,
                LevelAuthorName = bm.LevelAuthorName,
                Duration = bm.Duration,
                BPM = bm.BPM,
                Automapper = bm.Automapper,
                Difficulties = bm.Difficulties,
                Characteristics = new(
                    bm.Characteristics.Cast<PocoBeatmapCharacteristic>().ToArray()
                ),
            };
    }

    public sealed record PocoBeatmapCharacteristic
    {
        public string Name { get; set; }
        public Dictionary<string, BeatmapCharacteristicDifficulty> Difficulties { get; set; }

        public static explicit operator PocoBeatmapCharacteristic(BeatmapCharacteristic bm) =>
            new()
            {
                Name = bm.Name,
                Difficulties = bm.Difficulties.ToDictionary(x => x.Key, y => y.Value)
            };
    }
}