using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model
{
    public class SingletonNu : RègleGroupe
    {
        public override bool Appliquer(Groupe groupe, out string description)
        {
            bool res = false;

            description = "";
            foreach (Cellule c in groupe)
            {
                if ( !c.Trouvé && !c.Fixé && c.Possibilités.Count == 1)
                {
                    c.ModifierValeur( c.Possibilités.First() );
                }
                if (c.Trouvé || c.Fixé)
                {
                    if (groupe.SupprimerPossibilité(c.Valeur, c))
                    {
                        res = true;
                        description += $"\nSingleton nu : La {c.Nom} ne contient que {c.Valeur} dans le groupe {groupe.Nom},"
                             + $"{c.Valeur} est supprimé du reste du groupe.\n";
                        if (this.ModePasAPas) break;
                    }
                }
            }

            return res;
        }
    }
}
