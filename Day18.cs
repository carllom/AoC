using System.Diagnostics;

namespace aoc2k21
{
    internal class Day18 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => new Queue<char>(l.ToCharArray())).ToArray();
            List<Node> school = indata.Select(q => ParseNodes(q)).ToList();

            var fSum = school[0];
            for (int i = 1; i < school.Count; i++)
            {
                //Console.WriteLine(fSum);
                fSum = SplitExplode(new Node(fSum, school[i]));
                //Console.WriteLine();
            }
 
            return Magnitude(fSum);
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => new Queue<char>(l.ToCharArray())).ToArray();
            List<Node> school = indata.Select(q => ParseNodes(q)).ToList();

            long maxSum = 0;
            for (int i = 0; i < school.Count-1; i++)
            {
                for (int j = i+1; j < school.Count; j++)
                {
                    maxSum = Math.Max(maxSum, Magnitude(SplitExplode(new Node(school[i].Clone(), school[j].Clone())))); // Addition is not commutative, try a+b and b+a
                    maxSum = Math.Max(maxSum, Magnitude(SplitExplode(new Node(school[j].Clone(), school[i].Clone())))); // Clone because SplitExplode mutates passed nodes and we reuse school[n] in later iterations.
                }
            }
            return maxSum;
        }

        private Node SplitExplode(Node n)
        {
            while (Explode(n, null, null, 0) || Split(n)) { /*Console.WriteLine(fSum);*/ }
            return n;
        }

        private bool Explode(Node n, Node? lLit, Node? rLit, int depth)
        {
            if (n.Leaf && depth >= 4) // Explode
            {
                if (lLit != null) lLit.Value += n.Left.Value;
                if (rLit != null) rLit.Value += n.Right.Value;
                n.ToLiteral(0);
                return true;
            }
            return (!n.Left.Literal && Explode(n.Left, lLit, n.Right.FirstLit ?? rLit, depth+1)) ||
                (!n.Right.Literal && Explode(n.Right, n.Left.LastLit ?? lLit, rLit, depth+1));
        }

        private bool Split(Node n)
        {
            if (n.Literal && n.Value > 9) // Split
            {
                n.ToLeaf(n.Value/2, n.Value/2 + n.Value %2);
                return true;
            }
            return !n.Literal && (Split(n.Left) || Split(n.Right));
        }

        private long Magnitude(Node n)
        {
            if (n.Literal) return n.Value;
            return 3L * Magnitude(n.Left) + 2L * Magnitude(n.Right);
        }

        private Node ParseNodes(Queue<char> q) 
        {
            var l = q.Dequeue();
            if (char.IsDigit(l)) return new Node(l-'0'); // Leaf node
            Debug.Assert(l == '[', $"Parse error:expected '[', got {l}");
            var left = ParseNodes(q); // Parse left side

            var sep = q.Dequeue(); // Eat separator(,)
            Debug.Assert(sep == ',', $"Parse error: expected ',', got {sep}");

            var right = ParseNodes(q); // Parse right side

            var end = q.Dequeue(); // Eat closing bracket(])
            Debug.Assert(end == ']', $"Parse error: expected ']', got {end}");
            return new Node(left, right);
        }
    }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    internal class Node
    {
        public Node Left { get; private set; } 
        public Node Right { get; private set; } 
        private int value = -1;
        public int Value { 
            get => value; 
            set
            {
                Debug.Assert(Literal, "Node is not a literal");
                Debug.Assert(value >= 0, "Literal value cannot be negative");
                this.value = value;
            } 
        }
        public bool Literal => value >= 0 && Left == null && Right == null;
        public bool Leaf => value < 0 && Left.Literal && Right.Literal;

        public void ToLiteral(int value)
        {
            Debug.Assert(value >= 0, "Literal value cannot be negative");
            this.value = value;
            Left = null;
            Right = null;
        }

        public void ToLeaf(int left, int right)
        {
            Left = new Node(left);
            Right = new Node(right);
            value = -1;
        }

        public Node Clone()
        {
            if (Literal) return new Node(value);
            return new Node(Left.Clone(), Right.Clone());
        }

        public Node? FirstLit => Literal ? this : Left?.FirstLit;
        public Node? LastLit => Literal ? this : Right?.LastLit;

        public Node(Node left, Node right)
        {
            Left=left;
            Right=right;
        }
        public Node(int value) : this(null,null)
        {
            Debug.Assert(value >= 0, "Literal value cannot be negative");
            this.value = value;
        }

        public override string ToString() => Literal ? value.ToString() : $"[{Left},{Right}]";
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
