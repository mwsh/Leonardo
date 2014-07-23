using NBehave.Spec.NUnit;
using NUnit.Framework;
using VierGewinnt.Spiellogik.Brett.Vierer;

namespace VierGewinnt.Spiellogik.Tests.Brett.Vierer
{
    [TestFixture]
    public class ViererKoordinatenTests
    {
        [Test]
        public void Horizontale_Koordinaten() {
            var vierer = new HorizontalerVierer(4, 3);

            vierer.Eins.ShouldEqual(new Koordinate(4, 3));
            vierer.Zwei.ShouldEqual(new Koordinate(5, 3));
            vierer.Drei.ShouldEqual(new Koordinate(6, 3));
            vierer.Vier.ShouldEqual(new Koordinate(7, 3));
        }

        [Test]
        public void Vertikale_Koordinaten() {
            var vierer = new VertikalerVierer(4, 3);

            vierer.Eins.ShouldEqual(new Koordinate(4, 3));
            vierer.Zwei.ShouldEqual(new Koordinate(4, 4));
            vierer.Drei.ShouldEqual(new Koordinate(4, 5));
            vierer.Vier.ShouldEqual(new Koordinate(4, 6));
        }

        [Test]
        public void Diagonal_nach_oben_Koordinaten() {
            var vierer = new DiagonalHochVierer(4, 3);

            vierer.Eins.ShouldEqual(new Koordinate(4, 3));
            vierer.Zwei.ShouldEqual(new Koordinate(5, 4));
            vierer.Drei.ShouldEqual(new Koordinate(6, 5));
            vierer.Vier.ShouldEqual(new Koordinate(7, 6));
        }

        [Test]
        public void Diagonal_nach_unten_Koordinaten() {
            var vierer = new DiagonalRunterVierer(4, 3);

            vierer.Eins.ShouldEqual(new Koordinate(4, 3));
            vierer.Zwei.ShouldEqual(new Koordinate(5, 2));
            vierer.Drei.ShouldEqual(new Koordinate(6, 1));
            vierer.Vier.ShouldEqual(new Koordinate(7, 0));
        }
    }
}