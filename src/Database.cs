using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeatSaverSharp;
using LiteDB;

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

        public void SaveBeatmap(PocoBeatmap bm)
        {
            _liteDatabase.GetCollection<PocoBeatmap>(LocalBeatmapsCollection)
                .Insert(bm);
        }

        public PocoBeatmap GetBeatmapByKey(string key)
        {
            return _liteDatabase.GetCollection<PocoBeatmap>(LocalBeatmapsCollection)
                .FindOne(i => i.Key == key);
        }

        public PocoBeatmap GetBeatmapByHash(string Hash)
        {
            return _liteDatabase.GetCollection<PocoBeatmap>(LocalBeatmapsCollection)
                .FindOne(i => i.Hash == Hash);
        }

        public IEnumerable<PocoBeatmap> GetAllBeatmaps()
        {
            return _liteDatabase.GetCollection<PocoBeatmap>(LocalBeatmapsCollection)
                .FindAll();
        }

        public void SaveBeatmaps(List<PocoBeatmap> beatmaps)
        {
            _liteDatabase.GetCollection<PocoBeatmap>(LocalBeatmapsCollection)
                .Insert(beatmaps);
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