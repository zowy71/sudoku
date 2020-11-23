using System;
using System.Collections.Generic;
using CS_Sudoku.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestSudoku {
    [TestClass]
    public class UnitTestCellule {
        private Cellule cell;
        private static Random rnd = new Random();
        [TestInitialize]
        public void Setup() {
            cell = new Cellule(new Ligne(0), new Colonne(0), new Bloc(0));
        }

        [TestMethod]
        public void Essai() { }

        [TestMethod]
        public void FixerValeur_DonnéesCorrectes_CelluleModifiée() {
            for (int i=1; i<=9; i++) {
                Assert.IsTrue(cell.FixerValeur(i));
                Assert.AreEqual(i, cell.Valeur);
                Assert.IsTrue(cell.Fixé);
            }
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(10)]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes =false)]
        public void FixerValeur_DonnéesIncorrectes_Exception(int v) {
            cell.FixerValeur(v);
        }

        [TestMethod]
        public void FixerValeur_CelluleNonModifiée() {
            for (int i=1; i<=9; i++) {
                Assert.IsTrue(cell.FixerValeur(i));
                Assert.IsFalse(cell.FixerValeur(i));
            }
        }

        [TestMethod]
        public void Effacer() {
            cell.FixerValeur(6);
            cell.Possibilités.Add(1);
            cell.Possibilités.Add(3);
            cell.Effacer();
            Assert.IsFalse(cell.Fixé);
            Assert.IsFalse(cell.Trouvé);
            Assert.AreEqual(Cellule.NONE, cell.Valeur);
            Assert.AreEqual(0, cell.Possibilités.Count);
        }

        [TestMethod]
        public void ModifierValeur_ValeursCorrectes() {
            for (int i=1; i<=9; i++) {
                Assert.IsTrue(cell.ModifierValeur(i));
                Assert.IsTrue(cell.Trouvé);
                Assert.AreEqual(i, cell.Valeur);
                Assert.IsFalse(cell.Fixé);
                // On affecte la même valeur à la cellule --> Aucune modification
                Assert.IsFalse(cell.ModifierValeur(i));
            }
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(10)]
        [DataRow(20)]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes =false)]
        public void ModifierValeur_DonnéeIncorrecte_Exception(int v) {
            cell.ModifierValeur(v);
        }

        [TestMethod]
        public void ModifierValeur_CelluleFixée_AucuneModification() {
            cell.FixerValeur(3);
            for (int i=1; i<=9; i++) {
                Assert.IsFalse(cell.ModifierValeur(i));
                Assert.IsFalse(cell.Trouvé);
                Assert.AreEqual(3, cell.Valeur);
            }
        }

        [TestMethod]
        public void NomCourt_Propriété() {
            string[,] noms =  {
                { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9" },
                { "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9" },
                { "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9" },
                { "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" },
                { "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9" },
                { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9" },
                { "G1", "G2", "G3", "G4", "G5", "G6", "G7", "G8", "G9" },
                { "H1", "H2", "H3", "H4", "H5", "H6", "H7", "H8", "H9" },
                { "I1", "I2", "I3", "I4", "I5", "I6", "I7", "I8", "I9" }
            };
            for (int i=0; i<9; i++) {
                for (int j=0; j<9; j++) {
                    Cellule _c = new Cellule(new Ligne(i), new Colonne(j), new Bloc(0));
                    Assert.AreEqual(noms[i, j], _c.NomCourt);
                    Assert.AreEqual("Cellule " + noms[i, j], _c.Nom);
                }
            }
        }

        [TestMethod]
        public void SupprimerValeur_CelluleAvecValeur() {
            cell.ModifierValeur(4);
            Assert.IsTrue(cell.SupprimerValeur());
            Assert.IsFalse(cell.Trouvé);
            Assert.IsFalse(cell.Fixé);
            Assert.AreEqual(Cellule.NONE, cell.Valeur);
        }

        [TestMethod]
        public void SupprimerValeur_CelluleFixée() {
            cell.FixerValeur(3);
            Assert.IsFalse(cell.SupprimerValeur());
            Assert.IsFalse(cell.Trouvé);
            Assert.IsTrue(cell.Fixé);
            Assert.AreEqual(cell.Valeur, 3);
        }

        [TestMethod]
        public void SupprimerValeur_CelluleSansValeur() {
            cell.Effacer();
            Assert.IsFalse(cell.SupprimerValeur());
            Assert.IsFalse(cell.Trouvé);
            Assert.IsFalse(cell.Fixé);
            Assert.AreEqual(Cellule.NONE, cell.Valeur);
        }

        [TestMethod]
        public void AjouterPossibilité_ValeurCorrecte() {
            for (int i=1; i<=9; i++) {
                Assert.IsTrue(cell.AjouterPossibilité(i));
                Assert.IsFalse(cell.Fixé);
                Assert.IsFalse(cell.Trouvé);
                Assert.AreEqual(Cellule.NONE, cell.Valeur);
                Assert.IsTrue(cell.Possibilités.Contains(i));
            }
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-10)]
        [DataRow(10)]
        [DataRow(20)]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes =false)]
        public void AjouterPossibilité_ValeurIncorrecte_Exception(int v) {
            cell.AjouterPossibilité(v);
        }

        [TestMethod]
        public void AjouterPossibilité_ValeurDéjàPrésente_AucuneAction() {
            cell.AjouterPossibilité(2);
            cell.AjouterPossibilité(4);
            cell.AjouterPossibilité(6);
            Assert.IsFalse(cell.AjouterPossibilité(2));
            Assert.IsFalse(cell.AjouterPossibilité(4));
            Assert.IsFalse(cell.AjouterPossibilité(6));
        }

        [TestMethod]
        public void Contient_CelluleFixéOuTrouvé() {
            cell.AjouterPossibilité(3);
            cell.AjouterPossibilité(4);
            Assert.IsTrue(cell.Contient(3));

            cell.ModifierValeur(5);
            Assert.IsFalse(cell.Contient(3));
            Assert.IsTrue(cell.Possibilités.Contains(3));

            cell.FixerValeur(2);
            Assert.IsFalse(cell.Contient(3));
            Assert.IsTrue(cell.Possibilités.Contains(3));
        }

        [TestMethod]
        public void AjouterPossibilité_CelluleFixéOuTrouvé() {
            cell.Effacer();
            cell.ModifierValeur(7);
            Assert.IsFalse(cell.AjouterPossibilité(1));
            Assert.IsFalse(cell.Possibilités.Contains(1));

            cell.Effacer();
            cell.FixerValeur(4);
            Assert.IsFalse(cell.AjouterPossibilité(2));
            Assert.IsFalse(cell.Possibilités.Contains(2));
        }

        [TestMethod]
        public void SupprimerPossibilité_ValeurPrésenteOuNon() {
            cell.AjouterPossibilité(1);
            cell.AjouterPossibilité(2);
            cell.AjouterPossibilité(4);
            Assert.IsTrue(cell.SupprimerPossibilité(2));
            Assert.IsFalse(cell.Possibilités.Contains(2));
            Assert.IsFalse(cell.SupprimerPossibilité(2));
            Assert.IsTrue(cell.SupprimerPossibilité(4));
            Assert.IsFalse(cell.Possibilités.Contains(4));
            Assert.IsFalse(cell.SupprimerPossibilité(4));
        }

        [TestMethod]
        public void SupprimerPossibilité_CelluleTrouvéOuFixé() {
            cell.AjouterPossibilité(3);
            cell.ModifierValeur(6);
            Assert.IsFalse(cell.SupprimerPossibilité(3));
            Assert.IsTrue(cell.Possibilités.Contains(3));
            cell.SupprimerValeur();
            cell.FixerValeur(8);
            Assert.IsFalse(cell.SupprimerPossibilité(3));
            Assert.IsTrue(cell.Possibilités.Contains(3));
        }

        [TestMethod]
        public void RemplirPossibilités_DifférentsCas() {
            // Cas où la cellule contient déjà des possibilités
            cell.AjouterPossibilité(4);
            cell.RemplirPossibilités();
            Assert.IsFalse(cell.Trouvé);
            Assert.IsFalse(cell.Fixé);
            for (int i=1; i<=9; i++) {
                Assert.IsTrue(cell.Possibilités.Contains(i));
            }
            // Cas où la cellule est fixée : 
            cell.Effacer();
            cell.FixerValeur(3);
            cell.RemplirPossibilités();
            Assert.IsFalse(cell.Trouvé);
            Assert.IsTrue(cell.Fixé);
            Assert.AreEqual(3, cell.Valeur);
            for (int i=1; i<=9; i++) {
                Assert.IsFalse(cell.Possibilités.Contains(i));
            }
            // Cas où la cellule est trouvée : 
            cell.Effacer();
            cell.ModifierValeur(3);
            cell.RemplirPossibilités();
            Assert.IsTrue(cell.Trouvé);
            Assert.IsFalse(cell.Fixé);
            Assert.AreEqual(3, cell.Valeur);
            for (int i = 1; i <= 9; i++) {
                Assert.IsFalse(cell.Possibilités.Contains(i));
            }
        }

        [TestMethod]
        public void Contient_CasPositifs_CasNegatifs() {
            cell.AjouterPossibilité(3);
            cell.AjouterPossibilité(6);
            cell.AjouterPossibilité(8);
            cell.AjouterPossibilité(9);
            Assert.IsTrue(cell.Contient(new int[] { 3 }));
            Assert.IsTrue(cell.Contient(new int[] { 6, 3 }));
            Assert.IsTrue(cell.Contient(new int[] { 6, 8, 3 }));
            Assert.IsTrue(cell.Contient(new int[] { 3, 9, 6, 8 }));

            Assert.IsFalse(cell.Contient(new int[] { 3, 5 }));
            Assert.IsFalse(cell.Contient(new int[] { 5, 7 }));
        }

        [TestMethod]
        public void NeContientPas_DifférentsCas() {
            cell.AjouterPossibilité(3);
            cell.AjouterPossibilité(6);
            cell.AjouterPossibilité(8);
            cell.AjouterPossibilité(9);
            Assert.IsFalse(cell.NeContientPas(new int[] { 3 }));
            Assert.IsFalse(cell.NeContientPas(new int[] { 1, 2, 8 }));
            Assert.IsFalse(cell.NeContientPas(new int[] { 6, 1, 2, 4 }));

            Assert.IsTrue(cell.NeContientPas(new int[] { 1, 2, 4 }));
            Assert.IsTrue(cell.NeContientPas(new int[] { 5 }));
            Assert.IsTrue(cell.NeContientPas(new int[] { 1, 2, 4, 5, 7 }));

            cell.ModifierValeur(5);
            Assert.IsFalse(cell.NeContientPas(new int[] { 5 }));

            cell.FixerValeur(3);
            Assert.IsFalse(cell.NeContientPas(new int[] { 5 }));

        }

        [TestMethod]
        public void SupprimerPossibilité_DifferentsCas() {
            cell.AjouterPossibilité(3);
            cell.AjouterPossibilité(6);
            cell.AjouterPossibilité(8);
            cell.AjouterPossibilité(9);
            Assert.IsTrue(cell.SupprimerPossibilité(new int[] { 3, 5 }));
            foreach (int v in new int [] {  6, 8, 9}) {
                Assert.IsTrue(cell.Contient(v));
            }
            foreach (int v in new int [] { 1, 2, 3, 4, 5, 7}) {
                Assert.IsFalse(cell.Contient(v));
            }
            Assert.IsFalse(cell.SupprimerPossibilité(new int[] { 2, 1, 7 }));
        }

        private SortedSet<int> GénérerListe() {
            SortedSet<int> res = new SortedSet<int>();
            // Choix du nomre d'éléments
            int nb = rnd.Next(1, 10);
            for (int i=0; i<nb; ++i) {
                res.Add(rnd.Next(1, 10));
            }
            return res;
        }

        [TestMethod]
        public void SupprimerExcepté_DifférentsCas() {
            SortedSet<int> depart;
            SortedSet<int> conserve;
            for (int i = 0; i < 1000; i++) {
                cell.Effacer();
                // On ajoute des possibilités
                depart = GénérerListe();
                foreach (int v in depart) {
                    cell.AjouterPossibilité(v);
                }
                conserve = GénérerListe();
                bool res = cell.SupprimerExcepté(conserve);
                // Vérification à partir de l'ensemble de départ !
                bool modif = false;
                foreach (int v in depart) {
                    Assert.AreEqual(conserve.Contains(v), cell.Contient(v));
                    if (!conserve.Contains(v)) modif = true;
                }
                Assert.AreEqual(modif, res);
            }
        }

        [TestMethod]
        public void GetFirstPossibilite_UneSeulePossibilite() {
            cell.AjouterPossibilité(4);
            Assert.AreEqual(4, cell.GetFirstPossibilité());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), AllowDerivedTypes =false)]
        public void GetFirstPossibilite_AucunePossibilite() {
            cell.GetFirstPossibilité();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), AllowDerivedTypes = false)]
        public void GetFirstPossibilite_CelluleFixe() {
            cell.AjouterPossibilité(2);
            cell.AjouterPossibilité(4);
            cell.FixerValeur(3);
            cell.GetFirstPossibilité();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), AllowDerivedTypes = false)]
        public void GetFirstPossibilite_CelluleTrouvée() {
            cell.AjouterPossibilité(2);
            cell.AjouterPossibilité(4);
            cell.ModifierValeur(3);
            cell.GetFirstPossibilité();
        }
    }
}
