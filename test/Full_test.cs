using System.Linq;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;

namespace BeatLibrary
{
    [TestFixture]
    public class AppTests
    {
        private UIA3Automation _automation;
        private FlaUI.Core.Application _app;

        [SetUp]
        protected void SetUp()
        {
            _app = FlaUI.Core.Application.Launch(Settings.Instance.ExePath);
            _automation = new UIA3Automation();
        }
        
        [TearDown]
        protected void TearDown()
        {
            _app.Close();
        }

        [Test]
        public void StartApp()
        {
            var window = _app.GetMainWindow(_automation);
            Assert.AreEqual("BeatLibrary", window.Title);
        }
        
        [Test]
        public void ClickButton()
        {
            var window = _app.GetMainWindow(_automation);
            var button1 = window.FindFirstDescendant(cf => cf.ByText("Settings"))?.AsButton();
            button1?.Invoke();
            var windows = window.ModalWindows;//_app.GetAllTopLevelWindows(_automation);
            var x = windows.SingleOrDefault((s => s.Title == "Settings"));
            Assert.NotNull(x);
        }
    }
}