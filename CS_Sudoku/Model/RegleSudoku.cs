using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model {

 
    /// <summary>
    /// Classe générique d'application des règles de sudoku.
    /// </summary>
    public abstract class RègleSudoku {

        /// <summary>
        /// Référence au Sudoku sur lequel on travaille....
        /// </summary>
        public Sudoku Sudoku { get; protected set; }

        /// <summary>
        /// Fichier de log permettant d'enregistrer toutes les modifications apportées
        /// </summary>
        public StreamWriter Log { get; set; }
        /// <summary>
        /// Mode pas à pas : dès qu'une modification est apportée, stoppe l'application de la règle.
        /// </summary>
        public bool ModePasAPas { get; set; }

        /// <summary>
        /// Méthode permettant d'appliquer la règle sur une grille de sudoku passée en paramètre.
        /// </summary>
        /// <param name="sudoku">Grille de sudoku</param>
        /// <param name="description">Contiendra la description des modifications 
        /// apportées à la grille de sudoku.</param>
        /// <returns><code>true</code> si la grille est modifiée, <code>false</code> sinon</returns>
        public abstract bool Appliquer(Sudoku sudoku, out string description);

        /// <summary>
        /// Cette méthode ajoute une description de modification dans le 
        /// fichier référencé par la propriété Log
        /// </summary>
        /// <param name="str">Description à ajouter au fichier</param>
        public void AddToLog(string str)
        {
            //if (Log != null)
            //{
            //    Log.WriteLine(str);
            //}
            Log?.WriteLine(str);
        }
    }
}
