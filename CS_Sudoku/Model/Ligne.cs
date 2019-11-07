using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model
{
    public class Ligne : Groupe
    {
        /// <summary>
        /// Constructeur à partir du numéro de groupe
        /// </summary>
        /// <param name="numero">Numéro du groupe (entre 0 et 8)</param>
        /// <see cref="Groupe.Groupe(int)"/>
        public Ligne(int numero) : base(numero)
        {
        }

        /// <summary>
        /// Redéfinition du nom court du groupe : 
        /// Le nom court est de la forme :
        ///     Lettre
        /// où Lettre est une lettre avec la correspondance A pour 0, B pour 1, C pour 2, etc.
        /// </summary>
        public override string NomCourt => throw new NotImplementedException();

        /// <summary>
        /// Redéfinition du nom du groupe : 
        /// Le nom est de la forme : 
        ///     Ligne Lettre
        /// où Lettre est le nom court de la ligne.
        /// </summary>
        public override string Nom => throw new NotImplementedException();

    }
}
