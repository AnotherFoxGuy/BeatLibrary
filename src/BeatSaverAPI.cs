using System;
using System.Threading.Tasks;
using BeatSaverSharp;

namespace BeatLibrary
{
    class BeatSaverAPI
    {
        

        private void Init()
        {
           
        }


        #region Singleton

        private static Lazy<Database> _lazyDatabase;

        public static Database Instance => _lazyDatabase.Value;

        private Database()
        {
            _lazyDatabase = new Lazy<Database>(() => this);
            Init();
        }

        #endregion
    }
}