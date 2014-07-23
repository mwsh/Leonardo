using NBehave.Spec.NUnit;
using NUnit.Framework;
using VierGewinnt.Spiellogik.Brett.Vierer;

namespace VierGewinnt.Spiellogik.Tests.Brett.Vierer
{
    [TestFixture]
    public class Vierer_Equals_Tests
    {
        [Test]
        public void Zwei_DiagonalHochVierer_mit_gleicher_Koordinate_sind_gleich() {
            var vierer1 = new DiagonalHochVierer(2, 4);
            var vierer2 = new DiagonalHochVierer(2, 4);
            vierer1.ShouldEqual(vierer2);
        }

        [Test]
        public void Zwei_DiagonalHochVierer_mit_unterschiedlichen_Koordinate_sind_ungleich() {
            var vierer1 = new DiagonalHochVierer(4, 2);
            var vierer2 = new DiagonalHochVierer(2, 4);
            vierer1.ShouldNotEqual(vierer2);
        }

        [Test]
        public void Zwei_DiagonalRunterVierer_mit_gleicher_Koordinate_sind_gleich() {
            var vierer1 = new DiagonalRunterVierer(2, 4);
            var vierer2 = new DiagonalRunterVierer(2, 4);
            vierer1.ShouldEqual(vierer2);
        }

        [Test]
        public void Zwei_DiagonalRunterVierer_mit_unterschiedlichen_Koordinate_sind_ungleich() {
            var vierer1 = new DiagonalRunterVierer(4, 2);
            var vierer2 = new DiagonalRunterVierer(2, 4);
            vierer1.ShouldNotEqual(vierer2);
        }

        [Test]
        public void Zwei_VertikalerVierer_mit_gleicher_Koordinate_sind_gleich() {
            var vierer1 = new VertikalerVierer(2, 4);
            var vierer2 = new VertikalerVierer(2, 4);
            vierer1.ShouldEqual(vierer2);
        }

        [Test]
        public void Zwei_VertikalerVierer_mit_unterschiedlichen_Koordinate_sind_ungleich() {
            var vierer1 = new VertikalerVierer(4, 2);
            var vierer2 = new VertikalerVierer(2, 4);
            vierer1.ShouldNotEqual(vierer2);
        }

        [Test]
        public void Zwei_HorizontalerVierer_mit_gleicher_Koordinate_sind_gleich() {
            var vierer1 = new HorizontalerVierer(2, 4);
            var vierer2 = new HorizontalerVierer(2, 4);
            vierer1.ShouldEqual(vierer2);
        }

        [Test]
        public void Zwei_HorizontalerVierer_mit_unterschiedlichen_Koordinate_sind_ungleich() {
            var vierer1 = new HorizontalerVierer(4, 2);
            var vierer2 = new HorizontalerVierer(2, 4);
            vierer1.ShouldNotEqual(vierer2);
        }
    }
}