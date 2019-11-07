using CS_Sudoku.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestSudoku {
    [TestClass]
    public class UnitTestGrilleSudoku {
        [TestMethod]
        public void ConstructeurParDéfaut_VérificationDesCellules() {
            Sudoku sudo = new Sudoku();
            // Le sudoku est créé ligne par ligne
            // Liste des noms courts des cellules
            string[] noms = {
                "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9",
                "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9",
                "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9",
                "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9",
                "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9",
                "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9",
                "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8", "G9",
                "H1", "H2", "H3", "H4", "H5", "H6", "H7", "H8", "H9",
                "I1", "I2", "I3", "I4", "I5", "I6", "I7", "I8", "I9"
            };
            string[] blocs = {
                "b1", "b1", "b1", "b2", "b2", "b2", "b3", "b3", "b3",
                "b1", "b1", "b1", "b2", "b2", "b2", "b3", "b3", "b3",
                "b1", "b1", "b1", "b2", "b2", "b2", "b3", "b3", "b3",
                "b4", "b4", "b4", "b5", "b5", "b5", "b6", "b6", "b6",
                "b4", "b4", "b4", "b5", "b5", "b5", "b6", "b6", "b6",
                "b4", "b4", "b4", "b5", "b5", "b5", "b6", "b6", "b6",
                "b7", "b7", "b7", "b8", "b8", "b8", "b9", "b9", "b9",
                "b7", "b7", "b7", "b8", "b8", "b8", "b9", "b9", "b9",
                "b7", "b7", "b7", "b8", "b8", "b8", "b9", "b9", "b9"
            };

            int n = 0;
            foreach (Cellule c in sudo.Grille) {
                // Vérification du nom de la cellule
                Assert.AreEqual(noms[n], c.NomCourt);
                // Vérification de la bonne affectation de la ligne
                Assert.AreEqual(noms[n][0].ToString(), c.Ligne.NomCourt);
                // Vérification de la bonne affectation de la colonne
                Assert.AreEqual(noms[n][1].ToString(), c.Colonne.NomCourt);
                // Vérification de la bonne affectation du bloc
                Assert.AreEqual(blocs[n], c.Bloc.NomCourt);
                ++n;
            }
        }

    }
}
