using System;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using VierGewinnt.Spiellogik.Brett;
using VierGewinnt.Spiellogik.Brett.Vierer;
using VierGewinnt.Spiellogik.Contracts;

namespace VierGewinnt.Spiellogik.Tests.Brett
{
    [TestFixture]
    public class SpielbrettTests
    {
        private Spielbrett spielbrett;
        private int[,] spielfeld;

        [SetUp]
        public void Setup() {
            spielfeld = new int[Spielbrett.Spalten,Spielbrett.SteineUebereinander];
            spielbrett = new Spielbrett(spielfeld);
        }

        [Test]
        public void Ein_leeres_Spielbrett_hat_keinen_Gewinner() {
            spielbrett.Gewinner.ShouldEqual(Spieler.Keiner);
        }

        [Test]
        public void Ein_leeres_Spielbrett_hat_keine_Gewinner_Koordinaten() {
            spielbrett.GewinnerKoordinaten.ShouldBeNull();
        }

        [Test]
        public void SpieleStein_legt_einen_roten_Stein_als_untersten_ins_Spielfeld() {
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielfeld[1, 0].ShouldEqual(-1);
        }

        [Test]
        public void SpieleStein_legt_einen_gelben_Stein_als_untersten_ins_Spielfeld() {
            spielbrett.SpieleStein(Spieler.Gelb, 3);
            spielfeld[3, 0].ShouldEqual(1);
        }

        [Test]
        public void SpieleStein_legt_vier_gelbe_Steine_uebereinander_ins_Spielfeld() {
            spielbrett.SpieleStein(Spieler.Gelb, 6);
            spielbrett.SpieleStein(Spieler.Gelb, 6);
            spielbrett.SpieleStein(Spieler.Gelb, 6);
            spielbrett.SpieleStein(Spieler.Gelb, 6);
            spielfeld[6, 0].ShouldEqual(1);
            spielfeld[6, 1].ShouldEqual(1);
            spielfeld[6, 2].ShouldEqual(1);
            spielfeld[6, 3].ShouldEqual(1);
        }

        [Test]
        public void Es_koennen_6_Steine_in_eine_Spalte_gelegt_werden() {
            spielbrett.SpieleStein(Spieler.Gelb, 6);
            spielbrett.SpieleStein(Spieler.Rot, 6);
            spielbrett.SpieleStein(Spieler.Gelb, 6);
            spielbrett.SpieleStein(Spieler.Rot, 6);
            spielbrett.SpieleStein(Spieler.Gelb, 6);
            spielbrett.SpieleStein(Spieler.Rot, 6);
            spielfeld[6, 0].ShouldEqual(1);
            spielfeld[6, 1].ShouldEqual(-1);
            spielfeld[6, 2].ShouldEqual(1);
            spielfeld[6, 3].ShouldEqual(-1);
            spielfeld[6, 4].ShouldEqual(1);
            spielfeld[6, 5].ShouldEqual(-1);
        }

        [Test]
        public void Beim_versucht_den_siebten_Stein_abzulegen_wird_eine_Ausnahme_ausgeloest() {
            spielbrett.SpieleStein(Spieler.Gelb, 3);
            spielbrett.SpieleStein(Spieler.Gelb, 3);
            spielbrett.SpieleStein(Spieler.Gelb, 3);
            spielbrett.SpieleStein(Spieler.Gelb, 3);
            spielbrett.SpieleStein(Spieler.Gelb, 3);
            spielbrett.SpieleStein(Spieler.Gelb, 3);

            Assert.Throws<InvalidOperationException>(() => spielbrett.SpieleStein(Spieler.Gelb, 3));
        }

        [Test]
        public void Die_Position_der_Steine_kann_ermittelt_werden() {
            spielbrett.SpieleStein(Spieler.Gelb, 0);
            spielbrett.Spielfeld[0, 0].ShouldEqual(1);
            spielbrett.Spielfeld[0, 1].ShouldEqual(0);
        }

        [Test]
        public void Bei_4_roten_Steinen_uebereinander_hat_rot_gewonnen() {
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Rot, 1);

