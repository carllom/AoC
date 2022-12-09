using common;
using System.Diagnostics.CodeAnalysis;

namespace aoc2022
{
    [AocDay(9, Caption = "Rope Bridge")]
    internal class Day9
    {
        [AocTask(1)]
        public int Task1()
        {
            var input = AocInput.GetLines(9).Select(m => (m[0], int.Parse(m.AsSpan(2))));
            Point2 s = new(), t = new(), d;
            var visited = new HashSet<Point2> { t }; // initial position
            foreach (var line in input)
            {
                for (int i = 0; i < line.Item2; i++)
                {
                    switch(line.Item1) // move head
                    {
                        case 'U': s.y++; break;
                        case 'D': s.y--; break;
                        case 'L': s.x--; break;
                        case 'R': s.x++; break;
                    }
                    d = s-t;
                    t.x += xmove[d.y+2, d.x+2]; // move tail
                    t.y += ymove[d.y+2, d.x+2];
                    visited.Add(t);
                }
            }
            return visited.Count;
        }

        [AocTask(2)]
        public int Task2()
        {
            var input = AocInput.GetLines(9).Select(m => (m[0], int.Parse(m.AsSpan(2))));
            var knots = new Point2[10];

            var visited = new HashSet<Point2> { knots[9] }; // initial position
            foreach (var line in input)
            {
                for (int i = 0; i < line.Item2; i++)
                {
                    switch (line.Item1) // move knot 0
                    {
                        case 'U': knots[0].y++; break;
                        case 'D': knots[0].y--; break;
                        case 'L': knots[0].x--; break;
                        case 'R': knots[0].x++; break;
                    }
                    for (int k = 1; k < knots.Length; k++) // move knots 1..9
                    {
                        var d = knots[k-1]-knots[k];
                        knots[k].x += xmove[d.y+2, d.x+2];
                        knots[k].y += ymove[d.y+2, d.x+2];
                    }
                    visited.Add(knots[9]);
                }
            }
            return visited.Count;
        }

        private static int[,] ymove = { // "upside-down"
            {-1,-1,-1,-1,-1},
            {-1, 0, 0, 0,-1},
            { 0, 0, 0, 0, 0},
            { 1, 0, 0, 0, 1},
            { 1, 1, 1, 1, 1},
        };
        private static int[,] xmove = {
            {-1,-1, 0, 1, 1},
            {-1, 0, 0, 0, 1},
            {-1, 0, 0, 0, 1},
            {-1, 0, 0, 0, 1},
            {-1,-1, 0, 1, 1},
        };
    }
}