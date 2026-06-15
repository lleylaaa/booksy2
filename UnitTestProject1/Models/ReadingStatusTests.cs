using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLibrary.Models;

namespace UnitTestProject1.Models
{
    [TestClass]
    public class ReadingStatusTests
    {
        [TestMethod]
        public void ToText_GivesDutchLabels()
        {
            // B-10-01: de drie toegestane statussen.
            Assert.AreEqual("Wil ik lezen", ReadingStatus.WilIkLezen.ToText());
            Assert.AreEqual("Bezig", ReadingStatus.Bezig.ToText());
            Assert.AreEqual("Gelezen", ReadingStatus.Gelezen.ToText());
        }

        [TestMethod]
        public void FromText_ParsesKnownValues()
        {
            Assert.AreEqual(ReadingStatus.Bezig, ReadingStatusExtensions.FromText("Bezig"));
            Assert.AreEqual(ReadingStatus.Gelezen, ReadingStatusExtensions.FromText("Gelezen"));
            Assert.AreEqual(ReadingStatus.WilIkLezen, ReadingStatusExtensions.FromText("Wil ik lezen"));
        }

        [TestMethod]
        public void FromText_FallsBackToWilIkLezen_ForUnknown()
        {
            // B-10-02: een boek moet altijd een (geldige) status hebben.
            Assert.AreEqual(ReadingStatus.WilIkLezen, ReadingStatusExtensions.FromText("onzin"));
            Assert.AreEqual(ReadingStatus.WilIkLezen, ReadingStatusExtensions.FromText(null));
        }

        [TestMethod]
        public void IsValid_OnlyAcceptsTheThreeStatuses()
        {
            Assert.IsTrue(ReadingStatusExtensions.IsValid("Wil ik lezen"));
            Assert.IsTrue(ReadingStatusExtensions.IsValid("Bezig"));
            Assert.IsTrue(ReadingStatusExtensions.IsValid("Gelezen"));
            Assert.IsFalse(ReadingStatusExtensions.IsValid("Klaar"));
            Assert.IsFalse(ReadingStatusExtensions.IsValid(""));
        }
    }
}
