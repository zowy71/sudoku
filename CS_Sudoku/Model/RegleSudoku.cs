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
        /// Constructeur permettant d'initialiser le nom du fichier et le mode pas à pas
        /// </summary>
        /// <param name="filename">Nom du fichier "log"</param>
        /// <param name="pasAPas">Détermine si on est en mode pas à pas ou non</param>
        public RègleSudoku(string filename=null, bool pasAPas = false)
        {
            _filename = filename;
            ModePasAPas = pasAPas;
        }
        /// <summary>
        /// Référence au Sudoku sur lequel on travaille....
        /// </summary>
        public Sudoku Sudoku { get; protected set; }

        private StreamWriter _log; // sauvegarde de modif sur un fichiers
        private string _filename; // nom fichier

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
        public bool Appliquer(Sudoku sudoku, out string description)
        {
            if (_filename != null)
            {
                _log = new StreamWriter(_filename, true);
            }

            bool res = DoAppliquer(out description);
            
            if (_log != null)
            {
                _log.Close();
                _log = null;
            }

            return res;

        }

        /// <summary>
        /// Applique une règle sur le sudoku enregistré dans la propriété Sudoku
        /// </summary>
        /// <param name="description">Description des modifications apportées au sudoku</param>
        /// <returns></returns>
        protected abstract bool DoAppliquer(out string description);

        /// <summary>
        /// Cette méthode ajoute une description de modification dans le 
        /// fichier référencé par la propriété Log
        /// </summary>
        /// <param name="str">Description à ajouter au fichier</param>
        public void AddToLog(string str)
        {
            if (_log != null)
            {
                _log.WriteLine(str);
            }
        }
    }
}
