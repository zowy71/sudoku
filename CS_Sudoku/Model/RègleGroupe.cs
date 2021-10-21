using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model
{
    public abstract class RègleGroupe : RègleSudoku
    {
        abstract protected bool Appliquer(Groupe groupe, out string description);

        protected override bool DoAppliquer(out string description)
        {
            throw new NotImplementedException();
        }
    }
}
