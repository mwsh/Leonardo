namespace VierGewinnt.Spiellogik.Brett.Vierer
{
    internal struct Koordinate
    {
        public Koordinate(int x, int y)
            : this() {
            X = x;
            Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public override string ToString() {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }
    }
}