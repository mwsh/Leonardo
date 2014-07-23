using VierGewinnt.Spiellogik.Brett.Vierer;
using VierGewinnt.Spiellogik.Contracts;

namespace VierGewinnt.Spiellogik.Brett
{
    internal class Spielbrett
    {
        internal const int Spalten = 7;
        internal const int SteineUebereinander = 6;

        private readonly int[,] spielfeld;

        public Spielbrett()
            : this(new int[Spalten,SteineUebereinander]) {
        }

        internal Spielbrett(int[,] spielfeld) {
            this.spielfeld = spielfeld;
        }

        public void SpieleStein(Spieler spieler, int spalte) {
            BrettLogik.SpieleStein(spielfeld, spieler, spalte);
        }

        public Spieler Gewinner {
            get { return BrettLogik.Gewinner(spielfeld); }
        }

        public IVierer GewinnerKoordinaten {
            get { return BrettLogik.GewinnerKoordinaten(spielfeld); }
        }

        public int[,] Spielfeld {
            get { return spielfeld; }
        }
    }
}