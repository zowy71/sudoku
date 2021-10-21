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
using System.Windows.Shapes;

namespace CS_Sudoku.View
{
    /// <summary>
    /// Logique d'interaction pour DéfinirLog.xaml
    /// L'option MVVM est mal implémentée ici.
    /// Ne peut-on pas connecter directement le nom du fichier ???
    /// </summary>
    public partial class DéfinirLog : Window
    {
        private VMSudoku _sudoku;
        private MainWindow _wnd;
        public DéfinirLog(MainWindow wnd, VMSudoku sudoku)
        {
            _sudoku = sudoku;
            _wnd = wnd;
            InitializeComponent();

            this.DataContext = sudoku;
            cbUse.IsChecked = sudoku.LogFilename != null;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (!(bool)cbUse.IsChecked) _sudoku.LogFilename = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _sudoku.LogFilename = _wnd.GetSaveFileName("Log files (*.log)|*.log|Text files (*.txt)|*.txt|All files (*.*)|*.*");
            cbUse.IsChecked = _sudoku.LogFilename != null;
        }
    }
}
