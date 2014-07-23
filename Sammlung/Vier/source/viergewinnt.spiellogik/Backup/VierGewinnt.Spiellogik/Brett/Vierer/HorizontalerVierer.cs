namespace VierGewinnt.Spiellogik.Brett.Vierer
{
    internal struct HorizontalerVierer : IVierer
    {
        private readonly int x;
        private readonly int y;

        public HorizontalerVierer(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Koordinate Eins {
            get { return new Koordinate(x, y); }
        }

        public Koordinate Zwei {
            get { return new Koordinate(x + 1, y); }
        }

        public Koordinate Drei {
            get { return new Koordinate(x + 2, y); }
        }

        public Koordinate Vier {
            get { return new Koordinate(x + 3, y); }
        }

        public override string ToString() {
            return string.Format("Horizontal X: {0}, Y: {1}", x, y);
        }
    }
}