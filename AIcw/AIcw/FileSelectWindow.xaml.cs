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
using System.Windows.Shapes;

namespace AIcw
{
    /// <summary>
    /// Interaction logic for FileSelect.xaml
    /// </summary>
    public partial class FileSelectWindow : Window
    {
        private CavernReader instance = CavernReader.Instance;
        public FileSelectWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "CAV Files (*.cav)|*.cav|All files (*.*)|*.*" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            tboxFile.Text = ofd.FileName;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!instance.ReadData(tboxFile.Text))
                MessageBox.Show("No such file exists");
            else
            {
                instance.BuildConnections();
                MainWindow nextWindow = new MainWindow();
                nextWindow.Show();
                this.Close();
            }
            
        }
    }
}
