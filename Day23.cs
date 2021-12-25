using System.Diagnostics.CodeAnalysis;

namespace aoc2k21
{
    internal class Day23 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Where((_,i) => i<3 || i > 4).Select(l => l).ToArray(); // Skip folded section
            var states = new Queue<State>();
            var initstate = new State(indata, 0) { pods = GetPods(indata) };
            initstate.Goal('A', (3, 3));
            initstate.Goal('B', (5, 3));
            initstate.Goal('C', (7, 3));
            initstate.Goal('D', (9, 3));
            //MoveToGoal(initstate);
            states.Enqueue(initstate);
            State best = new State(Array.Empty<string>(), int.MaxValue); // Dummy state
            while (states.Any()) states = MakeMoves(states, ref best);
            return best.cost;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l).ToArray();
            var states = new Queue<State>();
            var initstate = new State(indata, 0) { pods = GetPods(indata) };
            initstate.Goal('A', (3, 5));
            initstate.Goal('B', (5, 5));
            initstate.Goal('C', (7, 5));
            initstate.Goal('D', (9, 5));
            //MoveToGoal(initstate);
            states.Enqueue(initstate);
            State best = new State(Array.Empty<string>(), int.MaxValue);
            while (states.Any()) states = MakeMoves(states, ref best);
            return best.cost;
        }

        private List<Amphipod> GetPods(string[] map)
        {
            var pods = new List<Amphipod>();
            foreach (var symbol in "ABCD")
            {
                for (int y = 0; y<map.Length; y++)
                {
                    for (int x = 0; x<map[y].Length; x++)
                    {
                        if (map[y][x] == symbol) pods.Add(new Amphipod(symbol, (x, y)));
                    }
                }
            }
            return pods;
        }

        private Queue<State> MakeMoves(Queue<State> states, ref State best)
        {
            Console.WriteLine(states.Count);
            var max = states.Count;
            for (int sIdx = 0; sIdx < max; sIdx++)
            {
                var state = states.Dequeue();
                foreach (var pod in state.pods)
                {
                    if (pod.InCorridor) continue; // Already in corridor, skip
                    foreach (var corrPos in valid_corr_dest)
                    {
                        var moveCost = CheckMove(state.map, pod.pos, corrPos);
                        if (moveCost < 0) continue; // Move failed
                        var nextState = new State(state);
                        nextState.moves.Add((pod.symbol, pod.pos, corrPos, moveCost*pod.Cost)); // Register move
                        nextState.pods.Find(p => p.Equals(pod)).pos = corrPos; // Update pod pos in new state
                        nextState.cost += moveCost*pod.Cost;
                        nextState.map = MoveSymbol(state.map, pod.pos, corrPos);
                        // MoveToGoal affects passed state directly as it is never an option not to move a piece to its goal
                        MoveToGoal(nextState);
                        if (nextState.pods.Count == 0) // No more pods to move == goal
                        {
                            if (best.cost > nextState.cost)
                            {
                                var s = nextState;
                                Console.WriteLine($"New best: {nextState.cost}");
                                best = nextState;
                            }
                        }
                        else if (nextState.cost < best.cost) states.Enqueue(nextState); // No point in continuing state if it is more expensive than the best
                    }
                }
                // If we have reached this point without adding a child state for the current state the game is unsolveable and should not be continued anyway
            }
            return states;
        }

        private void MoveToGoal(State state)
        {
            var moved = false;
            do
            {
                var toRemove = new List<Amphipod>();
                moved = false;
                foreach (var pod in state.pods)
                {
                    if (MoveToGoal(state, pod))
                    {
                        toRemove.Add(pod);
                        moved = true;
                    }
                }
                foreach (var pod in toRemove) state.pods.Remove(pod);
            } while (moved);
        }

        private bool MoveToGoal(State state, Amphipod pod)
        {
            var goalpos = state.Goal(pod.symbol);
            var goalCost = CheckMove(state.map, pod.pos, goalpos);
            if (goalCost < 0) return false; // Move failed
            state.moves.Add((pod.symbol, pod.pos, goalpos, goalCost * pod.Cost));
            state.cost+=goalCost * pod.Cost;
            state.map = MoveSymbol(state.map, pod.pos, goalpos, 'X');
            state.Goal(pod.symbol, goalpos.Up);
            pod.pos = goalpos;
            return true;
        }

        private string[] MoveSymbol(string[] map, P2D from, P2D to, char? replaceSymbol = null)
        {
            var newMap = (string[])map.Clone();
            var sym = map[from.y][from.x];
            newMap = UpdateMap(newMap, from, '.');
            return UpdateMap(newMap, to, replaceSymbol ?? sym);
        }

        private string[] UpdateMap(string[] map, P2D pos, char symbol)
        {
            var line = map[pos.y].ToCharArray();
            line[pos.x] = symbol;
            map[pos.y] = new string(line);
            return map;
        }

        private int CheckMove(string[] map, P2D pos, P2D to)
        {
            P2D curr = pos;
            int costX;
            int costY;
            if (curr.y >= to.y)
            {
                costY = MoveVert(map, pos, to.y); // Destination corridor - move vertically first
                if (costY < 0) return -1;
                costX = MoveHorz(map, (pos.x, to.y), to.x);
                if (costX < 0) return -1;
            }
            else
            {
                costX = MoveHorz(map, pos, to.x); // Destination goal - move horizontally first
                if (costX < 0) return -1;
                costY = MoveVert(map, (to.x, pos.y), to.y);
                if (costY < 0) return -1;
            }
            return costX + costY;
        }

        private int MoveVert(string[] map, P2D pos, int to)
        {
            var steps = 0;
            if (pos.y == to) return 0;
            if (pos.y > to)
                while (pos.y > to && IsEmpty(map, pos.Up)) { pos = pos.Up; steps++; }
            else
                while (pos.y < to && IsEmpty(map, pos.Down)) { pos = pos.Down; steps++; }
            return pos.y == to ? steps : -1;
        }
        private int MoveHorz(string[] map, P2D pos, int to)
        {
            var steps = 0;
            if (pos.x == to) return 0;
            if (pos.x < to)
                while (pos.x < to && IsEmpty(map, pos.Right)) { pos = pos.Right; steps++; }
            else
                while (pos.x > to && IsEmpty(map, pos.Left)) { pos = pos.Left; steps++; }
            return pos.x == to ? steps : -1;
        }

        private bool IsEmpty (string[] map, P2D pos) => map[pos.y][pos.x] == '.';

        private static readonly P2D[] valid_corr_dest = new P2D[] { (1, 1), (2, 1), (4, 1), (6, 1), (8, 1), (10, 1), (11, 1) }; // Valid corridor positions

        private class Amphipod
        {
            private static readonly int[] move_cost = new int[] { 1, 10, 100, 1000 }; // Move cost for A,B,C,D respectively
            public bool InCorridor => pos.y == 1;
            public int Cost => move_cost[symbol - 'A'];
            public readonly char symbol;
            public P2D pos;
            public Amphipod(char symbol, P2D pos) { this.symbol = symbol; this.pos = pos; }
            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                if (obj == null) return false;
                var other = (Amphipod)obj;
                return symbol == other.symbol && pos.x == other.pos.x && pos.y == other.pos.y;
            }
            public Amphipod(Amphipod a) { symbol = a.symbol; pos = a.pos; }
        }

        private struct P2D
        {
            public readonly int x;
            public readonly int y;
            public P2D(int x, int y) { this.x=x; this.y=y;}
            public static implicit operator P2D((int x,int y) tuple) { return new P2D(tuple.x, tuple.y); }
            public P2D Up => (x, y-1);
            public P2D Down => (x, y+1);
            public P2D Left => (x-1, y);
            public P2D Right => (x+1, y);
        }

        private class State
        {
            public List<Amphipod> pods = new List<Amphipod> ();
            public List<(char symbol, P2D from, P2D to, int cost)> moves = new List<(char, P2D from, P2D to, int cost)>();
            public string[] map;
            public P2D[] goal = new P2D[4];
            public P2D Goal(char symbol) => goal[symbol - 'A'];
            public void Goal(char symbol, P2D point) { goal[symbol- 'A'] = point; }

            public int cost;
            public State(string[] map, int cost) { this.map = map; this.cost = cost; }
            public State(State parent) { 
                map = (string[])parent.map.Clone();
                Array.Copy(parent.goal, goal, 4);
                cost = parent.cost;
                pods = parent.pods.Select(a => new Amphipod(a)).ToList();
            }
        }
    }
}
