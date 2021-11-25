using CS_Sudoku.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.ViewModel {
    public class VMBlocCell {
        private int num;
        // TODO
        // Générer le modèle Sudoku
        private Sudoku grille;
        private VMCell[] _array = new VMCell[9];

        // TODO Générer le modèle Sudoku et l'envoyer au constructeur de VMBlocCell
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="num"></param>
        public VMBlocCell(Sudoku grille, int num)
        {
            this.grille = grille;
            this.num = num;
            for (int i = 0; i < _array.Length; ++i)
            {
                _array[i] = new VMCell(grille[num / 3 * 3 + i / 3, num % 3 * 3 + i % 3]);
            }
        }

        public void Refresh() {
            foreach (VMCell vmC in _array) {
                vmC.Refresh();
            }
        }

        public VMCell this[int i] {
            get { return this._array[i]; }
        }

        public int Concerned {
            set {
                foreach (VMCell c in _array) {
                    c.Concerned = value;
                }
            }
        }

        public void ClearAll( bool all = true) {
            foreach (VMCell cell in _array) {
                cell.ClearAll(all);
            }
        }
    }
}
