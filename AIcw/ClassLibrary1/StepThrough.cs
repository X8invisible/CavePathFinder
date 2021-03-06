﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /* 
    * Author: Ovidiu - Andrei Radulescu
    * The Step Through class
    * Last edited: 26/03/2018
    */

    //does the same thing as the Path() function from cavern reader, but it's a class because I need a lot of the variables returned to the program
    //Also needed is remembering the node positions and distances for stepping through, a function would lose that and do it from the beggining
    public class StepThrough
    {
        
        private Dictionary<int, double> distances = new Dictionary<int, double>();
        private List<int> nodes = new List<int>();
        private CavernReader instance = CavernReader.Instance;
        private int finish =0;
        private bool done = false;

        public StepThrough() { }

        
        public Dictionary<int,double> Distances
        {
            get { return distances; }
        }

        //does the same thing as Path(), but the function ends after one loop through a cave's neighbours
        public List<int> Step(List<Cave> unpopulated)
        {
            int start = 1;
            if(finish == 0)
                finish = unpopulated.Count;
            List<int> path = new List<int>();
            if(distances.Count == 0)
            {
                foreach (Cave cv in unpopulated)
                {
                    if (cv.Number == start)
                    {
                        distances.Add(cv.Number, 0);
                    }
                    else
                    {
                        distances.Add(cv.Number, int.MaxValue);
                    }
                    nodes.Add(cv.Number);
                }
            }
        

            while (nodes.Count != 0)
            {
                nodes.Sort((pair1, pair2) => distances[pair1].CompareTo(distances[pair2]));
                int smallest = nodes[0];
                nodes.Remove(smallest);
                if (smallest == finish)
                {
                    done = true;
                    Cave node = instance.GetCave(smallest);
                    while (node != null)
                    {
                        path.Add(node.Number);
                        node = node.Previous;
                    }
                    //flag for the program to know the path has been found
                    path.Add(8080);
                    break;
                }
                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }
                Cave caveSmallest = instance.GetCave(smallest);
                foreach (var neighbour in caveSmallest.Neighbours)
                {
                    double alt = distances[smallest] + neighbour.Value;
                    if (alt < distances[neighbour.Key])
                    {
                        distances[neighbour.Key] = alt;
                        instance.GetCave(neighbour.Key).Previous = caveSmallest;
                    }
                }
                break;
            }

            if (!done)
            {
                Cave nodey = instance.GetCave(nodes[0]);
                while (nodey != null)
                {
                    path.Add(nodey.Number);
                    nodey = nodey.Previous;
                }
            }
            
            return path;
        }

    }
}
