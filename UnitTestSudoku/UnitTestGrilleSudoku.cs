using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS_Sudoku.Model;

namespace UnitTestSudoku {
    [TestClass]
    public class UnitTestGrilleSudoku {
        private static readonly Random rnd = new Random();

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

        [TestMethod]
        public void ConstructeurParDéfaut_TestGroupesEtEtatDesCellules()
        {
            Sudoku sudoku = new Sudoku();

            // Test des groupes
            for (int i=0; i<9; ++i)
            {
                Assert.IsTrue(sudoku.Groupes.Contains(sudoku.Lignes[i]), $"La ligne {i} devrait appartenir à l'ensemble des groupes");
                Assert.IsTrue(sudoku.Groupes.Contains(sudoku.Colonnes[i]), $"La colonne {i} devrait appartenir à l'ensemble des groupes");
                Assert.IsTrue(sudoku.Groupes.Contains(sudoku.Blocs[i]), $"Le bloc {i} devrait appartenir à l'ensemble des groupes");
                // Test sur les lignes, colonnes, blocs
                Assert.AreEqual(9, sudoku.Lignes[i].Count, $"La ligne {i} devrait contenir 9 cellules.");
                Assert.AreEqual(9, sudoku.Colonnes[i].Count, $"La colonne {i} devrait contenir 9 cellules.");
                Assert.AreEqual(9, sudoku.Blocs[i].Count, $"Le bloc {i} devrait contenir 9 cellules.");
            }

            for (int li=0; li<9; ++li)
            {
                for (int co=0; co<9; ++co)
                {
                    Cellule c = sudoku[li, co];
                    // Test de l'appartenance aux groupes de la cellule
                    Assert.IsTrue(sudoku.Lignes[li].Contains(c), $"La cellule de la ligne {li}, colonne {co} devrait appartenir à la ligne {li}");
                    Assert.IsTrue(sudoku.Colonnes[co].Contains(c), $"La cellule de la ligne {li}, colonne {co} devrait appartenir à la colonne {co}");
                    Assert.IsTrue(sudoku.Blocs[li/3*3 + co/3].Contains(c), $"La cellule de la ligne {li}, colonne {co} devrait appartenir à la colonne {co}");
                    // Test des lignes, colonnes et blocs de la cellule
                    Assert.AreEqual(c.Ligne, sudoku.Lignes[li], $"La cellule de la ligne {li}, colonne {co} ne référencie pas la bonne ligne");
                    Assert.AreEqual(c.Colonne, sudoku.Colonnes[co], $"La cellule de la ligne {li}, colonne {co} ne référencie pas la bonne colonne");
                    Assert.AreEqual(c.Bloc, sudoku.Blocs[li/3*3 + co/3], $"La cellule de la ligne {li}, colonne {co} ne référencie pas le bon bloc");
                    // Test de l'état de la cellule
                    Assert.IsFalse(c.Fixé, $"A l'initialisation la cellule ({li}, {co}) ne devrait pas être fixée.");
                    Assert.IsFalse(c.Trouvé, $"A l'initialisation la cellule ({li}, {co}) ne devrait pas être fixée.");
                    Assert.AreEqual(0, c.Possibilités.Count, $"A l'initialisation, la cellule ({li}, {co}) ne devrait pas contenir de possibilités.");
                }
            }
        }

