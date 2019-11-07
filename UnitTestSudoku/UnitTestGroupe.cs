using CS_Sudoku.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestSudoku {
    [TestClass]
    public class UnitTestGroupe {
        private int li = 0, co = 0;
        [TestInitialize]
        public void Setup() {
            li = co = 0;
        }

        private Cellule GénérerCellule(int valeurFixe=Cellule.NONE, int valeurTrouvé=Cellule.NONE, List<int> possibilités = null) {
            Cellule c = new Cellule(new Ligne(li), new Colonne(co), new Bloc(0));
            if (++li>8) {
                li = 0;
                ++co;
            }
            if (possibilités!=null) { 
                foreach (int n in possibilités) {
                    c.AjouterPossibilité(n);
                }
            }
            if (valeurFixe!=Cellule.NONE) {
                c.FixerValeur(valeurFixe);
            } else if (valeurTrouvé!=Cellule.NONE) {
                c.ModifierValeur(valeurTrouvé);
            }
            return c;
        }

        private Groupe GénérerGroupe(params Cellule[] c) {
            Groupe g = new Groupe();
            foreach(Cellule _c in c) {
                g.Add(_c);
            }
            return g;
        }

        [TestMethod]
        public void SupprimerPossibilités_GroupeModifié() {
            Cellule c = null;
            Groupe g = GénérerGroupe(
                GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> {  1, 2, 3}),
                GénérerCellule(possibilités: new List<int> {  1, 2, 3}),
                GénérerCellule(possibilités: new List<int> {  4, 5, 6}),
                c = GénérerCellule(possibilités: new List<int>{ 1, 2 })
                );
            Assert.IsTrue(g.SupprimerPossibilité(1, c));
            foreach (Cellule _c in g) {
                if (_c.Fixé || _c.Trouvé) {
                    Assert.IsTrue(_c.Possibilités.Contains(1));
                } else if (object.ReferenceEquals(_c, c)) {
                    Assert.IsTrue(_c.Possibilités.Contains(1));
                } else {
                    Assert.IsFalse(_c.Possibilités.Contains(1));
                }
            }
        }

        [TestMethod]
        public void SupprimerPossibilités_GroupeNonModifié() {
            Cellule c = null;
            Groupe g = GénérerGroupe(
                GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 7, 3 }),
                GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> { 1, 7, 3 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(possibilités: new List<int> { 4, 5, 6 }),
                c = GénérerCellule(possibilités: new List<int> { 1, 2, 7 })
                );
            Assert.IsFalse(g.SupprimerPossibilité(7, c));
            foreach (Cellule _c in g) {
                if (_c.Fixé || _c.Trouvé) {
                    Assert.IsTrue(_c.Possibilités.Contains(7));
                } else if (object.ReferenceEquals(_c, c)) {
                    Assert.IsTrue(_c.Possibilités.Contains(7));
                } else {
                    Assert.IsFalse(_c.Possibilités.Contains(7));
                }
            }
        }

        [TestMethod]
        public void SupprimerPossibilités2_GroupeModifié() {
            Cellule c = null;
            Groupe g = GénérerGroupe(
                GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 3, 4 }),
                GénérerCellule(possibilités: new List<int> { 4, 5, 6 }),
                c = GénérerCellule(possibilités: new List<int> { 1, 2 })
                );
            Assert.IsTrue(g.SupprimerPossibilité(new int[] { 1, 2 }, c));
            foreach (Cellule _c in g) {
                if (_c.Fixé || _c.Trouvé || object.ReferenceEquals(c,_c)) {
                    Assert.IsTrue(_c.Possibilités.Contains(1), "Err : " + _c);
                    Assert.IsTrue(_c.Possibilités.Contains(2), "Err : " + _c);
                } else {
                    Assert.IsFalse(_c.Possibilités.Contains(1));
                    Assert.IsFalse(_c.Possibilités.Contains(2));
                }
            }
        }

        [TestMethod]
        public void SupprimerPossibilités2_GroupeNonModifié() {
            Cellule c = null;
            Groupe g = GénérerGroupe(
                GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 7, 8, 3 }),
                GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> { 1, 7, 8, 3 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(possibilités: new List<int> { 4, 5, 6 }),
                c = GénérerCellule(possibilités: new List<int> { 1, 2, 7, 8 })
                );
            Assert.IsFalse(g.SupprimerPossibilité(new int[] { 7, 8, 9 }, c));
            foreach (Cellule _c in g) {
                if (_c.Fixé || _c.Trouvé || object.ReferenceEquals(c, _c)) {
                    Assert.IsTrue(_c.Possibilités.Contains(7));
                    Assert.IsTrue(_c.Possibilités.Contains(8));
                } else {
                    Assert.IsFalse(_c.Possibilités.Contains(7));
                    Assert.IsFalse(_c.Possibilités.Contains(8));
                }
            }
        }

        [TestMethod]
        public void SupprimerPossibilités3_GroupeModifié() {
            Cellule c1 = null, c2 = null;
            Groupe g = GénérerGroupe(
                GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> { 1, 2, 3 }),
                c2 = GénérerCellule(possibilités: new List<int> { 1, 2, 3, 6 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 3, 4 }),
                GénérerCellule(possibilités: new List<int> { 4, 5, 6 }),
                c1 = GénérerCellule(possibilités: new List<int> { 1, 2 })
                );
            Groupe except = new Groupe { c1, c2 };
            Assert.IsTrue(g.SupprimerPossibilité(new int[] { 1, 2 }, except));
            foreach (Cellule _c in g) {
                if (_c.Fixé || _c.Trouvé || except.Contains(_c)) {
                    Assert.IsTrue(_c.Possibilités.Contains(1), "Err : " + _c);
                    Assert.IsTrue(_c.Possibilités.Contains(2), "Err : " + _c);
                } else {
                    Assert.IsFalse(_c.Possibilités.Contains(1));
                    Assert.IsFalse(_c.Possibilités.Contains(2));
                }
            }
        }

        [TestMethod]
        public void SupprimerPossibilités3_GroupeNonModifié() {
            Cellule c1 = null, c2 = null;
            Groupe g = GénérerGroupe(
                GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 7, 8, 3 }),
                GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> { 1, 7, 8, 3 }),
                c2 = GénérerCellule(possibilités: new List<int> { 1, 2, 8, 6, 7 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(possibilités: new List<int> { 4, 5, 6 }),
                c1 = GénérerCellule(possibilités: new List<int> { 1, 2, 7, 8 })
                );
            Groupe except = new Groupe { c1, c2 };
            Assert.IsFalse(g.SupprimerPossibilité(new int[] { 7, 8, 9 }, except));
            foreach (Cellule _c in g) {
                if (_c.Fixé || _c.Trouvé || except.Contains(_c)) {
                    Assert.IsTrue(_c.Possibilités.Contains(7));
                    Assert.IsTrue(_c.Possibilités.Contains(8));
                } else {
                    Assert.IsFalse(_c.Possibilités.Contains(7));
                    Assert.IsFalse(_c.Possibilités.Contains(8));
                }
            }
        }

        [TestMethod]
        public void GetNbCellulesNonTrouvées_ValeurNonNulleEtNulle() {
            Cellule c = null;
            Groupe g = GénérerGroupe(
                c = GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 7, 8, 3 }),
                GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> { 1, 7, 8, 3 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 8, 6, 7 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 3 }),
                GénérerCellule(possibilités: new List<int> { 4, 5, 6 }),
                GénérerCellule(possibilités: new List<int> { 1, 2, 7, 8 })
                );
            Assert.AreEqual(4, g.GetNbCellulesNonTrouvées());
        }

        [TestMethod]
        public void GetCellulesContenantAuMoins_GroupeNonNulleEtNulle() {
            Cellule [] cells = null;
            Groupe g = GénérerGroupe(
                cells = new Cellule[] {
                    GénérerCellule(valeurFixe: 3, possibilités: new List<int> { 1, 7, 8, 3 }),
                    GénérerCellule(valeurTrouvé: 2, possibilités: new List<int> { 1, 7, 8, 3 }),
                    GénérerCellule(possibilités: new List<int> { 1, 2, 8, 6, 7 }),
                    GénérerCellule(possibilités: new List<int> { 1, 2, 3 }),
                    GénérerCellule(possibilités: new List<int> { 4, 5, 6 }),
                    GénérerCellule(possibilités: new List<int> { 1, 2, 7, 8 })
                    }
                );
            Groupe res = g.GetCellulesContenantAuMoins(new int[] { 1,2 });
            // Vérification
            Assert.AreEqual(3, res.Count);
            foreach (int i in new int [] {  2, 3, 5}) {
                Assert.IsTrue(res.Contains(cells[i]));
            }
            foreach (int i in new int[] { 0, 1, 4 }) {
                Assert.IsFalse(res.Contains(cells[i]));
            }
            res = g.GetCellulesContenantAuMoins(new int[] { 1, 2, 4 });
            // Vérification
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void OperatorPlus_Union() {
            Cellule[] cells = new Cellule[] {
                GénérerCellule(valeurFixe: 4, possibilités: new List<int> { 1, 2}),
                GénérerCellule(valeurTrouvé: 3, possibilités: new List<int> { 2, 4, 6}),
                GénérerCellule(possibilités: new List<int> {1, 2, 4, 7}),
                GénérerCellule(possibilités: new List<int> { 6, 7, 8, 9}),
                GénérerCellule(possibilités: new List<int> { 1, 2, 8, 9})
            };
            Groupe g1 = GénérerGroupe(cells[0], cells[2], cells[3], cells[4]);
            Groupe g2 = GénérerGroupe(cells[1], cells[2], cells[3]);

            Assert.AreEqual(4, g1.Count);
            Assert.AreEqual(3, g2.Count);

            Groupe g = g1 + g2;
            Assert.AreEqual(5, g.Count);
            foreach (Cellule c in cells) {
                Assert.IsTrue(g.Contains(c));
            }
        }

        [TestMethod]
        public void OperatorMoins_PrivéDe() {
            Cellule[] cells = new Cellule[] {
                GénérerCellule(valeurFixe: 4, possibilités: new List<int> { 1, 2}),
                GénérerCellule(valeurTrouvé: 3, possibilités: new List<int> { 2, 4, 6}),
                GénérerCellule(possibilités: new List<int> {1, 2, 4, 7}),
                GénérerCellule(possibilités: new List<int> { 6, 7, 8, 9}),
                GénérerCellule(possibilités: new List<int> { 1, 2, 8, 9})
            };
            Groupe g1 = GénérerGroupe(cells[0], cells[2], cells[3], cells[4]);
            Groupe g2 = GénérerGroupe(cells[1], cells[2], cells[3]);

            Assert.AreEqual(4, g1.Count);
            Assert.AreEqual(3, g2.Count);

            Groupe g = g1 - g2;
            Assert.AreEqual(2, g.Count);
            // Appartenance
            foreach (Cellule c in new Cellule[] { cells[0], cells[4] }) {
                Assert.IsTrue(g.Contains(c));
            }
            // Non appartenance
            foreach (Cellule c in new Cellule[] { cells[1], cells[2], cells[3] }) {
                Assert.IsFalse(g.Contains(c));
            }
        }

        [TestMethod]
        public void OperatorMultiply_Intersection() {
            Cellule[] cells = new Cellule[] {
                GénérerCellule(valeurFixe: 4, possibilités: new List<int> { 1, 2}),
                GénérerCellule(valeurTrouvé: 3, possibilités: new List<int> { 2, 4, 6}),
                GénérerCellule(possibilités: new List<int> {1, 2, 4, 7}),
                GénérerCellule(possibilités: new List<int> { 6, 7, 8, 9}),
                GénérerCellule(possibilités: new List<int> { 1, 2, 8, 9})
            };
            Groupe g1 = GénérerGroupe(cells[0], cells[2], cells[3], cells[4]);
            Groupe g2 = GénérerGroupe(cells[1], cells[2], cells[3]);

            Assert.AreEqual(4, g1.Count);
            Assert.AreEqual(3, g2.Count);

            Groupe g = g1 * g2;
            Assert.AreEqual(2, g.Count);
            // Appartenance
            foreach (Cellule c in new Cellule[] { cells[2], cells[3] }) {
                Assert.IsTrue(g.Contains(c));
            }
            // Non appartenance
            foreach (Cellule c in new Cellule[] { cells[0], cells[1], cells[4] }) {
                Assert.IsFalse(g.Contains(c));
            }

        }

        [TestMethod]
        public void Contient_AvecCellulesFixésEtTrouvés() {
            Cellule[] cells = new Cellule[] {
                GénérerCellule(valeurFixe: 5, possibilités: new List<int> { 1, 2, 5}),
                GénérerCellule(valeurTrouvé: 3, possibilités: new List<int> { 2, 4, 3, 6}),
                GénérerCellule(possibilités: new List<int> {1, 2, 4, 7}),
                GénérerCellule(possibilités: new List<int> { 6, 7, 8}),
                GénérerCellule(possibilités: new List<int> { 1, 2, 8})
            };
            Groupe g = GénérerGroupe(cells);

            // Cellule Trouvé contenant 3
            Assert.IsFalse(g.Contient(3));
            // Cellule Fixée contenant 5
            Assert.IsFalse(g.Contient(5));
            // Cas classiques
            Assert.IsTrue(g.Contient(1));
            Assert.IsTrue(g.Contient(6));
            Assert.IsFalse(g.Contient(9));
        }

        [TestMethod]
        public void GetCellulesContenantAuPlus_MelangesCellulesFixéTrouvéEtAutre() {
            Cellule[] cells = {
                GénérerCellule(possibilités: new List<int> {2, 3, 4, 5}),
                GénérerCellule(valeurFixe: 4, possibilités: new List<int> { 1, 2}),
                GénérerCellule(valeurTrouvé: 3, possibilités: new List<int> { 1, 2}),
                GénérerCellule(possibilités: new List<int> { 1, 2, 3}),
                GénérerCellule(possibilités: new List<int> { 1, 2}),
                GénérerCellule(possibilités: new List<int> { 1, 3}),
                GénérerCellule(possibilités: new List<int> { 1, 3, 4})
            };
            Groupe g = GénérerGroupe(cells);
            Groupe g2 = g.GetCellulesContenantAuPlus(new int[] { 1, 2, 3 });
            Assert.AreEqual(3, g2.Count);
            foreach (int i in new int [] { 0, 1, 2, 6 }) {
                Assert.IsFalse(g2.Contains(cells[i]), "Ne devrait pas contenir " + cells[i]);
            }
            foreach (int i in new int[] { 3, 4, 5 }) {
                Assert.IsTrue(g2.Contains(cells[i]), "Devrait contenir " + cells[i]);
            }
        }
    }
}
