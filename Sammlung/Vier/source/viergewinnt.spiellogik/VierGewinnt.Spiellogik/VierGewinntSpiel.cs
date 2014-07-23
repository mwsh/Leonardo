using System;
using VierGewinnt.Spiellogik.Brett;
using VierGewinnt.Spiellogik.Contracts;

namespace VierGewinnt.Spiellogik
{
    public class VierGewinntSpiel : ISpielLogik
    {
        private readonly Spielbrett spielbrett = new Spielbrett();

        public VierGewinntSpiel() {
            Zustand = Zustaende.RotIstAmZug;
        }

        public Zustaende Zustand { get; private set; }

        public void LegeSteinInSpalte(int spalte) {
            if (spielbrett.Gewinner != Spieler.Keiner) {
                throw new InvalidOperationException();
            }

            if (Zustand == Zustaende.RotIstAmZug) {
                spielbrett.SpieleStein(Spieler.Rot, spalte);
                Zustand = Zustaende.GelbIstAmZug;
            }
            else {
                spielbrett.SpieleStein(Spieler.Gelb, spalte);
                Zustand = Zustaende.RotIstAmZug;
            }

            if (spielbrett.Gewinner == Spieler.Rot) {
                Zustand = Zustaende.RotHatGewonnen;
            }
            if (spielbrett.Gewinner == Spieler.Gelb) {
                Zustand = Zustaende.GelbHatGewonnen;
            }
        }

        public int[,] Spielfeld {
            get { return spielbrett.Spielfeld; }
        }

        public int[,] GewinnerKoordinaten {
            get {
                var result = new int[4,2];
                result[0, 0] = spielbrett.GewinnerKoordinaten.Eins.X;
                result[0, 1] = spielbrett.GewinnerKoordinaten.Eins.Y;
                result[1, 0] = spielbrett.GewinnerKoordinaten.Zwei.X;
                result[1, 1] = spielbrett.GewinnerKoordinaten.Zwei.Y;
                result[2, 0] = spielbrett.GewinnerKoordinaten.Drei.X;
                result[2, 1] = spielbrett.GewinnerKoordinaten.Drei.Y;
                result[3, 0] = spielbrett.GewinnerKoordinaten.Vier.X;
                result[3, 1] = spielbrett.GewinnerKoordinaten.Vier.Y;
                return result;
            }
        }
    }
}