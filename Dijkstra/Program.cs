using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            int s = int.Parse(Console.ReadLine());
            for (int testCase = 0; testCase < s; ++testCase)
            {
                int n = int.Parse(Console.ReadLine());

            }
        }
    }

    class Heap : FiboHeap<string, int>
    {
        public Heap(int minKeyValue) : base(minKeyValue)
        {

        }
    }

    class Node : FiboNode<string, int>
    {
        public Node(string data, int key) : base(data, key)
        {
        }
    }

    class FiboHeap<T, TKey> where TKey : IComparable
    {
        private static readonly double OneOverLogPhi = 1.0 / Math.Log((1.0 + Math.Sqrt(5.0)) / 2.0);

        private FiboNode<T, TKey> _minNode;

        private int _nCount;
        private readonly TKey _minKeyValue;

        public bool IsEmpty
        {
            get { return _minNode == null; }
        }

        public FiboNode<T, TKey> Min
        {
            get { return _minNode; }
        }

        public int Size
        {
            get { return _nCount; }
        }

        public FiboHeap(TKey minKeyValue)
        {
            _minKeyValue = minKeyValue;
        }

        public void Clear()
        {
            _minNode = null;
            _nCount = 0;
        }

        public void DecreaseKey(FiboNode<T, TKey> x, TKey k)
        {
            if (k.CompareTo(x.Key) > 0)
                throw new ArgumentException("Key is larger");

            x.Key = k;

            var p = x.Parent;

            if ((p != null) && (x.Key.CompareTo(p.Key) < 0))
            {
                Cut(x, p);
                CascadingCut(p);
            }

            if (x.Key.CompareTo(_minNode.Key) < 0)
                _minNode = x;
        }

        public void Delete(FiboNode<T, TKey> x)
        {
            DecreaseKey(x, _minKeyValue);
            RemoveMin();
        }

        public void Insert(FiboNode<T, TKey> x)
        {
            if (!IsEmpty)
            {
                x.Left = _minNode;
                x.Right = _minNode.Right;
                _minNode.Right = x;
                x.Right.Left = x;

                if (x.Key.CompareTo(_minNode.Key) < 0)
                    _minNode = x;
            }
            else
                _minNode = x;

            _nCount++;
        }

        public FiboNode<T, TKey> RemoveMin()
        {
            var min = _minNode;

            if (min != null)
            {
                int numKids = min.Degree;
                var oldKid = min.Child;

                while (numKids > 0)
                {
                    var tempRight = oldKid.Right;

                    oldKid.Left.Right = oldKid.Right;
                    oldKid.Right.Left = oldKid.Left;

                    oldKid.Left = _minNode;
                    oldKid.Right = _minNode.Right;
                    _minNode.Right = oldKid;
                    oldKid.Right.Left = oldKid;

                    oldKid.Parent = null;
                    oldKid.Child = tempRight;
                    numKids--;
                }

                min.Left.Right = min.Right;
                min.Right.Left = min.Left;

                if (min == min.Right)
                    _minNode = null;
                else
                {
                    _minNode = min.Right;
                    Consolidate();
                }

                _nCount--;
            }

            return _minNode;
        }

        public static FiboHeap<T, TKey> Join(FiboHeap<T, TKey> f1, FiboHeap<T, TKey> f2)
        {
            var f = new FiboHeap<T, TKey>(
                f1._minKeyValue.CompareTo(f2._minKeyValue) < 0 ?
                f1._minKeyValue : f2._minKeyValue
                );

            if ((f1 != null) && (f2 != null))
            {
                if (f1._minNode != null)
                {
                    if (f2._minNode != null)
                    {
                        f._minNode.Right.Left = f2._minNode.Left;
                        f2._minNode.Left.Right = f._minNode.Right;
                        f._minNode.Right = f2._minNode;
                        f2._minNode.Left = f._minNode;

                        if (f2._minNode.Key.CompareTo(f1._minNode.Key) < 0)
                            f._minNode = f2._minNode;
                    }
                }
                else
                    f._minNode = f2._minNode;
                f._nCount = f1._nCount + f2._nCount;
            }
            
            return f;
        }

        protected void Consolidate()
        {
            int arraySize = ((int)Math.Floor(Math.Log(_nCount) * OneOverLogPhi)) + 1;
            var array = new List<FiboNode<T, TKey>>(arraySize);
            
            for (var i = 0; i < arraySize; i++)
                array.Add(null);
            
            var numRoots = 0;
            var x = _minNode;

            if (x != null)
            {
                numRoots++;
                x = x.Right;

                while (x != _minNode)
                {
                    numRoots++;
                    x = x.Right;
                }
            }
            
            while (numRoots > 0)
            {
                int d = x.Degree;
                var next = x.Right;
                
                while(true)
                {
                    var y = array[d];
                    if (y == null)
                        break;
                    
                    if (x.Key.CompareTo(y.Key) > 0)
                    {
                        var temp = y;
                        y = x;
                        x = temp;
                    }
                    
                    Link(y, x);
                    
                    array[d] = null;
                    d++;
                }

                array[d] = x;
                
                x = next;
                numRoots--;
            }

            _minNode = null;

            for (var i = 0; i < arraySize; i++)
            {
                var y = array[i];
                if (y == null)
                    continue;
                
                if (_minNode != null)
                {
                    y.Left.Right = y.Right;
                    y.Right.Left = y.Left;
                    
                    y.Left = _minNode;
                    y.Right = _minNode.Right;
                    _minNode.Right = y;
                    y.Right.Left = y;
                    
                    if (y.Key.CompareTo(_minNode.Key) < 0)
                        _minNode = y;
                }
                else
                    _minNode = y;
            }
        }

        protected void Link(FiboNode<T, TKey> newChild, FiboNode<T, TKey> newParent)
        {
            newChild.Left.Right = newChild.Right;
            newChild.Right.Left = newChild.Left;
            
            newChild.Parent = newParent;

            if (newParent.Child == null)
            {
                newParent.Child = newChild;
                newChild.Right = newChild;
                newChild.Left = newChild;
            }
            else
            {
                newChild.Left = newParent.Child;
                newChild.Right = newParent.Child.Right;
                newParent.Child.Right = newChild;
                newChild.Right.Left = newChild;
            }
            
            newParent.Degree++;
            newChild.IsVisited = false;
        }

        protected void CascadingCut(FiboNode<T, TKey> p)
        {
            var parent = p.Parent;

            if (parent != null)
            {
                if (!p.IsVisited)
                    p.IsVisited = true;
                else
                {
                    Cut(p, parent);
                    CascadingCut(parent);
                }
            }
        }

        protected void Cut(FiboNode<T, TKey> x, FiboNode<T, TKey> p)
        {
            if (_minNode == null)
                throw new InvalidOperationException("Heap is empty, nowhere to cut from");

            x.Left.Right = x.Right;
            x.Right.Left = x.Left;

            p.Degree--;

            if (p.Child == x)
                p.Child = x.Right;

            if (p.Degree == 00)
                p.Child = null;

            x.Left = _minNode;
            x.Right = _minNode.Right;
            _minNode.Right = x;
            x.Right.Left = x;

            x.Parent = null;
            x.IsVisited = false;
        }
    }

    class FiboNode<T, Tkey> where Tkey : IComparable
    {
        public FiboNode(T data, Tkey key)
        {
            Right = this;
            Left = this;
            Data = data;
            Key = key;
        }

        public T Data { get; }
        public Tkey Key { get; set; }

        public FiboNode<T, Tkey> Child { get; set; }
        public FiboNode<T, Tkey> Parent { get; set; }
        public FiboNode<T, Tkey> Left { get; set; }
        public FiboNode<T, Tkey> Right { get; set; }

        public bool IsVisited { get; set; }
        public int Degree { get; set; }
    }
}
