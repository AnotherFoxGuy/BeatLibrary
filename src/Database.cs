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

}