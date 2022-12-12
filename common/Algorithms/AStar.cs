namespace common.Algorithms
{
    public abstract class AStarSolver<T> where T : IEquatable<T>
    {
        public abstract int? TransitionCost(Cell<T> current, Cell<T> next, Map<T> map);
        public abstract int CostToTarget(Cell<T> current, Cell<T> target, Map<T> map);

        public IEnumerable<Cell<T>> Solve(Map<T> map, Cell<T> start, Cell<T> target)
        {
            var openQueue = new PriorityQueue<Node<T>, int>();
            var closed = new HashSet<Cell<T>>();
            var initial = new Node<T>(start); // f=0, g=0
            openQueue.Enqueue(initial, initial.f);

            while (openQueue.Count > 0)
            {
                var curr = openQueue.Dequeue();
                closed.Add(curr.cell);

                if (curr.cell.Equals(target)) return AStarSolver<T>.CreatePathTo(curr); // TODO win

                foreach (var neighbor in map.Neighbors(curr.cell))
                {
                    if (closed.Contains(neighbor)) continue;
                    var next = new Node<T>(neighbor, curr);
                    var travelCost = TransitionCost(curr.cell, neighbor, map);
                    if (travelCost == null) continue; // Cannot move to next location - skip
                    next.g = curr.g + travelCost.Value; // Cost 
                    next.f = next.g + CostToTarget(neighbor, target, map); // Estimated total cost
                    // The terrible search performance in PriorityQueue.UnorderedItems outweighs the benefit of not adding unneccessary expensive nodes
                    //var inopen = openQueue.UnorderedItems.FirstOrDefault(n => n.Element.cell.Equals(next.cell)).Element;
                    //if (inopen?.g < next.g) continue;
                    openQueue.Enqueue(next, next.f);
                }

                if (openQueue.Count == 0)
                    continue;
            }
            return Array.Empty<Cell<T>>();
        }

        private static IEnumerable<Cell<T>> CreatePathTo(Node<T>? node)
        {
            var path = new List<Cell<T>>();
            while (node != null)
            {
                path.Insert(0, node.cell);
                node = node.parent;
            }
            return path;
        }

        private class Node<TNode>
        {
            public int f;
            public int g;
            public readonly Node<TNode>? parent;
            public readonly Cell<TNode> cell;
            public Node(Cell<TNode> cell) { this.cell = cell; }
            public Node(Cell<TNode> cell, Node<TNode> parent) : this(cell) { this.parent = parent; }
            public override string ToString() => $"{cell} f={f}, g={g}";
        }
    }
}
