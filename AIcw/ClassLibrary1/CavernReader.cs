using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CavernReader
    {
        private static CavernReader instance;
        private static List<Cave> caves;
        private static Boolean[][] connections;

        private CavernReader() { }

        public static CavernReader Instance
        {
            get
            {
                if (instance == null)
                    instance = new CavernReader();
                return instance;
            }
        }

        public List<Cave> Caves
        {
            get
            { return caves; }

        }
        public bool ReadData(string file)
        {
            string path = @file;
            int index = 1;
            if (File.Exists(path))
            {
                caves = new List<Cave>();
                StreamReader readFileStream = new StreamReader(path);
                string buffer = readFileStream.ReadLine();
                String[] data = buffer.Split(',');

                int noOfCaves = Int32.Parse(data[0]);

                for (int count = 1; count < ((noOfCaves * 2) + 1); count = count + 2)
                {
                    int x = Int32.Parse(data[count]);
                    int y = Int32.Parse(data[count + 1]);
                    Cave temp = new Cave(index++, x, y);
                    caves.Add(temp);
                }

                Boolean[][] connected = new Boolean[noOfCaves][];
                for (int i = 0; i < noOfCaves; i++)
                {
                    connected[i] = new Boolean[noOfCaves];
                }

                int col = 0;
                int row = 0;

                for (int point = (noOfCaves * 2) + 1; point < data.Length; point++)
                {

                    if (data[point].Equals("1"))
                        connected[row][col] = true;
                    else
                        connected[row][col] = false;

                    row++;
                    if (row == noOfCaves)
                    {
                        row = 0;
                        col++;
                    }
                }
                connections = connected;
                return true;
            }
            else
                return false;

        }

        public bool IsConnected(int from, int dest)
        {
            return connections[from-1][dest-1];
        }
        public Cave GetCave(int number)
        {
            foreach (Cave cv in caves)
            {
                if (cv.Number == number)
                {
                    return cv;
                }
            }
            return null;
        }
        public double Distance(Cave from, Cave dest)
        {
            float xDist = dest.CoordinateX - from.CoordinateX;
            xDist *= xDist;
            float yDist = dest.CoordinateY - from.CoordinateY;
            yDist *= yDist;
            double result = xDist + yDist;
            
            return Math.Sqrt(result);
        }
        public void BuildConnections()
        {
            foreach (Cave caveFrom in caves)
            {
                Dictionary<int, double> neighbours = new Dictionary<int, double>();
                foreach (Cave caveTo in caves)
                {
                    if (this.IsConnected(caveFrom.Number, caveTo.Number))
                    {
                        double distance = this.Distance(caveFrom, caveTo);
                        neighbours.Add(caveTo.Number, distance);
                    }
                }
                caveFrom.Neighbours = neighbours;
            }
          
        }

        public List<int> Path(List<Cave> unpopulated)
        {
            int start = 1;
            int finish = unpopulated.Count;
            Dictionary<int, double> distances = new Dictionary<int, double>();
            List<int> nodes = new List<int>();
            List<int> path = new List<int>(); 
            foreach (Cave cv in unpopulated)
            {
                if(cv.Number == start)
                {
                    distances.Add(cv.Number, 0);
                }
                else
                {
                    distances.Add(cv.Number, int.MaxValue);
                }
               nodes.Add(cv.Number);
            }
            
            while(nodes.Count != 0)
            {
                nodes.Sort((pair1, pair2) => distances[pair1].CompareTo(distances[pair2]));
                int smallest = nodes[0];
                nodes.Remove(smallest);
                if(smallest == finish)
                {
                    Cave node = this.GetCave(smallest);
                    while(node != null)
                    {
                        path.Add(node.Number);
                        node = node.Previous;
                    }
                    break;
                   // path.Add(1);
                }
                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }
                Cave caveSmallest = this.GetCave(smallest);
                foreach(var neighbour in caveSmallest.Neighbours)
                {
                    double alt = distances[smallest] + neighbour.Value;
                    if(alt < distances[neighbour.Key])
                    {
                        distances[neighbour.Key] = alt;
                        this.GetCave(neighbour.Key).Previous = caveSmallest;
                    }
                }
            }

            return path;
        }
       
    }
}