            spielbrett.Gewinner.ShouldEqual(Spieler.Rot);
            spielbrett.GewinnerKoordinaten.ShouldEqual(new VertikalerVierer(1, 0));
        }

        [Test]
        public void Die_4_Steine_muessen_direkt_uebereinander_liegen_um_zu_gewinnen() {
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Gelb, 1);
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Rot, 1);

            spielbrett.Gewinner.ShouldEqual(Spieler.Keiner);
        }

        [Test]
        public void Bei_4_gelben_Steinen_nebeneinander_hat_gelb_gewonnen() {
            spielbrett.SpieleStein(Spieler.Gelb, 0);
            spielbrett.SpieleStein(Spieler.Gelb, 1);
            spielbrett.SpieleStein(Spieler.Gelb, 2);
            spielbrett.SpieleStein(Spieler.Gelb, 3);

            spielbrett.Gewinner.ShouldEqual(Spieler.Gelb);
            spielbrett.GewinnerKoordinaten.ShouldEqual(new HorizontalerVierer(0, 0));
        }

        [Test]
        public void Die_4_Steine_muessen_unmittelbar_nebeneinander_liegen_um_zu_gewinnen() {
            spielbrett.SpieleStein(Spieler.Gelb, 0);
            spielbrett.SpieleStein(Spieler.Gelb, 1);
            spielbrett.SpieleStein(Spieler.Gelb, 2);
            spielbrett.SpieleStein(Spieler.Rot, 3);
            spielbrett.SpieleStein(Spieler.Gelb, 4);
            spielbrett.SpieleStein(Spieler.Gelb, 5);

            spielbrett.Gewinner.ShouldEqual(Spieler.Keiner);
        }

        [Test]
        public void Bei_4_gelben_Steinen_diagonal_nach_oben_hat_gelb_gewonnen() {
            spielbrett.SpieleStein(Spieler.Gelb, 0);

            spielbrett.SpieleStein(Spieler.Rot, 1);
            spielbrett.SpieleStein(Spieler.Gelb, 1);

            spielbrett.SpieleStein(Spieler.Rot, 2);
            spielbrett.SpieleStein(Spieler.Rot, 2);
            spielbrett.SpieleStein(Spieler.Gelb, 2);

            spielbrett.SpieleStein(Spieler.Rot, 3);
            spielbrett.SpieleStein(Spieler.Rot, 3);
            spielbrett.SpieleStein(Spieler.Rot, 3);
            spielbrett.SpieleStein(Spieler.Gelb, 3);

            spielbrett.Gewinner.ShouldEqual(Spieler.Gelb);
            spielbrett.GewinnerKoordinaten.ShouldEqual(new DiagonalHochVierer(0, 0));
        }

        [Test]
        public void Bei_4_gelb_Steinen_am_Rand_diagonal_nach_oben_hat_gelb_gewonnen() {
            spielfeld = new[,] {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {-1, -1, 1, 0, 0, 0},
                {-1, 1, -1, 1, 0, 0},
                {1, -1, 1, 1, 1, 0},
                {-1, 1, -1, 1, -1, 1},
            };
            spielbrett = new Spielbrett(spielfeld);

            spielbrett.Gewinner.ShouldEqual(Spieler.Gelb);
            spielbrett.GewinnerKoordinaten.ShouldEqual(new DiagonalHochVierer(3, 2));
        }

        [Test]
        public void Bei_4_gelben_Steinen_am_Rand_diagonal_nach_unten_hat_gelb_gewonnen() {
            spielfeld = new[,] {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {-1, -1, 1, -1, 1, 1},
                {-1, 1, -1, 1, 1, 0},
                {1, -1, 1, 1, 0, 0},
                {1, -1, 1, 0, 0, 0},
            };
            spielbrett = new Spielbrett(spielfeld);

            spielbrett.Gewinner.ShouldEqual(Spieler.Gelb);
            spielbrett.GewinnerKoordinaten.ShouldEqual(new DiagonalRunterVierer(3, 5));
        }
    }
}