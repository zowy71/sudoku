using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model
{
    public class Bloc : Groupe
    {
        /// <summary>
        /// Constructeur à partir du numéro de groupe
        /// </summary>
        /// <param name="numero">Numéro du groupe (entre 0 et 8)</param>
        /// <see cref="Groupe.Groupe(int)"/>
        public Bloc(int numero) : base(numero)
        {
        }

        /// <summary>
        /// Redéfinition du nom court du groupe : 
        /// Le nom court est de la forme :
        ///     b<code>num</code>
        /// où <code>num</code> est un entier compris entre 1 et 9.
        /// </summary>
        public override string NomCourt => throw new NotImplementedException();

        /// <summary>
        /// Redéfinition du nom du groupe : 
        /// Le nom est de la forme : 
        ///     bloc <code>num</code>
        /// où <code>num</code> est un entier compris entre 1 et 9.
        /// </summary>
        public override string Nom => throw new NotImplementedException();

    }
}
