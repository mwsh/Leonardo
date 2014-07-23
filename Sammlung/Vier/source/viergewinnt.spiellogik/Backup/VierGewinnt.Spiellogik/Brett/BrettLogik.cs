using System;
using System.Linq;
using VierGewinnt.Spiellogik.Brett.Vierer;
using VierGewinnt.Spiellogik.Contracts;

namespace VierGewinnt.Spiellogik.Brett
{
    internal static class BrettLogik
    {
        internal static void SpieleStein(int[,] spielfeld, Spieler spieler, int spalte) {
            for (var j = 0; j < spielfeld.GetLength(1); j++) {
                if (spielfeld[spalte, j] == 0) {
                    spielfeld[spalte, j] = (int)spieler;
                    return;
                }
            }
            throw new InvalidOperationException();
        }

        internal static Spieler Gewinner(int[,] spielfeld) {
            var gewinnerVierer = spielfeld.AlleVierer().SelbeFarbe(spielfeld);
            if (gewinnerVierer.Count() == 0) {
                return Spieler.Keiner;
            }
            var ersteKoordinateDesVierers = gewinnerVierer.First().Eins;
            return (Spieler)spielfeld[ersteKoordinateDesVierers.X, ersteKoordinateDesVierers.Y];
        }

        internal static IVierer GewinnerKoordinaten(int[,] spielfeld) {
            var gewinnerVierer = spielfeld.AlleVierer().SelbeFarbe(spielfeld);
            return gewinnerVierer.FirstOrDefault();
        }
    }
}