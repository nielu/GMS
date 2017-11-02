using System;
using System.Collections.Generic;
using System.Linq;

namespace MST
{
    class Program
    {
        static void Main(string[] args)
        {
            var testCases = int.Parse(Console.ReadLine());

            for (int n = 0; n < testCases; n++)
            {
                var g = new graph(Console.ReadLine(), Console.ReadLine());
                //Console.WriteLine(string.Join(" ", FleuryAlgo(g)));
            }
        }
    }

    class graph
    {
        public graph(string desc, string vert)
        {
            var t = desc.Split(',');
            this.n = int.Parse(t[0].Substring(2));
            this.m = int.Parse(t[1].Substring(2));

            var vertSplit = vert.Replace("{", "").Replace("}", ",").Split(' ');
            this.Vertices = new List<vert>();

            for (int i = 0; i < this.m; ++i)
            {
                var tempArr = vertSplit[i].Split(',');
                var temp = new vert
                {
                    u = int.Parse(tempArr[0]),
                    v = int.Parse(tempArr[1]),
                    weight = int.Parse(tempArr[2])
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
        public int u, v, weight;
    }
}
