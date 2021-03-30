using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace BeatLibrary
{
    public class Settings
    {
        public string Gamepath { get; set; }

        private readonly string _settingsPath = $"{Directory.GetCurrentDirectory()}/settings.json";

        public void SetDefaults()
        {
            Gamepath = "NOT_FOUND";
        }

        public void Load()
        {
            if (File.Exists(_settingsPath))
            {
                try
                {
                    var tmp = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_settingsPath));
                    Gamepath = tmp.Gamepath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    SetDefaults();
                    Save();
                }
            }
            else
            {
                SetDefaults();
                Save();
            }
        }

        public void Save()
        {
            File.WriteAllText(_settingsPath, JsonConvert.SerializeObject(this));
        }

        #region Singleton

        private static readonly Lazy<Settings> LazySettings = new Lazy<Settings>(() => new Settings());

        public static Settings Instance => LazySettings.Value;

        private Settings()
        {
        }

        #endregion

    }
}