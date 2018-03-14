using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
   public class Dijkstra
    {
        private Dictionary<int, Dictionary<int, double>> caves = new Dictionary<int, Dictionary<int, double>>();

        public void AddCave(int number, Dictionary<int, double> neighbours)
        {
            caves[number] = neighbours;
        }

        public string Something()
        {
            return caves.Count.ToString();
        }
        public List<int> Path(int start, int finish)
        {
            var previous = new Dictionary<int, int>();
            var distances = new Dictionary<int, double>();
            var nodes = new List<int>();

            List<int> path = new List<int>();

            foreach(var cave in caves)
            {
                if(cave.Key == start)
                {
                    distances[cave.Key] = 0;
                }
                else
                {
                    distances[cave.Key] = int.MaxValue;
                }
                nodes.Add(cave.Key);
            }

            while(nodes.Count != 0)
            {

                nodes.Sort((pair1,pair2) => distances[pair1].CompareTo(distances[pair2]));
                var smallest = nodes[0];
                nodes.Remove(smallest);
                if(smallest == finish)//if no more nodes
                {
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }
                    break;
                }

                if(distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach(var neighbour in caves[smallest])
                {
                    var alt = distances[smallest] + neighbour.Value;
                    if(alt < distances[neighbour.Key])
                    {
                        distances[neighbour.Key] = alt;
                        previous[neighbour.Key] = smallest;
                        
                    }
                }
            }
            return path;
        }
    }
}
