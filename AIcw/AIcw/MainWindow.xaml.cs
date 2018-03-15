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

        private void DrawCirle(int x, int y, int nr)
        {
            TextBlock TB = new TextBlock();
            TB.Width = 1;
            TB.Height = 1;
            TB.FontSize = 0.5;
            TB.Text = nr.ToString();
            TB.Background = Brushes.White;
            // TB.Name = "TextB";
            canvas.Children.Add(TB);
            Canvas.SetLeft(TB, x);
            Canvas.SetTop(TB, y+1);

            double dotSize = 1;
            Ellipse currentDot = new Ellipse();
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = dotSize;
            currentDot.Width = dotSize;
            currentDot.Fill = new SolidColorBrush(Colors.CadetBlue);
            currentDot.Margin = new Thickness(x, y, 0, 0); // Sets the position.
            canvas.Children.Add(currentDot);
        }
        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            Line line = new Line();
            line.Stroke = Brushes.Blue;

            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;

            line.StrokeThickness = 0.5;
            canvas.Children.Add(line);
        }
        public MainWindow()
        {
            InitializeComponent();
            foreach(Cave cv in instance.Caves)
            {
                DrawCirle(cv.CoordinateX, cv.CoordinateY, cv.Number);
          
            }

        }

        private void BtnConnection_Click(object sender, RoutedEventArgs e)
        {
            List<Cave> unpop = new List<Cave>(instance.Caves);
            List<int> path = instance.Path(unpop);
            path.Reverse();
            string pathS = "";
            for(int i =0; i<path.Count; i++)
            {
                pathS += (path[i] + " ");
                Cave cv1 = instance.GetCave(path[i]);
                if(path.Count > i + 1)
                {
                    Cave cv2 = instance.GetCave(path[i + 1]);
                    DrawLine(cv1.CoordinateX, cv1.CoordinateY, cv2.CoordinateX, cv2.CoordinateY);
                }
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
