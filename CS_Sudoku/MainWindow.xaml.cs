using CS_Sudoku.View;
using CS_Sudoku.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS_Sudoku {
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private VMSudoku sudoku = new VMSudoku();
        private bool valueToFix = false;

        /// <summary>
        /// Nom du fichier de Log pour enregistrer les modifications
        /// apportées par les différentes règles
        /// </summary>
        private string logFilename = null;

        private int actualDigit;
        public MainWindow() {
            InitializeComponent();

            DataContext = sudoku;

        }
        private void ContentControl_MouseDown(object sender, MouseButtonEventArgs e) {
            Console.WriteLine("Click sur " + sender.GetType().ToString());
            if (sender is ContentControl cc) {
                Console.WriteLine("\tDataContext : " + cc.DataContext);
                if (cc.DataContext is VMCell cell) {
                    if (actualDigit > 0) {
                        if (valueToFix) {
                            Console.WriteLine("-- Fixing value ---");
                            cell.FixeValue(actualDigit);
                        } else if (ToggleButtonEdit.IsChecked == true) {
                            Console.WriteLine("-- Adding possibility...");
                            cell.AddPossibility(actualDigit);
                        } else {
                            Console.WriteLine("-- Setting value...");
                            cell.SetValue(actualDigit);
                        }
                    }
                    Console.WriteLine("--  IsSet       : " + cell.IsSet);
                    Console.WriteLine("--  IsFixed     : " + cell.IsFixed);
                    Console.WriteLine("--  IsConcerned : " + cell.IsConcerned);
                    Console.WriteLine("--  Actual digit: " + actualDigit);
                    Console.Write(" -- Possibilités: " + (cell[1].Is ? "1" : " "));
                    for (int i = 2; i < 10; i++) {
                        Console.Write("," + (cell[i].Is ? i.ToString() : " "));
                    }
                    Console.WriteLine();
                }
            }
        }

        private void RBDigits_MouseDown(object sender, MouseButtonEventArgs e) {
            Console.WriteLine(sender);
            if (sender is Label lb) {
                int.TryParse(lb.Content.ToString(), out int num);
                Console.WriteLine(lb.Content + " - " + num);
                sudoku.Concerned = num;
                actualDigit = num;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show(this, "Etes-vous sûr de vouloir tout effacer ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                sudoku.ClearAll();
                ToggleButtonEdit.IsChecked = false;
                ToggleButtonEdit.Visibility = Visibility.Hidden;
                ButtonValidateAll.Visibility = Visibility.Visible;
                valueToFix = true;
            }
        }

        private void ButtonValidateAll_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show(this, "Avez-vous fini de définir le nouveau Sudoku ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                ButtonValidateAll.Visibility = Visibility.Hidden;
                valueToFix = false;
                ToggleButtonEdit.Visibility = Visibility.Visible;
            }
        }

        private void ButtonApplyRules_Click(object sender, RoutedEventArgs e) {
            Console.WriteLine("-- Apply Rule IsChecked : " + ButtonApplyRules.IsChecked);
            bool isChecked = ButtonApplyRules.IsChecked == true;
            MenuRules.IsEnabled = isChecked;
            ButtonUndo.IsEnabled = !isChecked;
            ToggleButtonEdit.IsEnabled = !isChecked;
            ButtonClear.IsEnabled = !isChecked;

        }

        public delegate bool AppliquerUneRègle(string file, bool pasApas, out string res);

        private void MenuRule1_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerRègleSingletonNu);
        }

        private void MenuRule2_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerRègleSingletonCaché);
        }

        private void MenuRule3_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerRègleInteractionLigneColonneBloc);
        }

        private void MenuRule4_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerRègleGroupeNu);
        }

        private void MenuRule5_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerRègleGroupeCaché);
        }

        private void MenuX_Wing_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerRègleXWing);
        }

        private void MenuSwordfish_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerRègleSwordFish);
        }

        private void MenuBackTracking_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.AppliquerBackTracking);
        }

        private void GèreApplicationRègle(AppliquerUneRègle règle) {
            if (AfficherResultat.IsChecked == true) {
                AfficheModifications aff = new AfficheModifications();
                if (règle(logFilename, ModePasAPas.IsChecked == true, out string res)) {
                    aff.Contenu = res;
                } else {
                    aff.Contenu = "Aucune modification apportée.";
                }
                aff.ShowDialog();
                if (!aff.AfficherModifications) {
                    AfficherResultat.IsChecked = false;
                }
            }
        }

        private void MenuAllRules_Click(object sender, RoutedEventArgs e) {
            GèreApplicationRègle(sudoku.Résoudre);
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            string fileName = null;
            // ofd.InitialDirectory = "c:\\";
            ofd.Filter = "CSV files (*.csv)|*.csv|Json Files (*.json)|*.json|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true) {
                fileName = ofd.FileName;
            }
            if (fileName != null) {
                //Do something with the file, for example read text from it
                // string text = File.ReadAllText(fileName);
                if (!sudoku.LoadCSV(fileName, out string rapport)) {
                    System.Windows.MessageBox.Show(this, rapport, "Problèmes lors du chargement");
                }
            }
        }

        private void RemplirPossibilites_Click(object sender, RoutedEventArgs e) {
            sudoku.RemplirPossibilités();
        }

        private void MenuDefinirLog_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            // ofd.InitialDirectory = "c:\\";
            ofd.Filter = "Log files (*.log)|*.log|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true) {
                logFilename = ofd.FileName;
                tbLog.Text = logFilename;
            }

        }

        private void MenuSupprimerLog_Click(object sender, RoutedEventArgs e) {
            logFilename = null;
            tbLog.Text = "Aucun";
        }
    }
}
