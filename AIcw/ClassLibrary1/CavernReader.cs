using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /* 
    * Author: Ovidiu - Andrei Radulescu
    * The Cavern Reader class, contains the input file reader as well as the Dijkstra's algorithm for finding a path directly(no step through)
    * Last edited: 26/03/2018
    */
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
        //used for the clear canvas function
        public void Reset()
        {
            foreach(Cave cv in caves)
            {
                cv.Previous = null;
            }
        }
        //code supplied by the module for reading the .cav files(updated for c#)
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
        //adds the list of neighbour caves to a cave
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

        //dijkstra's algorithm for finding shortest path(no step through)
        public List<int> Path(List<Cave> unpopulated)
        {
            //always starts at 1
            int start = 1;
            //last cave number
            int finish = unpopulated.Count;
            //list of cave number and the distance to cave 1
            Dictionary<int, double> distances = new Dictionary<int, double>();
            //the list of unvisited caves
            List<int> nodes = new List<int>();
            //path to return
            List<int> path = new List<int>(); 
            //adds the distances
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
            //loop as long as there are unvisited caves
            while(nodes.Count != 0)
            {
                //sorts the unvisited list by the shortest distance to cave 1
                nodes.Sort((pair1, pair2) => distances[pair1].CompareTo(distances[pair2]));
                //gets the closest cave and removes it from unvisited
                int smallest = nodes[0];
                nodes.Remove(smallest);
                //if it's the last cave, return the path
                if(smallest == finish)
                {

                    Cave node = this.GetCave(smallest);
                    while(node != null)
                    {
                        path.Add(node.Number);
                        node = node.Previous;
                    }
                    break;
                }
                //if the unvisited list returns a cave with a max int value, it means there is no solution(cave isn't connected so no valid path)
                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }
                Cave caveSmallest = this.GetCave(smallest);
                //looks through the caves neighbours and calculates distances 
                foreach(var neighbour in caveSmallest.Neighbours)
                {
                    //potential new distance from neighbour to cave 1
                    double alt = distances[smallest] + neighbour.Value;
                    //if the new distance is shorter, update the distance and the cave's previous link
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
