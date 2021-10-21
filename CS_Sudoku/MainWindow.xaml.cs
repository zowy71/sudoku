using CS_Sudoku.View;
using CS_Sudoku.ViewModel;
using Microsoft.Win32;
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
        private VMSudoku sudoku;

        private int actualDigit;
        public MainWindow()
        {
            InitializeComponent();

            this.sudoku = new VMSudoku(this);
            DataContext = sudoku;
        }

        private void ContentControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Click sur " + sender.GetType().ToString());
            if (sender is ContentControl cc)
            {
                Console.WriteLine("\tDataContext : " + cc.DataContext);
                if (cc.DataContext is VMCell cell)
                {
                    if (actualDigit > 0)
                    {
                        //if (valueToFix) {
                        if (sudoku.Définir)
                        {
                            Console.WriteLine("-- Fixing value ---");
                            cell.FixeValue(actualDigit);
                        }
                        //else if (ToggleButtonEdit.IsChecked == true) {
                        else if (sudoku.PlacerChiffre)
                        {
                            Console.WriteLine("-- Setting value...");
                            cell.SetValue(actualDigit);
                        }
                        else
                        {
                            Console.WriteLine("-- Adding possibility...");
                            cell.AddPossibility(actualDigit);
                        }
                    }
                    Console.WriteLine("--  IsSet       : " + cell.IsSet);
                    Console.WriteLine("--  IsFixed     : " + cell.IsFixed);
                    Console.WriteLine("--  IsConcerned : " + cell.IsConcerned);
                    Console.WriteLine("--  Actual digit: " + actualDigit);
                    Console.Write(" -- Possibilités: " + (cell[1].Is ? "1" : " "));
                    for (int i = 2; i < 10; i++)
                    {
                        Console.Write("," + (cell[i].Is ? i.ToString() : " "));
                    }
                    Console.WriteLine();
                }
            }
        }

        private void RBDigits_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(sender);
            if (sender is Label lb)
            {
                int.TryParse(lb.Content.ToString(), out int num);
                Console.WriteLine(lb.Content + " - " + num);
                sudoku.Concerned = num;
                actualDigit = num;
            }
        }


        public void AfficherModifications(string modifications)
        {
            AfficheModifications aff = new AfficheModifications(sudoku, modifications);
            aff.ShowDialog();
        }

        public string GetLoadFileName(string pattern)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string fileName = null;
            // ofd.InitialDirectory = "c:\\";
            ofd.Filter = pattern;
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if ((bool)ofd.ShowDialog())
            {
                fileName = ofd.FileName;
            }
            return fileName;
        }

        public string GetSaveFileName(string pattern)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            string fileName = null;
            // ofd.InitialDirectory = "c:\\";
            ofd.Filter = pattern;
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if ((bool)ofd.ShowDialog())
            {
                fileName = ofd.FileName;
            }
            return fileName;
        }


        public bool ConfirmMessage(string message, string title = null)
        {
            string t = title ?? "Confirmation";
            return MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes;
        }

        public void InformMessage(string message, string title = null)
        {
            string t = title ?? "Information";
            MessageBox.Show(message, t, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DéfinirLog()
        {
            new DéfinirLog(this, sudoku).ShowDialog();
        }
    }
}
