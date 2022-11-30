namespace aoc2021
{
    internal class Day15 : IAocTask
    {
        public long Task1(string indatafile)
        {
            Node[][] indata = File.ReadAllLines(indatafile).Select((l,yi) => l.ToCharArray().Select((carr,xi) => new Node(xi, yi, int.Parse(carr.ToString()))).ToArray()).ToArray();
            var trace = Astar(indata); // The A* shortest path solver
            return trace.First().GCost() - trace.Last().GCost();
        }

        public long Task2(string indatafile)
        {
            Node[][] indata = File.ReadAllLines(indatafile).Select((l, yi) => l.ToCharArray().Select((carr, xi) => new Node(xi, yi, int.Parse(carr.ToString()))).ToArray()).ToArray();

            int mult = 5;
            Node[][] indata2 = new Node[indata.Length * mult][];
            for (int yi = 0; yi < mult; yi++)
            {
                for (int y = 0; y < indata.Length; y++)
                {
                    for (int xi = 0; xi < mult; xi++)
                    {
                        if (indata2[(indata.Length*yi)+y] == null) indata2[(indata.Length*yi)+y] = new Node[indata[y].Length * mult];
                        for (int x = 0; x < indata[y].Length; x++)
                        {
                            indata2[(indata.Length*yi)+y][(indata[y].Length*xi)+x] = new Node((indata[y].Length*xi)+x, (indata.Length*yi)+y, ((indata[y][x].risk + xi + yi - 1) % 9) + 1);
                        }
                    }
                }
            }
            var trace = Astar(indata2); // Same but 5*bigger
            return trace.First().GCost() - trace.Last().GCost();
        }

        private IEnumerable<Node> Astar(Node[][] map)
        {
            var openPq = new PriorityQueue<Node, int>(); // New in .NET6 - Thanks Mr. Microsoft!
            var start = map[0][0];
            Node goal = map[map.Length-1][map[0].Length-1];

            start.state = State.Open;
            openPq.Enqueue(start, Math.Abs(start.x-goal.x) + Math.Abs(start.y-goal.y)); // Start with manhattan distance to goal (shortest path when only lateral movements are allowed)

            while (openPq.Peek() != goal)
            {
                var curr = openPq.Dequeue();
                curr.state = State.Closed;
                foreach (var neighbour in curr.Neighbours(map))
                {
                    if (neighbour == curr.parent) continue;

                    var cost = curr.GCost() + neighbour.risk;
                    if (neighbour.state == State.Open && cost < neighbour.GCost())
                    {
                        openPq = new PriorityQueue<Node, int>(openPq.UnorderedItems.Where(i => i.Element != neighbour));
                        neighbour.state = State.None;
                    }
                    else if (neighbour.state == State.Closed && cost < neighbour.GCost())
                        continue;
                    else if (neighbour.state == State.None) // In neither
                    {
                        neighbour.parent = curr;
                        neighbour.state = State.Open;
                        openPq.Enqueue(neighbour, cost + 1/*Math.Abs(neighbour.x-goal.x) + Math.Abs(neighbour.y-goal.y);*/); // Do wide search (h(f) = 1. Manhattan distance gave a suboptimal solution..
                    }
                }
            }

            var g = openPq.Dequeue(); // First entry is best path
            var w = new List<Node>();
            while (g != null) // Backtrack path from goal to start
            {
                w.Add(g);
                g = g.parent;
            }
            return w;
        }

        private class Node
        {
            public Node? parent;
            public readonly int x;
            public readonly int y;
            public int risk;
            public State state = State.None;

            public Node(int x, int y, int risk)
            {
                this.x = x;
                this.y = y;
                this.risk = risk;
            }

            private int g = -1;
            public int GCost() => g <= -1 ? (g = risk + parent?.GCost() ?? risk) : g;

            public IEnumerable<Node> Neighbours(Node[][] map)
            {
                var nb = new List<Node>();
                if (y > 0) nb.Add(map[y - 1][x]);
                if (y < map.Length-1) nb.Add(map[y+1][x]);
                if (x > 0) nb.Add(map[y][x-1]);
                if (x < map[y].Length-1) nb.Add(map[y][x+1]);
                return nb;
            }
        }

        private enum State
        {
            None,
            Open,
            Closed
        }
    }
}
