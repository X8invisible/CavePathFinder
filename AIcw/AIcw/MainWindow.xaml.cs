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

        private void DrawCirle(int x, int y)
        {
            double dotSize = 1;
            Ellipse currentDot = new Ellipse();
            currentDot.Stroke = new SolidColorBrush(Colors.ForestGreen);
            currentDot.StrokeThickness = 0.2;
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = dotSize;
            currentDot.Width = dotSize;
            currentDot.Fill = new SolidColorBrush(Colors.Green);
            currentDot.Margin = new Thickness(x, y, 0, 0); // Sets the position.
            canvas.Children.Add(currentDot);
        }
        public MainWindow()
        {
            InitializeComponent();
            foreach(Cave cv in instance.Caves)
            {
                DrawCirle(cv.CoordinateX, cv.CoordinateY);
          
            }

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
