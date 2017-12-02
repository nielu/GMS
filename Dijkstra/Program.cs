using System;
using System.Collections.Generic;

namespace Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            int s = int.Parse(Console.ReadLine());
            for (int testCase = 0; testCase < s; ++testCase)
            {
                var g = Graph.Parse();
                var pathCount = int.Parse(Console.ReadLine());
                var pathsToFind = new string[pathCount];
                for (int i = 0; i < pathCount; ++i)
                    pathsToFind[i] = Console.ReadLine();

                foreach (var p in pathsToFind)
                {
                    var cities = p.Split(' ');
                    Console.WriteLine(HeapDjikstra(g, cities[0], cities[1]));
                }
            }
        }

        static int HeapDjikstra(Graph g, string start, string end)
        {
            var queue = new PriorityQueue();
            var distance = new Dictionary<uint, int>();

            uint startingCity = g.Cities[start];
            uint endingCity = g.Cities[end];

            foreach (var city in g.Cities.Values)
            {
                if (city == startingCity)
                {
                    distance[city] = 0;
                    queue.Enqueue(new Node() { City = city, Distance = 0 });
                }
                else
                {
                    distance[city] = int.MaxValue;
                    queue.Enqueue(new Node() { City = city, Distance = int.MaxValue });
                }
            }

            while (queue.Count() != 0)
            {
                var u = queue.Dequeue();

                foreach (var v in g.Vertices[u.City])
                {
                    long alt = (long)distance[u.City] + (long)v.Value;

                    if (alt < distance[v.Key])
                    {
                        //since alt is smaller than distance[v.key] it has to fit into int
                        distance[v.Key] = (int)alt;
                        queue.Enqueue(new Node() { City = v.Key, Distance = (int)alt });
                    }
                }
            }
            return distance[endingCity];
        }
    }

    public class Graph
    {
        public Dictionary<uint, Dictionary<uint, int>> Vertices = new Dictionary<uint, Dictionary<uint, int>>();
        public Dictionary<string, uint> Cities = new Dictionary<string, uint>();

        public static Graph Parse()
        {
            var g = new Graph();

            var c = Console.ReadLine();
            if (c == string.Empty) // WA for empty lines in test input
                c = Console.ReadLine();
            var cityCount = int.Parse(c);

            for (uint i = 1; i <= cityCount; ++i)
            {
                var cityName = Console.ReadLine();
                g.Cities[cityName] = i;

                var neigbouringCitiesCount = int.Parse(Console.ReadLine());
                var pathsToNeighbours = new Dictionary<uint, int>();

                for (int j = 0; j < neigbouringCitiesCount; ++j)
                {
                    var line = Console.ReadLine().Split(' ');
                    var toCity = uint.Parse(line[0]);
                    var cost = int.Parse(line[1]);

                    pathsToNeighbours[toCity] = cost;
                }

                g.Vertices[i] = pathsToNeighbours;
            }

            return g;
        }
    }

    public class Node : IComparable<Node>
    {
        public uint City;
        public int Distance;

        public int CompareTo(Node other)
        {
            return this.Distance - other.Distance;
        }
    }

    public class PriorityQueue
    {
        private List<Node> data;

        public PriorityQueue()
        {
            this.data = new List<Node>();
        }

        public void Enqueue(Node item)
        {
            data.Add(item);
            int ci = data.Count - 1;
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if (data[ci].CompareTo(data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
                var tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp; //swap
                ci = pi;
            }
        }

        public Node Dequeue()
        {
            int li = data.Count - 1;
            var frontItem = data[0];
            data[0] = data[li];
            data.RemoveAt(li);

            --li;
            int pi = 0;
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && data[rc].CompareTo(data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (data[pi].CompareTo(data[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                var tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp; // swap parent and child
                pi = ci;
            }
            return frontItem;
        }
        public int Count()
        {
            return data.Count;
        }
    }
}
