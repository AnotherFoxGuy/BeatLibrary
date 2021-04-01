using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeatSaverSharp;
using LiteDB;
using Newtonsoft.Json;

namespace BeatLibrary
{
    internal class Database
    {
        private const string LocalBeatmapsCollection = "LocalBeatmaps";

        private LiteDatabase _liteDatabase;

        private void Init()
        {
            var currdir = Directory.GetCurrentDirectory();
            _liteDatabase = new LiteDatabase($"{currdir}/BeatLibrary.db");
        }

        public Beatmap GetBeatmapByKey(string key)
        {
            return _liteDatabase.GetCollection<Beatmap>(LocalBeatmapsCollection)
                .FindOne(i => i.Key == key);
        }

        public Beatmap GetBeatmapByHash(string Hash)
        {
            return _liteDatabase.GetCollection<Beatmap>(LocalBeatmapsCollection)
                .FindOne(i => i.Hash == Hash);
        }

        public IEnumerable<Beatmap> GetAllBeatmaps()
        {
            return _liteDatabase.GetCollection<Beatmap>(LocalBeatmapsCollection)
                .FindAll();
        }

        public void SaveBeatmaps(List<Beatmap> beatmaps)
        {
            _liteDatabase.GetCollection<Beatmap>(LocalBeatmapsCollection)
                .Insert(beatmaps);
        }

        public void SaveBeatmap(Beatmap bm)
        {
            _liteDatabase.GetCollection<Beatmap>(LocalBeatmapsCollection)
                .Insert(bm);
        }


        #region Singleton

        private static readonly Lazy<Database> LazyDatabase = new Lazy<Database>(() => new Database());

        public static Database Instance => LazyDatabase.Value;

        private Database()
        {
            Init();
        }

        #endregion
    }

    #region Classes

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
                CoverURL = bm.CoverURL,
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

    #endregion
}