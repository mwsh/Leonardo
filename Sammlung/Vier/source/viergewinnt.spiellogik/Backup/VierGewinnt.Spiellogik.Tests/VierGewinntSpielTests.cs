using System;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using VierGewinnt.Spiellogik.Contracts;

namespace VierGewinnt.Spiellogik.Tests
{
    [TestFixture]
    public class VierGewinntSpielTests
    {
        private VierGewinntSpiel vierGewinntSpiel;

        [SetUp]
        public void Setup() {
            vierGewinntSpiel = new VierGewinntSpiel();
        }

        [Test]
        public void Rot_beginnt_das_Spiel() {
            vierGewinntSpiel.Zustand.ShouldEqual(Zustaende.RotIstAmZug);
        }

        [Test]
        public void Nachdem_rot_einen_Stein_spielt_ist_gelb_am_Zug() {
            vierGewinntSpiel.LegeSteinInSpalte(1);

            vierGewinntSpiel.Zustand.ShouldEqual(Zustaende.GelbIstAmZug);
        }

        [Test]
        public void Nachdem_rot_und_gelb_jeweils_einen_Stein_gespielt_haben_ist_rot_wieder_am_Zug() {
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(1);

            vierGewinntSpiel.Zustand.ShouldEqual(Zustaende.RotIstAmZug);
        }

        [Test]
        public void Wenn_rot_vier_Steine_uebereinander_legen_konnte_hat_rot_gewonnen() {
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);

            vierGewinntSpiel.Zustand.ShouldEqual(Zustaende.RotHatGewonnen);
        }

        [Test]
        public void Die_Koordinaten_des_Gewinners_koennen_ermittelt_werden() {
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);

            vierGewinntSpiel.GewinnerKoordinaten.ShouldEqual(new[,] {{1, 0}, {1, 1}, {1, 2}, {1, 3}});
        }

        [Test]
        public void Wenn_gelb_vier_Steine_nebeneinander_legen_konnte_hat_gelb_gewonnen() {
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(3);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(4);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(5);

            vierGewinntSpiel.Zustand.ShouldEqual(Zustaende.GelbHatGewonnen);
        }

        [Test]
        public void Wenn_gelb_weiterspielt_obwohl_rot_bereits_gewonnen_hat_wird_eine_Ausnahme_ausgeloest() {
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);
            vierGewinntSpiel.LegeSteinInSpalte(2);
            vierGewinntSpiel.LegeSteinInSpalte(1);

            Assert.Throws<InvalidOperationException>(() => vierGewinntSpiel.LegeSteinInSpalte(2));
        }

        [Test]
        public void Der_Zustand_des_Spielbrettes_kann_ermittelt_werden() {
            vierGewinntSpiel.LegeSteinInSpalte(0);
            vierGewinntSpiel.Spielfeld[0, 0].ShouldEqual(-1);
            vierGewinntSpiel.LegeSteinInSpalte(0);
            vierGewinntSpiel.Spielfeld[0, 1].ShouldEqual(1);
            vierGewinntSpiel.LegeSteinInSpalte(0);
            vierGewinntSpiel.Spielfeld[0, 2].ShouldEqual(-1);
        }
    }
}