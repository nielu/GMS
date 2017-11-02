using System;
using System.Collections.Generic;

namespace BellmanFord
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new App();
            app.Run();

        }
    }


    class App
    {
        public void Run()
        {
            var graphs = readInput();

        }

        private List<Graph> readInput()
        {
            var list = new List<Graph>();
            var n = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < n; ++i)
                list.Add(Graph.ReadFromStdin());
            return list;
        }



    }

    struct Graph
    {
        int size;
        int[,] weights;

        public static Graph ReadFromStdin()
        {
            var g = new Graph();

            g.size = Int32.Parse(Console.ReadLine());
            g.weights = new int[g.size, g.size];

            for (int i = 0; i < g.size; ++i)
            {
                var weights = Console.ReadLine().Split(' ');
                for (int j = 0; j < g.size; ++j)
                    g.weights[i, j] = Int32.Parse(weights[j]);
            }
            return g;
        }
    }
}
