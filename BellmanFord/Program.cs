using System;

namespace BellmanFord
{
    class Program
    {
        static void Main(string[] args)
        {
            var tests = int.Parse(Console.ReadLine());
            for (int i = 0; i < tests; ++i)
            {
                var g = Graph.Read();
                g.BellmanFord();
                Console.WriteLine("");
            }
            //Console.ReadLine();
        }
    }

    class Graph
    {
        int size;
        int[,] vertices;


        public static Graph Read()
        {
            var g = new Graph();
            g.size = int.Parse(Console.ReadLine());
            g.vertices = new int[g.size, g.size];

            for (int i = 0; i < g.size; ++i)
            {
                var line = Console.ReadLine().Split(' ');
                for (int j = 0; j < g.size; ++j)
                {
                    g.vertices[i, j] = int.Parse(line[j]);
                }
            }

            return g;
        }

        public void BellmanFord()
        {
            var d = new int[size];
            var i = 0;
            d[0] = 0;

            for (i = 1; i < size; ++i)
                d[i] = Int32.MaxValue;

            for (i = 1; i < size; ++i)
            {
                for (int u = 0; u < size; ++u)
                {
                    for (int v = 0; v < size; ++v)
                    {
                        if (vertices[u, v] == 0 || d[u] == Int32.MaxValue)
                            continue;


                        if (d[u] + vertices[u, v] < d[v])
                            d[v] = d[u] + vertices[u, v];
                    }
                }

                for (int j = 0; j < size; ++j)
                {
                    if (d[j] == Int32.MaxValue)
                        Console.Write("0 ");
                    else
                        Console.Write($"{d[j]} ");
                }
                Console.WriteLine("");

            }
        }
    }
}
