using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model {
    public class Sudoku {
        /// <summary>
        /// Grille du sudoku
        /// </summary>
        public Cellule[,] Grille { get; private set; }
        /// <summary>
        /// Lignes du sudoku
        /// </summary>
        public List<Ligne> Lignes { get; private set; }
        /// <summary>
        /// Colonnes du sudoku
        /// </summary>
        public List<Colonne> Colonnes { get; private set; }
        /// <summary>
        /// Blocs du sudoku
        /// </summary>
        public List<Bloc> Blocs { get; private set; }
        /// <summary>
        /// Ensemble des lignes, colonnes et blocs
        /// </summary>
        public List<Groupe> Groupes { get; private set; }

        /// <summary>
        /// Permet d'allouer l'espace mémoire pour les groupes.
        /// </summary>
        /// <remarks>Tout doit être allouer, chaque ligne, bloc et colonne...</remarks>
        private void AllouerGroupes()
        {
            this.Lignes = new List<Ligne>();
            this.Colonnes = new List<Colonne>();
            this.Blocs = new List<Bloc>();
            this.Groupes = new List<Groupe>();

            // allocation de chaque ligne, colonne et bloc
            for (int i=0; i<9; i++)
            {
                this.Lignes.Add(new Ligne(i));
                this.Colonnes.Add(new Colonne(i));
                this.Blocs.Add(new Bloc(i));
            }

            // Mettre à jour l'ensemble des groupes
            this.Groupes.AddRange(this.Lignes);
            this.Groupes.AddRange(this.Colonnes);
            this.Groupes.AddRange(this.Blocs);

        }

        /// <summary>
        /// Permet de récupérer le numéro du bloc en fonction de la ligne et de la colonne
        /// </summary>
        /// <param name="li">Numéro de ligne commençant à 0</param>
        /// <param name="co">Numéro de colonne commençant à 0</param>
        /// <returns>Numéro de bloc correspondant.</returns>
        private static int GetNuméroBloc(int li, int co)
        {
            return li / 3 * 3 + co / 3;
        }

         /// <summary>
        /// Constructeur par défaut.
        /// 
        /// Construit la grille 9x9 et classe les cellules en fonction 
        /// de leur ligne, colonne et bloc.
        /// </summary>
        /// 
        public Sudoku()
        {
            AllouerGroupes();
            this.Grille = new Cellule[9, 9];
            for (int li = 0; li < 9; li++)
            {
                for (int co = 0; co < 9; co++)
                {
                    this.Grille[li, co] = new Cellule(Lignes[li], Colonnes[co], Blocs[GetNuméroBloc(li, co)]);
                }
            }
        }

        /// <summary>
        /// Constructeur par copie
        /// 
        /// Duplique complètement une grille de sudoku en évitant le partage mémoire.
        /// </summary>
        /// <param name="sudo">Grille de sudoku à copier</param>
        public Sudoku(Sudoku sudo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Accès à une cellule du sudoku
        /// </summary>
        /// <param name="i">Ligne de la cellule</param>
        /// <param name="j">Colonne de la cellule</param>
        /// <returns></returns>
        public Cellule this[int i, int j] {
            get { return Grille[i, j]; }
        }

        /// <summary>
        /// Efface toutes les cellules du Sudoku
        /// </summary>
        /// <see cref="Cellule.Effacer"/>
        public void Effacer()
        {
            // Parcourir le tableau this.Grille
            // Appeler la méthode Effacer() sur chaque cellule de la grille.
            foreach (Cellule c in this.Grille)
            {
                c.Effacer();
            }
        }

        /// <summary>
        /// Remplit avec les valeurs de 1 à 9 les possibilités de toutes les cellules
        /// qui ne sont pas dans l'état "Trouvé" ou "Fixé".
        /// </summary>
        public void RemplirPossibilités()
        {
            // Parcourir le tableau this.Grille
            // Appeler la méthode RemplirPossibilités sur chaque cellule de la grille
            foreach (Cellule c in this.Grille)
            {
                c.RemplirPossibilités();
            }
        }

        /// <summary>
        /// Détermine si le sudoku est dans un "état complet".
        /// 
        /// Un "état complet" est un état dans lequel toutes les cellules sont dans un état
        /// soit "Fixé", soit "Trouvé"
        /// </summary>
        /// <returns><code>true</code> si le sudoku est "résolu",
        /// <code>false</code> sinon.</returns>
        public bool EstComplète()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permet de déterminer si la grille vérifie les règles du Sudoku.
        /// Si c'est le cas, le résultat est "Ok !" et la méthode retourne <code>true</code>.
        /// 
        /// Dans le cas contraire, la chaîne de caractères contient la raison et la méthode retourne <code>false</code>.
        /// </summary>
        /// <param name="résultat">Chaîne contenant la raison du non respect des règles du sudoku le cas échéant.</param>
        /// <returns><code>true</code> si la grille vérifie les règles du Sudoku, <code>false</code> sinon.</returns>
        /// <remarks>Cette méthode ne doit être appelée que si le sudoku est complet.</remarks>
        /// <see cref="Sudoku.EstComplète"/>
        public bool Vérifier(out string résultat)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permet de déterminer si la grille est résoluble.
        /// 
        /// La grille est résoluble si les cases non trouvées possèdent au moins 1 possibilité,
        /// et si les lignes, colonnes et blocs ne contiennent pas plus d'une fois chaque chiffre
        /// de 1 à 9.
        /// </summary>
        /// <param name="explication">Contiendra l'explication si la grille n'est pas résoluble</param>
        /// <returns><code>true</code> si la grille est résoluble, <code>false</code> sinon.</returns>
        public bool EstRésoluble(out string explication)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Méthode permettant de charger une grille de sudoku à partir d'un fichier
        /// au format CSV.
        /// 
        /// Inconvénient : ne permet pas de charger une grille en cours de modification 
        /// (valeurs trouvées, possibilités, etc.)...
        /// </summary>
        /// <param name="filename">Nom du fichier</param>
        /// <param name="rapport">Chaîne contenant les rapports d'erreurs rencontrées</param>
        /// <returns><code>true</code> si tout s'est bien passé, <code>false</code> sinon.</returns>
        public bool ChargerCSV(string filename, out string rapport)
        {
            rapport = "";
            bool res = true;
            using (StreamReader sr = new StreamReader(filename))
            {
                // On efface la grille
                Effacer();
                int nbLine = 0;
                string line;
                bool tropDeLignes = false;
                while ((line = sr.ReadLine()) != null && !tropDeLignes)
                {
                    if (!string.IsNullOrEmpty(line) && line[0] != '#')
                    {
                        if (nbLine < 9)
                        {
                            ++nbLine;
                            string[] fields = line.Split(';');
                            if (fields.Length != 9)
                            {
                                res = false;
                                rapport += "La ligne " + nbLine + " contient " + fields.Length
                                    + " champs au lieu de 9.\n";
                            }
                            for (int i = 0; i < 9 && i < fields.Length; i++)
                            {
                                if (!string.IsNullOrWhiteSpace(fields[i]))
                                {
                                    if (int.TryParse(fields[i], out int valeur))
                                    {
                                        // TODO A continuer !!
                                        Grille[nbLine - 1, i].FixerValeur(valeur);
                                    }
                                    else
                                    {
                                        res = false;
                                        rapport += "Le champs n°" + (i + 1) + " de la ligne " + nbLine
                                            + " ne contient pas une valeur numérique.\n";
                                    }
                                }
                            }
                        }
                        else
                        {
                            res = false;
                            rapport += "Le fichier contient trop de lignes.\n";
                            tropDeLignes = true;
                        }
                    }
                }
                if (nbLine < 9)
                {
                    res = false;
                    rapport += "Le fichier ne contient que " + nbLine + " au lieu de 9.";
                }
            }
            return res;
        }

        /// <summary>
        /// Sauvegarde du Sudoku au format CSV : 
        /// Chaque ligne contient 8 points virgules séparant au plus 9 chiffres
        /// Le fichier doit contenir 9 lignes
        /// Ce format ne permet pas de sauvegarder les possibilités.
        /// </summary>
        /// <param name="fichier">Nom du fichier de sauvegarde</param>
        public void SauvegarderCSV(string fichier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Affichage en mode texte
        /// </summary>
        /// <returns>Chaîne de caractères contenant une représentation du sudoku</returns>
        public override string ToString()
        {
            string line = "+---+---+---++---+---+---++---+---+---+\n";
            StringBuilder res = new StringBuilder(line);
            // Construction des lignes !!
            for (int li = 0; li < 9; li++)
            {
                // Chaque ligne de la grille est affichée sur 3 lignes écran
                for (int li_ec = 0; li_ec < 3; li_ec++)
                {
                    res.Append("|");
                    for (int co = 0; co < 9; co++)
                    {
                        Cellule c = Grille[li, co];
                        // Chaque colonne est affichée sur 3 colonnes écran
                        for (int co_ec = 0; co_ec < 3; co_ec++)
                        {
                            if (li_ec * co_ec == 1)
                            {  // On est au milieu de la cellule
                                if (c.Fixé || c.Trouvé)
                                {
                                    res.Append(c.Valeur);
                                    continue;
                                }
                            }
                            if (li_ec == 1 && co_ec == 2 && c.Trouvé)
                            {
                                res.Append(".");
                                continue;
                            }
                            if (li_ec == 2 && co_ec == 1 && c.Fixé)
                            {
                                res.Append("-");
                                continue;
                            }
                            int p = li_ec * 3 + co_ec + 1;
                            res.Append(c.Contient(p) ? p.ToString() : " ");
                        }
                        res.Append("|");
                        if ((co + 1) % 3 == 0 && co != 8)
                        {
                            res.Append("|");
                        }
                    }
                    res.Append("\n");
                }
                res.Append(line);
                if ((li + 1) % 3 == 0 && li != 8)
                {
                    res.Append(line);
                }
            }
            return res.ToString();
        }
    }
}
