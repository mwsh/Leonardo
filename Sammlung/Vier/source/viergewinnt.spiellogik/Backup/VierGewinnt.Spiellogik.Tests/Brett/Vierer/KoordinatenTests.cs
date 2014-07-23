using NUnit.Framework;
using VierGewinnt.Spiellogik.Brett.Vierer;

namespace VierGewinnt.Spiellogik.Tests.Brett.Vierer
{
    [TestFixture]
    public class KoordinatenTests
    {
        [Test]
        public void Zwei_Koordinaten_mit_gleichen_Werten_sind_gleich() {
            var k1 = new Koordinate(1, 2);
            var k2 = new Koordinate(1, 2);
            Assert.That(k1, Is.EqualTo(k2));
        }

        [Test]
        public void Zwei_Koordinaten_mit_unterschiedlichen_Werten_sind_ungleich() {
            var k1 = new Koordinate(2, 1);
            var k2 = new Koordinate(1, 2);
            Assert.That(k1, Is.Not.EqualTo(k2));
        }
    }
}