using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Cave
    {
        private int number;
        private int x;
        private int y;
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
