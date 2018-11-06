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
        public AfficheModifications() {
            InitializeComponent();
        }

        public bool AfficherModifications {
            get { return cbAfficherModifications.IsChecked == false; }
        }
        public string Contenu {
            set {
                tbContenu.Text = value;
            }
        }
    }
}
