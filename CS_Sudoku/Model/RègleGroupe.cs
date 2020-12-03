using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model
{
    public abstract class RègleGroupe : RègleSudoku
    {
        abstract public bool Appliquer(Groupe groupe, out string description);

        public override bool Appliquer(Sudoku sudoku, out string description)
        {
            this.Sudoku = sudoku;

            description = sudoku.ToString();
            bool modif = false;

            foreach (Groupe g in sudoku.Groupes)
            {
                if (this.Appliquer(g, out string s))
                {
                   description += s;
                    modif = true;

                    if (this.ModePasAPas) break;

                }

            }

            description += sudoku.ToString();

            this.AddToLog(description);

            return modif;
        }
    }
}
