namespace VierGewinnt.Spiellogik.Contracts
{
    public interface ISpielLogik
    {
        Zustaende Zustand { get; }

        void LegeSteinInSpalte(int spalte);

        int[,] Spielfeld { get; }

        int[,] GewinnerKoordinaten { get; }
    }
}