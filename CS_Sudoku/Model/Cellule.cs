using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.Model
{
    /// <summary>
    /// Classe modélisant une case de Sudoku (le sudoku contenant 9x9 cases).
    /// </summary>
    public class Cellule
    {
        #region Constante
        /// <summary>
        /// Valeur à donner lorsque la cellule n'est pas "trouvée".
        /// </summary>
        public const int NONE = 0;
        #endregion

        private int __valeur = Cellule.NONE;

        #region Propriétés
        /// <summary>
        /// Nom de la cellule (exemple : "Cellule B3")
        /// </summary>
        public string Nom { get { return "Cellule " + NomCourt; } }
        /// <summary>
        /// Nom court de la cellule ne contenant que le numéro (exemple : "B3")
        /// </summary>
        public string NomCourt { get { return Ligne.NomCourt + Colonne.NomCourt; } }
        /// <summary>
        /// Détermine si la cellule peut être modifiée ou non par l'utilisateur.
        /// </summary>
        public bool Fixé { get; private set; }
        /// <summary>
        /// Détermine si la cellule possède une valeur (<code>true</code>) 
        /// ou uniquement des possibilités (<code>false</code>). Si la cellule
        /// est dans l'état "Fixé", la propriété "Trouvé" retourne <code>false</code>.
        /// </summary>
        public bool Trouvé { get { return !Fixé && this.Valeur != Cellule.NONE; } }
        /// <summary>
        /// Renvoie la valeur de la cellule. Si la cellule ne possède pas de valeur, la propriété renvoie
        /// <code>Cellule.NONE</code>.
        /// </summary>
        public int Valeur {
            get => __valeur;
            private set {
                if (value != __valeur)
                {
                    if (__valeur != Cellule.NONE)
                    {
                        SupprimerValeurFixéeOuTrouvée(__valeur);
                    }
                    __valeur = value;
                    if (__valeur != Cellule.NONE)
                    {
                        AjouterValeurFixéeOuTrouvée(__valeur);
                    }
                }
            }
        }
        /// <summary>
        /// Ensemble des possibilités (des chiffres que peut contenir cette cellule).
        /// </summary>
        public SortedSet<int> Possibilités { get; private set; }
        /// <summary>
        /// Ligne dans laquelle se trouve la cellule
        /// </summary>
        public Ligne Ligne { get; }
        /// <summary>
        /// Colonne dans laquelle se trouve la cellule
        /// </summary>
        public Colonne Colonne { get; }
        /// <summary>
        /// Bloc dans lequel se trouve la cellule
        /// </summary>
        public Bloc Bloc { get; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur permettant d'initialiser la ligne, la colonne et le bloc dans lesquels
        /// se trouve la cellule à créer.
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="colonne">Colonne de la cellule</param>
        /// <param name="bloc">Bloc de la cellule</param>
        public Cellule(Ligne ligne, Colonne colonne, Bloc bloc)
        {
            Ligne = ligne;
            Colonne = colonne;
            Bloc = bloc;
            Possibilités = new SortedSet<int>();

            // La cellule s'ajoute elle-même dans la ligne, la colonne et le bloc où elle se trouve
            Ligne.Add(this);
            Colonne.Add(this);
            Bloc.Add(this);
        }

        /// <summary>
        /// Constructeur "par copie", renseignant en outre la ligne, la colonne et le bloc.
        /// </summary>
        /// <param name="cellule">Cellule à copier</param>
        /// <param name="li">Ligne de la nouvelle cellule</param>
        /// <param name="co">Colonne de la nouvelle cellule</param>
        /// <param name="bl">Bloc de la nouvelle cellule</param>
        public Cellule(Cellule cellule, Ligne li, Colonne co, Bloc bl) : this(li, co, bl)
        {
            Possibilités = new SortedSet<int>(cellule.Possibilités);
            Fixé = cellule.Fixé;
            Valeur = cellule.Valeur;

        }
        #endregion

        #region Méthodes

        private void AjouterValeurFixéeOuTrouvée(int v)
        {
            Ligne.AjouterValeurFixéeOuTrouvée(v);
            Colonne.AjouterValeurFixéeOuTrouvée(v);
            Bloc.AjouterValeurFixéeOuTrouvée(v);
        }

        private void SupprimerValeurFixéeOuTrouvée(int v)
        {
            Ligne.SupprimerValeurFixéeOuTrouvée(v);
            Colonne.SupprimerValeurFixéeOuTrouvée(v);
            Bloc.SupprimerValeurFixéeOuTrouvée(v);
        }

        /// <summary>
        /// Permet de fixer la valeur de la cellule. L'utilisateur ne pourra pas la modifier.
        /// Cette méthode ne peut fonctionner que si la cellule n'a pas déjà été fixée.
        /// </summary>
        /// <param name="valeur">Valeur que prendra la cellule</param>
        /// <returns><code>true</code> si la cellule a effectivement été modifiée,
        /// <code>false</code> sinon.</returns>
        /// <exception cref="ArgumentOutOfRangeException">si la valeur n'est pas 
        /// comprise entre 1 et 9.</exception>
        public bool FixerValeur(int valeur)
        {
            if (valeur < 1 || valeur > 9)
            {
                throw new ArgumentOutOfRangeException("Cellule: La valeur (" + valeur + ") doit être entre 1 et 9.");
            }
            bool res = false;
            if (!Fixé || Valeur != valeur)
            {
                Fixé = true;
                Valeur = valeur;
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Efface tout le contenu de la cellule.
        /// </summary>
        public void Effacer()
        {
            Valeur = Cellule.NONE;
            Fixé = false;
            Possibilités.Clear();
        }

        /// <summary>
        /// Modifie la valeur d'une cellule. Cette dernière passe à l'état "Trouvé".
        /// </summary>
        /// <remarks>Si la cellule est dans l'état "Fixé", cette méthode ne peut
        /// modifier la valeur de la cellule. Il faut déjà effacer le contenu
        /// de la cellule, puis modifier sa valeur.</remarks>
        /// <param name="v">Nouvelle valeur de la cellule</param>
        /// <returns><code>true</code> si la cellule a été effectivement modifiée, 
        /// <code>false</code> sinon.</returns>
        public bool ModifierValeur(int v)
        {
            bool res = false;
            if (v < 1 || v > 9)
            {
                throw new ArgumentOutOfRangeException("Cellule: la valeur (" + v + ") pour modifier doit être comprise entre 1 et 9.");
            }
            if (Fixé) return false;
            if (Valeur != v)
            {
                res = true;
                Valeur = v;
            }
            return res;
        }

        /// <summary>
        /// Supprime toute valeur affectée à la cellule.
        /// </summary>
        /// <remarks>Si la cellule est dans l'état "Fixé", la méthode ne fait rien.</remarks>
        /// <returns><code>true</code> si la cellule a été modifiée, <code>false</code> sinon.</returns>
        public bool SupprimerValeur()
        {
            if (Fixé || !Trouvé) return false;
            Valeur = Cellule.NONE;
            return true;
        }

        /// <summary>
        /// Permet d'ajouter une possibilité à la cellule.
        /// </summary>
        /// <param name="v">Valeur à ajouter aux possibilités</param>
        /// <returns><code>true</code> si la cellule a été modifiée, 
        /// <code>false</code> sinon.</returns>
        /// <remarks>Cette méthode ne fait rien si la cellule est
        /// dans l'état "Trouvé" ou "Fixé".</remarks>
        public bool AjouterPossibilité(int v)
        {
            if (v < 1 || v > 9)
            {
                throw new ArgumentOutOfRangeException("Cellule: La valeur (" + v + ") doit être comprise entre 1 et 9.");
            }
            if (Fixé || Trouvé) return false;
            return Possibilités.Add(v);
        }

        /// <summary>
        /// Permet de supprimer une possibilité de la cellule.
        /// </summary>
        /// <param name="v">Valeur à retirer des possibilités.</param>
        /// <returns><code>true</code> si la cellule a été modifiée, 
        /// <code>false</code> sinon.</returns>
        /// <remarks>Si la cellule est dans l'état "Trouvé" ou "Fixé",
        /// cette méthode ne fait rien.</remarks>
        public bool SupprimerPossibilité(int v)
        {
            if (Fixé || Trouvé) return false;
            return Possibilités.Remove(v);
        }

        /// <summary>
        /// Permet de remplir une cellule avec toutes les possibilités
        /// de 1 à 9.
        /// </summary>
        /// <remarks>Si la cellule est dans l'état "Trouvé" ou "Fixé",
        /// cette méthode ne fait rien.</remarks>
        public void RemplirPossibilités()
        {
            if (!Fixé && !Trouvé)
            {
                for (int i = 1; i < 10; i++)
                {
                    AjouterPossibilité(i);
                }
            }
        }

        /// <summary>
        /// Permet de savoir si une cellule contient une valeur donnée dans 
        /// ses possibilités.
        /// </summary>
        /// <param name="v">Valeur à tester</param>
        /// <returns><code>true</code> si la valeur apparaît dans les 
        /// possibilités, <code>false</code> sinon.</returns>
        /// <remarks>Le résultat est sans valeur si la cellule est
        /// dans l'état "fixé" ou "trouvé".</remarks>
        /// <remarks>Si la cellule est dans l'état "Fixé" ou "Trouvé",
        /// la méthode retourne <code>false</code>.</remarks>
        public bool Contient(int v)
        {
            if (Fixé || Trouvé) return false;
            return Possibilités.Contains(v);
        }

        /// <summary>
        /// Méthode permettant de savoir si la cellule contient toutes les 
        /// possibilités énumérées en paramètre.
        /// </summary>
        /// <param name="vals">Liste des valeurs à tester.</param>
        /// <returns><code>true</code> si la cellule contient toutes les valeurs,
        /// <code>false</code> sinon.</returns>
        /// <remarks>Si la cellule est dans l'état "Fixé" ou "Trouvé",
        /// la méthode retourne <code>false</code>.</remarks>
        public bool Contient(IEnumerable<int> vals)
        {
            if (Fixé || Trouvé) return false;
            return Possibilités.IsSupersetOf(vals);
        }

        /// <summary>
        /// Détermine si la cellule contient ou non une des valeurs données en paramètre
        /// </summary>
        /// <param name="vals">Liste des valeurs à tester</param>
        /// <returns><code>true</code> si la cellule ne contient aucune de ces valeurs,
        /// <code>false</code> sinon.</returns>
        /// <remarks>Si la cellule est dans l'état "Fixé" ou "Trouvé",
        /// la méthode retourne <code>false</code>.</remarks>
        public bool NeContientPas(IEnumerable<int> vals)
        {
            if (Fixé || Trouvé) return false;
            foreach (int v in vals)
            {
                if (Contient(v)) return false;
            }
            return true;
        }

        /// <summary>
        /// Permet de supprimer une liste de possibilités
        /// </summary>
        /// <param name="vals">Liste des valeurs à supprimer des possibilités</param>
        /// <returns><code>true</code> si la cellule a été modifiée,
        /// <code>false</code> sinon.</returns>
        public bool SupprimerPossibilité(IEnumerable<int> vals)
        {
            if (Fixé || Trouvé) return false;
            int n = Possibilités.Count;
            Possibilités.ExceptWith(vals);
            return n != Possibilités.Count;
        }

        /// <summary>
        /// Retourne le nombre de possbilités que contient la cellule.
        /// </summary>
        /// <remarks>Si la cellule est dans l'état "Trouvé" ou "Fixé",
        /// la méthode retourne 0.</remarks>
        /// <returns>Nombre de possibilités de la cellule.</code></returns>
        public int NbPossibilités()
        {
            return Fixé || Trouvé ? 0 : Possibilités.Count;
        }

        /// <summary>
        /// Supprime toutes les possibilités sauf celle passée en paramètre
        /// </summary>
        /// <param name="v">Possibilité à ne pas supprimer</param>
        /// <returns><code>true</code> si la cellule a été modifiée,
        /// <code>false</code> sinon.</returns>
        /// <remarks>Si 
        /// la cellule est dans l'état "Trouvé",
        /// la méthode ne fait rien et retourne <code>false</code>.</remarks>
        public bool SupprimerExcepté(int v)
        {
            if (Fixé || Trouvé) return false;
            bool res = false;
            bool _c = Possibilités.Contains(v);
            int _n = Possibilités.Count;
            if (_n > 1 || (_n == 1 && !_c)) res = true;
            Possibilités.Clear();
            if (_c) Possibilités.Add(v);
            return res;
        }

        /// <summary>
        /// Permet de supprimer toutes les possibilités de la cellule,
        /// sauf celles passées en paramètre
        /// </summary>
        /// <param name="vals">Valeurs à conserver</param>
        /// <returns><code>true</code> si la cellule a été modifiée,
        /// <code>false</code> sinon.</returns>
        /// <remarks>Si la cellule est dans l'état "Fixé" ou "Trouvé",
        /// la méthode retourne <code>false</code>.</remarks>
        public bool SupprimerExcepté(IEnumerable<int> vals)
        {
            if (Fixé || Trouvé) return false;
            int n = Possibilités.Count;
            Possibilités.IntersectWith(vals);
            return n != Possibilités.Count;
        }

        /// <summary>
        /// Permet de retourner la première possibilité de la cellule (l'ordre n'est pas fixé !)
        /// </summary>
        /// <returns>Première possibilité de la cellule</returns>
        /// <exception cref="InvalidOperationException">Si la cellule est dans l'état Fixé ou Trouvé
        /// ou si elle ne possède aucune possibilité.</exception>
        public int GetFirstPossibilité()
        {
            if (Fixé || Trouvé || Possibilités.Count == 0)
            {
                throw new InvalidOperationException("Cellule: Cette cellule (" + NomCourt
                    + ")  n'a aucune possibilité" + (Fixé || Trouvé ? " car elle est fixée/trouvée." : "."));
            }
            IEnumerator<int> iter = Possibilités.GetEnumerator();
            iter.MoveNext();
            return iter.Current;
        }

        #endregion

        #region Redéfinition des méthodes
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Cellule c)
            {
                return c.Ligne.Numéro == this.Ligne.Numéro &&
                    c.Colonne.Numéro == this.Colonne.Numéro;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Ligne.Numéro * 10 + Colonne.Numéro;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder((Valeur == Cellule.NONE ? "-" : Valeur.ToString()) + " : [");
            bool first = true;
            foreach (int _i in Possibilités)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }
                res.Append(_i);
            }
            res.Append("]");
            return res.ToString();
        }

        #endregion

        #region Redéfinition des opérateurs
        public static bool operator ==(Cellule c1, Cellule c2)
        {
            if (object.ReferenceEquals(c1, null))
            {
                return object.ReferenceEquals(c2, null);
            }
            return c1.Equals(c2);
        }

        public static bool operator !=(Cellule c1, Cellule c2)
        {
            return !(c1 == c2);
        }
        #endregion

    }
}

