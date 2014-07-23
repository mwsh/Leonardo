namespace VierGewinnt.Spiellogik.Brett.Vierer
{
    internal struct VertikalerVierer : IVierer
    {
        private readonly int x;
        private readonly int y;

        public VertikalerVierer(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Koordinate Eins {
            get { return new Koordinate(x, y); }
        }

        public Koordinate Zwei {
            get { return new Koordinate(x, y + 1); }
        }

        public Koordinate Drei {
            get { return new Koordinate(x, y + 2); }
        }

        public Koordinate Vier {
            get { return new Koordinate(x, y + 3); }
        }

        public override string ToString() {
            return string.Format("Vertikal X: {0}, Y: {1}", x, y);
        }
    }
}