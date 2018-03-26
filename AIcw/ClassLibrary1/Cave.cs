using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /* 
    * Author: Ovidiu - Andrei Radulescu
    * The Cave class
    * Last edited: 26/03/2018
    */
    public class Cave
    {
        private int number;
        // X and Y coordinates
        private int x;
        private int y;
        //stores the node reference to the previous cave in the path to the first cave
        private Cave previous = null;
        private Dictionary<int, double> neighbours = new Dictionary<int, double>();

        public Cave(int nr, int xCoord, int yCoord)
        {
            number = nr;
            x = xCoord;
            y = yCoord;
        }
        public Cave Previous
        {
            get { return previous; }
            set { previous = value; }
        }
        public int Number
        {
            get
            {
                return number;
            }
        }
        public int CoordinateX
        {
            get
            {
                return x;
            }
        }
        public int CoordinateY
        {
            get
            {
                return y;
            }
        }
        public Dictionary<int, double> Neighbours
        {
            get {return neighbours; }
            set { neighbours = value; }
        }

    }
}
