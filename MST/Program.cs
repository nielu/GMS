using System;
using System.Collections.Generic;

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
                Console.WriteLine(kruskalAlgo(g));
            }
        }

        static int kruskalAlgo(graph g)
        {
            int sum = 0;
            var sortedVertices = new List<vert>(g.Vertices);
            sortedVertices.Sort((v1, v2) => v1.weight.CompareTo(v2.weight));

            var unionFindSet = new unionFindNode[g.Vertices.Count];
            for (int i = 0; i < g.Vertices.Count; i++)
                unionFindSet[i] = new unionFindNode();


            for (int i =0; i < g.Vertices.Count; i++)
            {
                var vert = sortedVertices[0];
                sortedVertices.RemoveAt(0);

                if (!unionFindSet[vert.v].IsUnionedWith(unionFindSet[vert.u]))
                {
                    sum += vert.weight;
                    unionFindSet[vert.v].Union(unionFindSet[vert.u]);
                }
            }

            return sum;
        }
    }

    class graph
    {
        public graph(string desc, string vert)
        {
            var t = desc.Split(',');
            var n = int.Parse(t[0].Substring(2));
            var m = int.Parse(t[1].Substring(2));

            var vertSplit = vert.Replace("{", "").Replace("}", ",").Split(' ');
            this.Vertices = new List<vert>();

            for (int i = 0; i < m; ++i)
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
    }

    struct vert
    {
        public int u, v, weight;
    }
    
    public class unionFindNode
    {
        private unionFindNode _parent;
        private uint _rank;
        
        public unionFindNode()
        {
            _parent = this;
        }

        public unionFindNode Find()
        {
            if (!ReferenceEquals(_parent, this)) _parent = _parent.Find();
            return _parent;
        }

        public bool IsUnionedWith(unionFindNode other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return ReferenceEquals(Find(), other.Find());
        }

        public bool Union(unionFindNode other)
        {
            if (other == null) throw new ArgumentNullException("other");
            var root1 = this.Find();
            var root2 = other.Find();
            if (ReferenceEquals(root1, root2)) return false;

            if (root1._rank < root2._rank)
            {
                root1._parent = root2;
            }
            else if (root1._rank > root2._rank)
            {
                root2._parent = root1;
            }
            else
            {
                root2._parent = root1;
                root1._rank++;
            }
            return true;
        }
    }
}
