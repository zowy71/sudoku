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

namespace CS_Sudoku.View {
    /// <summary>
    /// Logique d'interaction pour AfficheModifications.xaml
    /// </summary>
    public partial class AfficheModifications : Window {
        public AfficheModifications(VMSudoku sudoku, string content)
        {
            InitializeComponent();
            this.DataContext = sudoku;
            tbContenu.Text = content;
        }
    }
}
