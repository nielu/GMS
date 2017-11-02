using System;
using System.Collections.Generic;
using System.Linq;

namespace EulerCycles
{
    class Program
    {
        static void Main(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());

            for (int n = 0; n < testCases; n++)
            {
                var g = new graph(Console.ReadLine(), Console.ReadLine());
                Console.WriteLine(string.Join(" ", FleuryAlgo(g)));
            }
        }

        static List<int> FleuryAlgo(graph g)
        {
            var cycle = new List<int>();
            var stack = new Stack<int>();

            int u;
            do
            {
                if (stack.Count == 0)
                    u = g.Vertices[0].u;
                else
                    u = stack.Pop();

                cycle.Add(u);

                var path = g.Vertices.FirstOrDefault(vert => vert.u == u || vert.v == u);

                while (path != null)
                {
                    stack.Push(u);

                    if (!g.Vertices.Remove(path))
                        Console.WriteLine("WTF");

                    if (u == path.u)
                        u = path.v;
                    else
                        u = path.u;
                    path = g.Vertices.FirstOrDefault(vert => vert.u == u || vert.v == u);
                }

            } while (stack.Count != 0);

            return cycle;
        }

    }

    class graph
    {
        public graph(string desc, string vert)
        {
            var t = desc.Split(',');
            this.n = int.Parse(t[0].Substring(2));
            this.m = int.Parse(t[1].Substring(2));

            var vertSplit = vert.Replace("{", "").Replace("}", "").Split(' ');
            this.Vertices = new List<vert>();

            for (int i = 0; i < this.m; ++i)
            {

                var temp = new vert
                {
                    u = int.Parse(vertSplit[i].Substring(0, vertSplit[i].IndexOf(','))),
                    v = int.Parse(vertSplit[i].Substring(vertSplit[i].IndexOf(',') + 1))
                };
                this.Vertices.Add(temp);
            }
        }

        public List<vert> Vertices;
        public int n;
        public int m;
    }

    class vert
    {
        public int u, v;
    }
}
