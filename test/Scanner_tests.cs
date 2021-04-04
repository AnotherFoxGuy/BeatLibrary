using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BeatLibrary
{
    [TestFixture]
    public class ScannerTests
    {
        private Scanner _scanner;

        [SetUp]
        protected void SetUp()
        {
            _scanner = new Scanner();
        }

        [Test]
        public void CountInstalledBeatmaps()
        {
            Assert.AreEqual(Scanner.CountBeatmaps(), 2);
        }

        [Test]
        public async Task ScanSingleMap()
        {
            var path = $"{Settings.Instance.GamePath}\\map";
            var map = await _scanner.Scan(path);
            
            Assert.IsNotNull(map);
            Assert.AreEqual("My map", map.Name);
        }

        [Test]
        public async Task ScanSingleMapOnline()
        {
            var path = $"{Settings.Instance.GamePath}\\e621 (Name - Mapper)";
            var map = await _scanner.Scan(path);
            
            Assert.IsNotNull(map);
            Assert.AreEqual("BeatSaverApi map", map.Name);
        }
        
        [Test]
        public void HashMap()
        {
            var path = $"{Settings.Instance.GamePath}\\map";
            var dat = File.ReadAllText($"{path}\\info.dat");
            
            var map = JsonConvert.DeserializeObject<Scanner.LocalBeatmap>(dat);
            var hash = _scanner.HashBeatmap(path, map);
            
            Assert.AreEqual("892401197FE88215B1C9B977313369891E51CBAA", hash);
        }
        
    }
}