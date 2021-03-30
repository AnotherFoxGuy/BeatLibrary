using System;
using System.IO;
using System.Windows;
using BeatSaverSharp;
using Newtonsoft.Json;

namespace BeatLibrary
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public BeatSaver BeatSaverApi;

        private void InitApp(object sender, StartupEventArgs e)
        {
            // Setup the client's HTTP User Agent 
            var options = new HttpOptions(
                "BeatLibrary",
                new Version(0, 1, 0)
            );

            // Use this to interact with the API
            BeatSaverApi = new BeatSaver(options);

            Settings.Instance.Load();
        }

        #region Singleton

        private static Lazy<App> _lazyApp;

        public static App Instance => _lazyApp.Value;

        private App()
        {
#if !DEBUG
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            SentrySdk.Init("https://c34f44d72cbc461e9787103e1474f04a@o84816.ingest.sentry.io/5625812");
#endif
            _lazyApp = new Lazy<App>(() => this);
        }

        #endregion
    }
}