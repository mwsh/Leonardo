using System.Linq;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using VierGewinnt.Spiellogik.Brett;
using VierGewinnt.Spiellogik.Brett.Vierer;

namespace VierGewinnt.Spiellogik.Tests.Brett
{
    [TestFixture]
    public class StagesTests
    {
        [Test]
        public void Alle_Vierer_Kombinationen_werden_gefunden() {
            var feld = new[,] {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0}
            };

            var viererIndizes = Stages.AlleVierer(feld);
            viererIndizes.ToList().ShouldEqual(new IVierer[] {
                new HorizontalerVierer(0, 0),
                new HorizontalerVierer(0, 1),
                new HorizontalerVierer(0, 2),
                new HorizontalerVierer(0, 3),
                new HorizontalerVierer(0, 4),
                new HorizontalerVierer(0, 5),
                new HorizontalerVierer(1, 0),
                new HorizontalerVierer(1, 1),
                new HorizontalerVierer(1, 2),
                new HorizontalerVierer(1, 3),
                new HorizontalerVierer(1, 4),
                new HorizontalerVierer(1, 5),
                new HorizontalerVierer(2, 0),
                new HorizontalerVierer(2, 1),
                new HorizontalerVierer(2, 2),
                new HorizontalerVierer(2, 3),
                new HorizontalerVierer(2, 4),
                new HorizontalerVierer(2, 5),
                new HorizontalerVierer(3, 0),
                new HorizontalerVierer(3, 1),
                new HorizontalerVierer(3, 2),
                new HorizontalerVierer(3, 3),
                new HorizontalerVierer(3, 4),
                new HorizontalerVierer(3, 5),
                new VertikalerVierer(0, 0),
                new VertikalerVierer(0, 1),
                new VertikalerVierer(0, 2),
                new VertikalerVierer(1, 0),
                new VertikalerVierer(1, 1),
                new VertikalerVierer(1, 2),
                new VertikalerVierer(2, 0),
                new VertikalerVierer(2, 1),
                new VertikalerVierer(2, 2),
                new VertikalerVierer(3, 0),
                new VertikalerVierer(3, 1),
                new VertikalerVierer(3, 2),
                new VertikalerVierer(4, 0),
                new VertikalerVierer(4, 1),
                new VertikalerVierer(4, 2),
                new VertikalerVierer(5, 0),
                new VertikalerVierer(5, 1),
                new VertikalerVierer(5, 2),
                new VertikalerVierer(6, 0),
                new VertikalerVierer(6, 1),
                new VertikalerVierer(6, 2),
                new DiagonalHochVierer(0, 0),
                new DiagonalHochVierer(0, 1),
                new DiagonalHochVierer(0, 2),
                new DiagonalHochVierer(1, 0),
                new DiagonalHochVierer(1, 1),
                new DiagonalHochVierer(1, 2),
                new DiagonalHochVierer(2, 0),
                new DiagonalHochVierer(2, 1),
                new DiagonalHochVierer(2, 2),
                new DiagonalHochVierer(3, 0),
                new DiagonalHochVierer(3, 1),
                new DiagonalHochVierer(3, 2),
                new DiagonalRunterVierer(0, 3),
                new DiagonalRunterVierer(0, 4),
                new DiagonalRunterVierer(0, 5),
                new DiagonalRunterVierer(1, 3),
                new DiagonalRunterVierer(1, 4),
                new DiagonalRunterVierer(1, 5),
                new DiagonalRunterVierer(2, 3),
                new DiagonalRunterVierer(2, 4),
                new DiagonalRunterVierer(2, 5),
                new DiagonalRunterVierer(3, 3),
                new DiagonalRunterVierer(3, 4),
                new DiagonalRunterVierer(3, 5)
            });
        }

        [Test]
        public void Wenn_das_Feld_leer_ist_werden_keine_Vierer_mit_selber_Farbe_gefunden() {
            var feld = new[,] {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0}
            };
            var viererIndizes = Stages.AlleVierer(feld);
            var vierer = Stages.SelbeFarbe(viererIndizes, feld);
            vierer.ToList().ShouldBeEmpty();
        }

        [Test]
        public void Wenn_das_Feld_einen_Vierer_mit_selber_Farbe_enthaelt_wird_dieser_gefunden() {
            var feld = new[,] {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {1, 0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0},
                {0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0, 0}
            };
            var viererIndizes = Stages.AlleVierer(feld);

            var vierer = Stages.SelbeFarbe(viererIndizes, feld);
            vierer.ToArray().ShouldEqual(new[] {
                new DiagonalHochVierer(2, 0)
            });
        }
    }
}