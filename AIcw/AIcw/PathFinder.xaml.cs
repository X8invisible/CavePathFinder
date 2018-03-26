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
    /* 
     * Author: Ovidiu - Andrei Radulescu
     * The Path Finder window, contains drawing of lines, circles and paths functions
     * Last edited: 26/03/2018
     */
    public partial class MainWindow : Window
    {
        private CavernReader instance = CavernReader.Instance;
        private StepThrough stepThrough = new StepThrough();
        private List<Cave> unpop;
        //needed for scaling the canvas
        private int maxX = 0;
        private int maxY = 0;
        //method for drawing a circle
        private void DrawCirle(int x, int y, int nr)
        {
            TextBlock TB = new TextBlock();
            TB.Width = 0.6;
            TB.Height = 0.6;
            TB.FontSize = 0.4;
            TB.Text = nr.ToString();
            TB.Background = Brushes.White;
            // TB.Name = "TextB";
            canvas.Children.Add(TB);
            Canvas.SetLeft(TB, x);
            Canvas.SetTop(TB, y+0.5);

            double dotSize = 0.4;
            Ellipse currentDot = new Ellipse();
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = dotSize;
            currentDot.Width = dotSize;
            currentDot.Fill = new SolidColorBrush(Colors.CadetBlue);
            currentDot.Margin = new Thickness(x, y, x+0.5, y+0.5); // Sets the position.
            canvas.Children.Add(currentDot);
        }
        //method for drawing a line
        private void DrawLine(int x1, int y1, int x2, int y2, Brush colour)
        {
            Line line = new Line();
            line.Stroke = colour;

            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;

            line.StrokeThickness = 0.05;
            canvas.Children.Add(line);
        }

        //method for drawing a path(multiple lines)
        private void DrawPath(List<int> path, Brush colour)
        {
            double distance = 0;
            path.Reverse();
            string pathS = "";
            for (int i = 0; i < path.Count; i++)
            {
                pathS += (path[i] + " ");
                Cave cv1 = instance.GetCave(path[i]);
                if (path.Count > i + 1)
                {
                    Cave cv2 = instance.GetCave(path[i + 1]);
                    distance += instance.Distance(cv1, cv2);
                    DrawLine(cv1.CoordinateX, maxY - cv1.CoordinateY, cv2.CoordinateX, maxY - cv2.CoordinateY, colour);
                }
            }
            lblPath.Content = pathS;
            lblDist.Content = "Distance is: " + distance.ToString();
        }
        public MainWindow()
        {
            InitializeComponent();
            unpop = new List<Cave>(instance.Caves);
            dGridDist.ItemsSource = stepThrough.Distances;
            //gets the max X coordinate and Y coordinate from the map, in order to scale the canvas
            foreach (Cave cv in instance.Caves)
            {
                
                if (cv.CoordinateX > maxX)
                    maxX = cv.CoordinateX;
                if (cv.CoordinateY > maxY)
                    maxY = cv.CoordinateY;
            }
            //adds each cave as a point
            foreach(Cave cv in instance.Caves)
            {
                DrawCirle(cv.CoordinateX, maxY-cv.CoordinateY, cv.Number);
            }
            canvas.LayoutTransform = new ScaleTransform(520/maxX,318/maxY);
           

        }

        //shortest path through the caves(no stepping)
        private void BtnConnection_Click(object sender, RoutedEventArgs e)
        {
            unpop = new List<Cave>(instance.Caves);
            List<int> path = instance.Path(unpop);
            DrawPath(path, Brushes.RoyalBlue);
        }

        //previous window
        private void btnArrowBack_Click(object sender, RoutedEventArgs e)
        {
            FileSelectWindow back = new FileSelectWindow();
            back.Show();
            this.Close();
        }

        //clear canvas
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            stepThrough = new StepThrough();
            instance.Reset();
            dGridDist.ItemsSource = stepThrough.Distances;
            canvas.Children.Clear();
            lblDist.Content = "";
            lblPath.Content = "";
            foreach (Cave cv in instance.Caves)
            {
                DrawCirle(cv.CoordinateX, maxY - cv.CoordinateY, cv.Number);
            }
        }
        //step through pathing
        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            List<int> path = stepThrough.Step(unpop);
            if(path.Count > 0)
            {
                //this is a flag for letting the program know it reached the final node(and therefore it needs to draw a path with different colours)
                if(path.Last<int>() == 8080)
                {
                    path.Remove(8080);
                    DrawPath(path,Brushes.Blue);
                }
                else
                {
                    DrawPath(path, Brushes.Gold);
                }  
            }
            //refreshes the distances table with updated values after a step through
            dGridDist.Items.Refresh();
            
        }
    }
}
