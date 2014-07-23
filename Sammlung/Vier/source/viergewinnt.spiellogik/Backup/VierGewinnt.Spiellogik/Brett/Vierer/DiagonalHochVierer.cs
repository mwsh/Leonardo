namespace VierGewinnt.Spiellogik.Brett.Vierer
{
    internal struct DiagonalHochVierer : IVierer
    {
        private readonly int x;
        private readonly int y;

        public DiagonalHochVierer(int x, int y)
            : this() {
            this.x = x;
            this.y = y;
        }

        public Koordinate Eins {
            get { return new Koordinate(x, y); }
        }

        public Koordinate Zwei {
            get { return new Koordinate(x + 1, y + 1); }
        }

        public Koordinate Drei {
            get { return new Koordinate(x + 2, y + 2); }
        }

        public Koordinate Vier {
            get { return new Koordinate(x + 3, y + 3); }
        }

        public override string ToString() {
            return string.Format("Diagonal hoch X: {0}, Y: {1}", x, y);
        }
    }
}