using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace BeatLibrary
{
    public class Database
    {
        public Beatmap GetBeatmapByKey(string key)
        {
            return null;
        }


        #region Singleton

        private static readonly Lazy<Database> Singleton = new Lazy<Database>(() => new Database());

        public static Database Instance => Singleton.Value;

        private Database()
        {
        }

        #endregion

        
    }

}