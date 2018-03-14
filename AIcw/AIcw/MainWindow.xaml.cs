using Business;
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

namespace AIcw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CavernReader instance = CavernReader.Instance;
        public MainWindow()
        {
            InitializeComponent();
            dgridCave.ItemsSource = instance.Caves;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Cave> unpop = new List<Cave>(instance.Caves);
            List<int> path = instance.Path(unpop);
            path.Reverse();
            string pathS = "";
            foreach(int nr in path)
            {
                pathS += (nr + " ");
            }
            lblBool.Content = pathS;
        }

        private void btnArrowBack_Click(object sender, RoutedEventArgs e)
        {
            FileSelectWindow back = new FileSelectWindow();
            back.Show();
            this.Close();
        }
    }
}