        [TestMethod]
        public void ContructeurParCopie()
        {
            int i, j;
            int n;
            // générés aléatoirement...
            int[,] __fixés = new int[9, 9];
            int[,] __trouvés = new int[9, 9];
            SortedSet<int>[,] __possibilités = new SortedSet<int>[9, 9];
            // Génération aléatoire de la configuration du Sudoku
            for (i = 0; i < 9; ++i)
            {
                for (j=0; j<9; ++j)
                {
                    switch (rnd.Next(3))
                    {
                        case 0: // Valeur fixée
                            __fixés[i, j] = rnd.Next(1, 10);
                            __trouvés[i, j] = 0;
                            __possibilités[i, j] = null;
                            break;
                        case 1: // Valeur trouvé
                            __fixés[i, j] = 0;
                            __trouvés[i, j] = rnd.Next(1, 10);
                            __possibilités[i, j] = null;
                            break;
                        case 2: // Ni trouvé, ni fixé
                            __fixés[i, j] = __trouvés[i, j] = 0;
                            __possibilités[i, j] = new SortedSet<int>();
                            n = rnd.Next(10);
                            while (n>0)
                            {
                                __possibilités[i, j].Add(rnd.Next(1, 10));
                                --n;
                            }
                            break;
                    }
                }
            }
            // Création du sudoku correspondant
            Sudoku sudoku = new Sudoku();
            // Génération du sudoku
            for (i=0; i<9; i++)
            {
                for (j=0; j<9; ++j)
                {
                    if (__fixés[i,j] > 0)
                    {
                        sudoku[i, j].FixerValeur(__fixés[i, j]);
                    }
                    else if (__trouvés[i,j] > 0)
                    {
                        sudoku[i, j].ModifierValeur(__trouvés[i, j]);
                    }
                    else
                    {
                        foreach (int v in __possibilités[i, j])
                            sudoku[i, j].Possibilités.Add(v);
                    }
                }
            }
            // Copie du sudoku
            Sudoku s = new Sudoku(sudoku);
            // Remise à 0 du sudoku initial
            for (i=0; i<9; ++i)
            {
                for (j=0; j<9; ++j)
                {
                    sudoku[i, j].Effacer();
                }
                sudoku.Lignes[i].Clear();
                sudoku.Colonnes[i].Clear();
                sudoku.Blocs[i].Clear();
                sudoku.Groupes.Clear();
            }

            // Tests sur la copie !
            for (i=0; i<9; ++i)
            {
                for (j=0; j<9; ++j)
                {
                    // Définition des lignes
                    Assert.IsTrue(s.Lignes[i].Contains(s[i, j]), $"La ligne {i} devrait contenir la cellule {s[i,j].Nom}");
                    Assert.IsTrue(s.Colonnes[j].Contains(s[i, j]), $"La colonne {j} devrait contenir la cellule {s[i,j].Nom}");
                    Assert.IsTrue(s.Blocs[i/3*3 + j/3].Contains(s[i, j]), $"Le bloc {i/3*3 + j/3} devrait contenir la cellule {s[i,j].Nom}");
                    // Définition des cellules
                    Assert.AreEqual(s.Lignes[i], s[i, j].Ligne, $"La cellule {s[i, j].Nom} devrait contenir la ligne {s.Lignes[i].Nom}");
                    Assert.AreEqual(s.Colonnes[j], s[i, j].Colonne, $"La cellule {s[i, j].Nom} devrait contenir la colonne {s.Colonnes[i].Nom}");
                    Assert.AreEqual(s.Blocs[i/3*3 + j/3], s[i, j].Bloc, $"La cellule {s[i, j].Nom} devrait contenir le bloc {s.Blocs[i/3*3 + j/3].Nom}");

                    // Etat des cellules
                    if (__fixés[i,j] > 0)
                    {
                        Assert.IsTrue(s[i, j].Fixé, $"La cellule {s[i, j].Nom} devrait être dans l'état fixé.");
                        Assert.AreEqual(__fixés[i, j], s[i, j].Valeur, $"La cellule {s[i, j].Nom} devrait valoir {__fixés[i, j]}");
                    }
                    else if (__trouvés[i,j] > 0)
                    {
                        Assert.IsTrue(s[i, j].Trouvé, $"La cellule {s[i, j].Nom} devrait être dans l'état trouvé.");
                        Assert.AreEqual(__trouvés[i, j], s[i, j].Valeur, $"La cellule {s[i, j].Nom} devrait valoir {__trouvés[i, j]}");
                    }
                    else
                    {
                        Assert.IsFalse(s[i, j].Fixé || s[i, j].Trouvé, $"La cellule {s[i, j].Nom} ne devrait ni être dans l'état fixé, ni dans l'état trouvé.");
                        Assert.IsTrue(__possibilités[i, j].SetEquals(s[i, j].Possibilités), $"La cellule {s[i, j].Nom} devrait contenir les possibilités {__possibilités[i, j]} au lieu de {s[i, j].Possibilités}.");
                    }
                }
                // Vérification du groupe
                Assert.IsTrue(s.Groupes.Contains(s.Lignes[i]), $"L'ensemble des groupes ne contient pas la ligne {i}");
                Assert.IsTrue(s.Groupes.Contains(s.Colonnes[i]), $"L'ensemble des groupes ne contient pas la colonne {i}");
                Assert.IsTrue(s.Groupes.Contains(s.Blocs[i]), $"L'ensemble des groupes ne contient pas le bloc {i}");
            }
        }
    }
}
