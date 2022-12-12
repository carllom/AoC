using common;
using common.Algorithms;

namespace aoc2022
{
    [AocDay(12, Caption = "Hill Climbing Algorithm")]
    internal class Day12
    {
        private class HillClimbSolver : AStarSolver<char>
        {
            public override int CostToTarget(Cell<char> current, Cell<char> target, Map<char> map) => Math.Abs(target.x - current.x) + Math.Abs(target.y - current.y);
            public override int? TransitionCost(Cell<char> current, Cell<char> next, Map<char> map) => next.value - current.value > 1 ? null : 1;
        }

        [AocTask(1)]
        public int Task1()
        {
            var map = new Map<char>(AocInput.GetLines(12).Select(l => l.ToCharArray()).ToArray());

            var solver = new HillClimbSolver();
            var start = map.Find('S')!;
            start.value = 'a';
            map.Set(start);
            var goal = map.Find('E')!;
            goal.value = 'z';
            map.Set(goal);
            return solver.Solve(map, start, goal).Count()-1;
        }

        [AocTask(2)]
        public int Task2()
        {
            var map = new Map<char>(AocInput.GetLines(12).Select(l => l.ToCharArray()).ToArray());

            var solver = new HillClimbSolver();
            var start = map.Find('S')!;
            start.value = 'a';
            map.Set(start);
            var goal = map.Find('E')!;
            goal.value = 'z';
            map.Set(goal);
            var shortest = int.MaxValue;
            foreach (var strt in map.FindAll('a').Where(a => map.Neighbors(a).Any(n => n.value == 'b')))
            {
                var steps = solver.Solve(map, strt, goal).Count()-1;
                if (steps >= 0) shortest = Math.Min(shortest, steps);
            }
            return shortest;
        }
    }
}
