using CS_Sudoku.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CS_Sudoku.ViewModel {
    public class VMSudoku {
        // TODO : Créer le modèle Sudoku
        private Sudoku grille = new Sudoku();
        private VMBlocCell[] _array = new VMBlocCell[9];
        private static readonly Random rnd = new Random();

        /// <summary>
        /// Utilisé pour tous les tirages aléatoires
        /// </summary>
        public static Random Rnd => rnd;

        public VMSudoku() {
            for (int i = 0; i < _array.Length; ++i) {
                _array[i] = new VMBlocCell(grille, i);
            }

            /*foreach (VMBlocCell bc in _array) {
                bc.Concerned = 2; 
            }*/
        }

        public int Concerned {
            set {
                foreach (VMBlocCell bc in _array) {
                    bc.Concerned = value;
                }
            }
        }

        public VMBlocCell this[int i] {
            get { return _array[i]; }
        }

        public void ClearAll() {
            // On efface le contenu de toutes les cellules
            foreach (VMBlocCell bc in _array) {
                bc.ClearAll();
            }
        }

        public void Refresh() {
            foreach (VMBlocCell vmB in _array) {
                vmB.Refresh();
            }
        }

        /// <summary>
        /// Chargement d'un fichierv au format CSV
        /// </summary>
        /// <param name="fileName"></param>
        public bool LoadCSV(string fileName, out string rapport) {
            try
            {
                bool res = grille.ChargerCSV(fileName, out rapport);
                Refresh();
                return res;
            }
            catch (NotImplementedException)
            {
                rapport = "Non implémenté.";
                return false;
            }
        }

        public bool AppliquerRègleSingletonCaché(string file, bool pasAPas, out string res) {
            //SingletonCaché sc = new SingletonCaché {
            //    ModePasAPas = pasAPas
            //};
            //return AppliquerRègle(sc, file, out res);
            res = "Non implémenté.";
            return false;
        }

        public void RemplirPossibilités() {
            try
            {
                grille.RemplirPossibilités();
            }
            catch (NotImplementedException)
            {
                MessageBox.Show("Routine non implémentée...");
            }
            Refresh();
        }

        public bool AppliquerRègleSingletonNu(string file, bool pasAPas, out string res) {
            SingletonNu sn = new SingletonNu
            {
                ModePasAPas = pasAPas
            };
            return AppliquerRègle(sn, file, out res);
            //res = "Non implémenté.";
            //return false;
        }

        public bool AppliquerRègleInteractionLigneColonneBloc(string file, bool pasAPas, out string res) {
            //InteractionLigneColonneBloc ilcb = new InteractionLigneColonneBloc { ModePasAPas = pasAPas };
            //return AppliquerRègle(ilcb, file, out res);
            res = "Non implémenté.";
            return false;
        }

        public bool AppliquerRègleGroupeCaché(string file, bool pasAPas, out string res) {
            res = "En cours d'écriture...";
            return true;
        }

        public bool AppliquerRègleGroupeNu(string file, bool pasAPas, out string res) {
            res = "En cours d'écriture...";
            return true;
        }

        public bool AppliquerRègleXWing(string file, bool pasAPas, out string res) {
            res = "En cours d'écriture...";
            return true;
        }

        public bool AppliquerRègleSwordFish(string file, bool pasAPas, out string res) {
            res = "En cours d'écriture...";
            return true;
        }

        public bool AppliquerBackTracking(string file, bool pasAPas, out string res) {
            res = "En cours d'écriture...";
            return true;
        }

        public bool Résoudre(string file, bool pasAPas, out string res) {
            res = "En cours d'écriture...";
            return true;
        }

        public bool AppliquerRègle(RègleSudoku règle, string file, out string res) {
            if (file != null) {
                StreamWriter sw = new StreamWriter(file, true);
                règle.Log = sw;
            }
            bool ret = règle.Appliquer(grille, out res);
            if (ret) Refresh();
            if (règle.Log != null) { règle.Log.Close(); }
            return ret;

        }
    }
}
