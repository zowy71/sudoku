using CS_Sudoku.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CS_Sudoku.ViewModel {
    public class VMSudoku : INotifyPropertyChanged {

        #region Implémentation de l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Exposition des méthodes
        public ICommand RemplirPossibilités
        {
            get;
            private set;
        }

        public ICommand DéfinirPlacerOuPossibilités
        {
            get;
            private set;
        }

        public ICommand Effacer
        {
            get;
            private set;
        }

        public ICommand ChargerCSV
        {
            get;
            private set;
        }

        public ICommand SauvegarderCSV
        {
            get;
            private set;
        }

        public ICommand DéfinirLog
        {
            get;
            private set;
        }

        public ICommand SingletonNu
        {
            get;
            private set;
        }

        public ICommand SingletonCaché
        {
            get;
            private set;
        }

        public ICommand InteractionLigneColonneBloc
        {
            get;
            private set;
        }

        public ICommand GroupeCaché
        {
            get;
            private set;
        }

        public ICommand GroupeNu
        {
            get;
            private set;
        }

        public ICommand XWing
        {
            get;
            private set;
        }

        public ICommand SwordFish
        {
            get;
            private set;
        }

        public ICommand BackTracking
        {
            get;
            private set;
        }

        public ICommand ToutesLesRègles
        {
            get;
            private set;
        }
        #endregion

        // TODO : Créer le modèle Sudoku
        private Sudoku grille; // = new Sudoku();
        private VMBlocCell[] _array = new VMBlocCell[9];
        private static readonly Random rnd = new Random();

        private List<IRelayCommand> butDisabledWhenDefining = new List<IRelayCommand>();

        private MainWindow main;

        #region Propriétés attachées aux interfaces graphiques

        private bool _définir = false;
        /// <summary>
        ///  Permet de savoir si on définit le sudoku ou si on le résout...
        /// </summary>
        public bool Définir
        {
            get => _définir;
            set
            {
                if (value != _définir)
                {
                    _définir = value;
                    foreach (IRelayCommand relay in butDisabledWhenDefining)
                    {
                        relay.SetActive(!_définir);
                    }
                    OnPropertyChanged();
                }
            }
        }


        private bool _modePasAPas = true;
        /// <summary>
        /// Propriété définissant le mode Pas à Pas
        /// pour la résolution du sudoku
        /// </summary>
        public bool ModePasAPas
        {
            get => _modePasAPas;
            set
            {
                if (value != _modePasAPas)
                {
                    _modePasAPas = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Permet de savoir si on est en mode Possibilités ou Placer
        /// </summary>
        public bool PlacerChiffre { get; set; }

        private bool _afficherModifications = true;
        /// <summary>
        /// Permet de savoir s'il faut afficher les modifications 
        /// après avoir lancer une règle de résolution
        /// </summary>
        public bool AfficherModifications
        {
            get => _afficherModifications;
            set
            {
                if (value != _afficherModifications)
                {
                    _afficherModifications = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _logFilename = null;
        /// <summary>
        /// Définit le nom du fichier Log
        /// S'il vaut null, aucune information n'est enregistré dans aucun fichier
        /// </summary>
        public string LogFilename
        {
            get => _logFilename;
            set
            {
                if (value != _logFilename)
                {
                    _logFilename = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion


        /// <summary>
        /// Utilisé pour tous les tirages aléatoires
        /// </summary>
        public static Random Rnd => rnd;

        public VMSudoku(MainWindow main) {
            this.main = main;

            for (int i = 0; i < _array.Length; ++i) {
                _array[i] = new VMBlocCell(grille, i);
            }

            /*foreach (VMBlocCell bc in _array) {
                bc.Concerned = 2; 
            }*/

            #region Mise en place des commandes
            this.RemplirPossibilités = new RelayCommand(DoRemplirPossibilités);
            butDisabledWhenDefining.Add(RemplirPossibilités as IRelayCommand);
            //this.DéfinirSudoku = new RelayCommand(NeRienFaire);
            this.DéfinirPlacerOuPossibilités = new RelayCommand(NeRienFaire);
            butDisabledWhenDefining.Add(DéfinirPlacerOuPossibilités as IRelayCommand);
            this.Définir = false;
            this.Effacer = new RelayCommand<bool>(ClearAll);
            this.ChargerCSV = new RelayCommand(DoChargerCSV);
            butDisabledWhenDefining.Add(this.ChargerCSV as IRelayCommand);
            this.SauvegarderCSV = new RelayCommand(DoSauvegarderCSV);
            butDisabledWhenDefining.Add(this.SauvegarderCSV as IRelayCommand);
            ModePasAPas = true;
            this.DéfinirLog = new RelayCommand(DoDéfinirLog);
            this.SingletonNu = new RelayCommand(AppliquerRègleSingletonNu);
            butDisabledWhenDefining.Add(this.SingletonNu as IRelayCommand);
            this.SingletonCaché = new RelayCommand(AppliquerRègleSingletonCaché);
            butDisabledWhenDefining.Add(this.SingletonCaché as IRelayCommand);
            this.InteractionLigneColonneBloc = new RelayCommand(AppliquerRègleInteractionLigneColonneBloc);
            butDisabledWhenDefining.Add(this.InteractionLigneColonneBloc as IRelayCommand);
            this.GroupeCaché = new RelayCommand(AppliquerRègleGroupeCaché);
            butDisabledWhenDefining.Add(this.GroupeCaché as IRelayCommand);
            this.GroupeNu = new RelayCommand(AppliquerRègleGroupeNu);
            butDisabledWhenDefining.Add(this.GroupeNu as IRelayCommand);
            this.XWing = new RelayCommand(AppliquerRègleXWing);
            butDisabledWhenDefining.Add(this.XWing as IRelayCommand);
            this.SwordFish = new RelayCommand(AppliquerRègleSwordFish);
            butDisabledWhenDefining.Add(this.SwordFish as IRelayCommand);
            this.BackTracking = new RelayCommand(AppliquerBackTracking);
            butDisabledWhenDefining.Add(this.BackTracking as IRelayCommand);
            this.ToutesLesRègles = new RelayCommand(Résoudre);
            butDisabledWhenDefining.Add(this.ToutesLesRègles as IRelayCommand);
            #endregion
        }

        public int Concerned {
            set {
                foreach (VMBlocCell bc in _array) {
                    bc.Concerned = value;
                }
            }
        }

        public VMBlocCell this[int i] {
            get { return _array[i]; }
        }

        public void ClearAll(bool all = true) {
            if (!main.ConfirmMessage(
               all ? "Êtes-vous sûr de vouloir tout effacer ?"
               : "Êtes-vous sûr de vouloir recommencer ?")) return;
            // On efface le contenu de toutes les cellules
            foreach (VMBlocCell bc in _array) {
                bc.ClearAll(all);
            }
        }

        public void NeRienFaire() { }

        public void Refresh() {
            foreach (VMBlocCell vmB in _array) {
                vmB.Refresh();
            }
        }

        /// <summary>
        /// Chargement d'un fichierv au format CSV
        /// </summary>
        public void DoChargerCSV()
        {
            string filename = main.GetLoadFileName("CSV files (*.csv)|*.csv|Json Files (*.json)|*.json|All files (*.*)|*.*");
            if (filename != null)
            {
                try
                {
                    bool res = grille.ChargerCSV(filename, out string rapport);
                    Refresh();
                    if (!res)
                        main.InformMessage(rapport, "Problèmes lors du chargement");
                }
                catch (Exception e) when (e is NotImplementedException || e is NullReferenceException)
                {
                    main.InformMessage("Non implémenté");
                }
            }
        }

        public void DoSauvegarderCSV()
        {
            string filename = main.GetSaveFileName("CSV files (*.csv)|*.csv|Json Files (*.json)|*.json|All files (*.*)|*.*");
            if (filename != null)
            {
                try
                {
                    grille.SauvegarderCSV(filename);
                }
                catch (Exception e) when (e is NotImplementedException || e is NullReferenceException)
                {
                    main.InformMessage("Non implémenté...");
                }
            }
        }

        public void DoRemplirPossibilités() {
            if (main.ConfirmMessage("Voulez-vous remplir TOUTES les possibilités ?"))
            {
                try
                {
                    grille.RemplirPossibilités();
                    Refresh();
                }
                catch (Exception e) when (e is NotImplementedException || e is NullReferenceException)
                {
                    main.InformMessage("Routine non implémentée...");
                }
            }
        }

        public void AppliquerRègleSingletonCaché()
        {
            //SingletonCaché sc = new SingletonCaché(LogFilename, ModePasAPas);
            AppliquerRègle(null /*sc*/);
        }

        public void DoDéfinirLog()
        {
            main.DéfinirLog();
        }

        public void AppliquerRègleSingletonNu()
        {
//            SingletonNu sn = new SingletonNu(LogFilename, ModePasAPas);
            AppliquerRègle(null);
        }

        public void AppliquerRègleInteractionLigneColonneBloc()
        {
 //           InteractionLigneColonneBloc ilcb = new InteractionLigneColonneBloc(LogFilename, ModePasAPas);
            AppliquerRègle(null);
        }

        public void AppliquerRègleGroupeCaché()
        {
//            GroupeCaché gc = new GroupeCaché(LogFilename, ModePasAPas);
            AppliquerRègle(null);
        }

        public void AppliquerRègleGroupeNu()
        {
 //           GroupeNu gn = new GroupeNu(LogFilename, ModePasAPas);
            AppliquerRègle(null);
        }

        public void AppliquerRègleXWing()
        {
            AppliquerRègle(null);
        }

        public void AppliquerRègleSwordFish()
        {
            AppliquerRègle(null);
        }

        public void AppliquerBackTracking()
        {
//            BackTracking sn = new BackTracking(LogFilename, ModePasAPas);
            AppliquerRègle(null);
        }

        public void Résoudre()
        {
            AppliquerRègle(null);
        }

        public void AppliquerRègle(RègleSudoku règle)
        {
            if (règle is null)
            {
                main.InformMessage("En cours d'écriture...");
                return;
            }
            bool ret = règle.Appliquer(grille, out string res);
            if (ret)
            {
                Refresh();
                if (AfficherModifications)
                    main.AfficherModifications(res);
            }
            else
            {
                main.InformMessage("Aucune modification");
            }
        }

    }
}
