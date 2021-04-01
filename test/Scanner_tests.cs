using System.Threading.Tasks;
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
            var path = $"{Settings.Instance.Gamepath}\\map";
            var map = await _scanner.Scan(path);
            Assert.AreEqual(map.Name, "My map");
        }
    }
}