using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace BeatLibrary
{
    public class App
    {
        public MocBeatSaverApi BeatSaverApi = new MocBeatSaverApi();


        #region Singleton

        private static readonly Lazy<App> Singleton = new Lazy<App>(() => new App());

        public static App Instance => Singleton.Value;

        private App()
        {
        }

        #endregion

    }

    public class MocBeatSaverApi
    {
        public async Task<Beatmap> Key(string key)
        {
            return new Beatmap
            {
                Name = "BeatSaverApi map"
            };
        }
    }
}