namespace VierGewinnt.Spiellogik.Brett.Vierer
{
    internal interface IVierer
    {
        Koordinate Eins { get; }

        Koordinate Zwei { get; }

        Koordinate Drei { get; }

        Koordinate Vier { get; }
    }
}