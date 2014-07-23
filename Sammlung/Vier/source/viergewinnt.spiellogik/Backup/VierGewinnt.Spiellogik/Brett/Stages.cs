using System.Collections.Generic;
using VierGewinnt.Spiellogik.Brett.Vierer;

namespace VierGewinnt.Spiellogik.Brett
{
    internal static class Stages
    {
        internal static IEnumerable<IVierer> AlleVierer(this int[,] feld) {
            for (var x = 0; x <= feld.GetLength(0) - 4; x++) {
                for (var y = 0; y < feld.GetLength(1); y++) {
                    yield return new HorizontalerVierer(x, y);
                }
            }
            for (var x = 0; x < feld.GetLength(0); x++) {
                for (var y = 0; y <= feld.GetLength(1) - 4; y++) {
                    yield return new VertikalerVierer(x, y);
                }
            }
            for (var x = 0; x <= feld.GetLength(0) - 4; x++) {
                for (var y = 0; y <= feld.GetLength(1) - 4; y++) {
                    yield return new DiagonalHochVierer(x, y);
                }
            }
            for (var x = 0; x <= feld.GetLength(0) - 4; x++) {
                for (var y = 3; y < feld.GetLength(1); y++) {
                    yield return new DiagonalRunterVierer(x, y);
                }
            }
        }

        internal static IEnumerable<IVierer> SelbeFarbe(this IEnumerable<IVierer> vierer, int[,] feld) {
            foreach (var vier in vierer) {
                if ((feld[vier.Eins.X, vier.Eins.Y] != 0) &&
                    (feld[vier.Eins.X, vier.Eins.Y] == feld[vier.Zwei.X, vier.Zwei.Y]) &&
                    (feld[vier.Eins.X, vier.Eins.Y] == feld[vier.Drei.X, vier.Drei.Y]) &&
                    (feld[vier.Eins.X, vier.Eins.Y] == feld[vier.Vier.X, vier.Vier.Y])) {
                    yield return vier;
                }
            }
        }
    }
}