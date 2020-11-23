using CS_Sudoku.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sudoku.ViewModel {
    public class VMCell : INotifyPropertyChanged {
        // TODO
        // Générer le modèle Cellule
        private Cellule cell;
        /// <summary>
        /// Les possibilités vont de 1 à 9
        /// </summary>
        public class Possibility : INotifyPropertyChanged {

            // Vrai si la possibilité est dans 
            private bool _is;

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            public bool Is { get => _is; set { _is = value; OnPropertyChanged("Opacity"); } }

            public double Opacity {
                get => Is ? 1.0 : 0.0;
            }
        }

        /// <summary>
        /// Repère le chiffre sélectionné par l'utilisateur dans l'interface graphique
        /// </summary>
        private int _selectedValue;
        // Un tableau de 10 cases : La case 0 ne sert à rien !
        private Possibility[] possibilités = new Possibility[10];

        //!! Temporaire : A supprimer quand le modèle cell sera fait !!
        private int __valeur = VMSudoku.Rnd.Next(0, 10);
        private bool __fixed = VMSudoku.Rnd.Next(2) == 1;
        // !! Fin Temporaire : A supprimer quand le modèle cell sera écrit !!


        public int IntValeur => __valeur;    // cell.Valeur;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Propriété permettant de savoir si la cellule doit être sélectionnée ou non.
        /// </summary>
        public bool IsConcerned {
            get => ((IsSet || IsFixed) && IntValeur == _selectedValue)
                || (!IsSet && !IsFixed && possibilités[_selectedValue].Is);
        }

        /// <summary>
        /// Propriété appelée lorsque l'utilisateur change de chiffre. 
        /// Détermine ainsi si la cellule doit être désélectionnée ou sélectionnée.
        /// </summary>
        public int Concerned {
            set {
                bool oldValue = IsConcerned;
                _selectedValue = value;
                bool newValue = IsConcerned;
                if (newValue != oldValue) {
                    OnPropertyChanged("IsConcerned");
                }
            }
        }

        // TODO : Ecrire le modèle Cellule
        // et le passer au constructeur
        /// <summary>
        /// Constructeur recevant le modèle correspondant
        /// </summary>
        /// <param name="cell"></param>
        public VMCell(/*Cellule cell*/) {
            // this.cell = cell;
            possibilités[0] = new Possibility { Is = false };
            for (int i = 1; i < 10; i++) {
                // TODO : A connecter au modèle
                possibilités[i] = new Possibility { Is = VMSudoku.Rnd.Next(2) == 1 };    // cell.Possibilités.Contains(i) };
            }
        }

        // TODO
        // A connecter au modèle !!
        public bool IsFixed => __fixed && __valeur!=0;   //cell.Fixé;
        // public bool IsSet => cell.Trouvé;
        public bool IsSet {  get { return !IsFixed && __valeur != 0; } }

        public void Refresh() {
            for (int i = 1; i < 10; i++) {
                // TODO : A connecter au modèle
                possibilités[i].Is = VMSudoku.Rnd.Next(2) == 1;      //cell.Possibilités.Contains(i);
            }
            OnPropertyChanged("IsFixed");
            OnPropertyChanged("IsSet");
            OnPropertyChanged("IsConcerned");
            OnPropertyChanged("Valeur");
        }

        /// <summary>
        /// Modifie la valeur et l'état de la cellule : 
        // Si la cellule possède une valeur et si on essaie d'affecter la même valeur : 
        // alors on "supprime" cette valeur dans le sens où la case réaffiche les possibilités.
        // Si on essaie d'affecter une autre valeur, alors qu'une valeur est déjà affectée, on refuse l'affectation.
        // Enfin, si aucune valeur n'était fixée, alors on accepte simplement la nouvelle valeur et la case
        // prend l'état "IsSet = true".
        /// </summary>
        /// <param name="nv">Nouvelle valeur à affecter</param>
        public void SetValue(int nv) {
            if (IsFixed) return; // Ne pas modifier si la cellule est fixée.
            if (IsSet) {
                if (nv == this.IntValeur) {
                    // TODO : A connecter au modèle
                    // On retire alors l'état fixé
                    // cell.SupprimerValeur();
                    __valeur = 0;

                    // Il faut avertir des propriétés modifiées !
                    OnPropertyChanged("IsSet");
                    // OnPropertyChanged("Opacity");
                    OnPropertyChanged("IsConcerned");
                } // Sinon, on refuse toute modification
            } else {
                // TODO : A connecter au modèle
                // Ici, on applique la valeur
                // cell.ModifierValeur(nv);
                __valeur = nv;
                OnPropertyChanged("IsSet");
                //OnPropertyChanged("Opacity");
                OnPropertyChanged("IsConcerned");
                OnPropertyChanged("Valeur");
            }
        }

        public void FixeValue(int value) {
            // TODO : A connecter au modèle
            if (IsFixed && value == this.IntValeur) {
                // cell.Effacer();
            } else {
                // cell.FixerValeur(value);
            }
            OnPropertyChanged("IsFixed");
            OnPropertyChanged("IsConcerned");
            OnPropertyChanged("Valeur");
        }

        public void ClearAll() {
            // TODO : A connecter au modèle
            // On efface tout
            // cell.Effacer();
            for (int i = 1; i < 10; ++i) {
                possibilités[i].Is = false;
            }
            OnPropertyChanged("IsSet");
            OnPropertyChanged("IsFixed");
            OnPropertyChanged("IsConcerned");
        }

        public void AddPossibility(int valeur) {
            if (!IsSet) {
                this.possibilités[valeur].Is = !this.possibilités[valeur].Is;
                // TODO : A connecter au modèle 
                // bool b = (cell.Contient(valeur) ? cell.SupprimerPossibilité(valeur) : cell.AjouterPossibilité(valeur));
                OnPropertyChanged("IsConcerned");
                //OnPropertyChanged("Opacity");
            } // Sinon, on refuse toute action
        }

        public String Valeur {
            get => IntValeur.ToString();
        }

        public bool HasValue(int v) {
            return possibilités[v].Is;
        }

        public Possibility this[int v] {
            get => possibilités[v];
        }
    }
}
