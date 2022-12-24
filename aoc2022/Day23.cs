using common;

namespace aoc2022
{
    [AocDay(23, Caption = "Unstable Diffusion")]
    internal class Day23
    {
        [AocTask(1)]
        public long Task1()
        {
            var input = AocInput.GetLines(23, false).Select(l => l.ToCharArray()).ToArray();
            var map = new Map<char>(input).Grow('.');
            var moves = new Dictionary<Point2, List<Point2>>();
            var start = 0;
            var cons = new Func<int, int, bool>[]
            {
                (int x, int y) => { if (map[x-1, y-1] == '.' && map[x, y-1] == '.' && map[x+1, y-1] == '.') { AddTo(moves, new(x,y), new(x, y-1)); return true; } return false; }, // N
                (int x, int y) => { if (map[x-1, y+1] == '.' && map[x, y+1] == '.' && map[x+1, y+1] == '.') { AddTo(moves, new(x,y), new(x, y+1)); return true; } return false; }, // S
                (int x, int y) => { if (map[x-1, y+1] == '.' && map[x-1, y] == '.' && map[x-1, y-1] == '.') { AddTo(moves, new(x,y), new(x-1, y)); return true; } return false; }, // W
                (int x, int y) => { if (map[x+1, y+1] == '.' && map[x+1, y] == '.' && map[x+1, y-1] == '.') { AddTo(moves, new(x,y), new(x+1, y)); return true; } return false; } // E
            };

            for (int rounds = 0; rounds < 10; rounds++)
            {
                var bnd = map.Bounds((c) => c == '#');
                if (bnd.x0 == 0 || bnd.y0 == 0 || bnd.x1 == map.width-1 || bnd.y1 == map.height-1) map = map.Grow('.');
                moves.Clear();
                for (int y = 1; y < map.height-1; y++)
                {
                    for (int x = 1; x < map.width-1; x++)
                    {
                        if (map[x, y] == '.') continue; // Skip empty
                        if (map.Count('.', new Rectangle(x-1, y-1, x+1, y+1)) == 8) continue; // Empty all around - do not move
                        if (cons[start%4](x, y)) continue; // Try 1st direction
                        if (cons[(start+1)%4](x, y)) continue; // Try 2nd direction..
                        if (cons[(start+2)%4](x, y)) continue;
                        if (cons[(start+3)%4](x, y)) continue;
                    }
                }
                foreach (var move in moves)
                {
                    if (move.Value.Count > 1) continue;
                    map[move.Value[0].x, move.Value[0].y] = '.';
                    map[move.Key.x, move.Key.y] = '#';
                }
                start++;
            }
            var bounds = map.Bounds((c) => c == '#');
            return map.Count('.', bounds);
        }
        private void AddTo(Dictionary<Point2, List<Point2>> m, Point2 f, Point2 t) { if (!m.ContainsKey(t)) m.Add(t, new() { f }); else m[t].Add(f); }

        [AocTask(2)]
        public long Task2()
        {
            var input = AocInput.GetLines(23, false).Select(l => l.ToCharArray()).ToArray();
            var map = new Map<char>(input).Grow('.');
            var moves = new Dictionary<Point2, List<Point2>>();
            var start = 0;
            var cons = new Func<int, int, bool>[]
            {
                (int x, int y) => { if (map[x-1, y-1] == '.' && map[x, y-1] == '.' && map[x+1, y-1] == '.') { AddTo(moves, new(x,y), new(x, y-1)); return true; } return false; }, // N
                (int x, int y) => { if (map[x-1, y+1] == '.' && map[x, y+1] == '.' && map[x+1, y+1] == '.') { AddTo(moves, new(x,y), new(x, y+1)); return true; } return false; }, // S
                (int x, int y) => { if (map[x-1, y+1] == '.' && map[x-1, y] == '.' && map[x-1, y-1] == '.') { AddTo(moves, new(x,y), new(x-1, y)); return true; } return false; }, // W
                (int x, int y) => { if (map[x+1, y+1] == '.' && map[x+1, y] == '.' && map[x+1, y-1] == '.') { AddTo(moves, new(x,y), new(x+1, y)); return true; } return false; } // E
            };

            while (true)
            {
                var bnd = map.Bounds((c) => c == '#');
                if (bnd.x0 == 0 || bnd.y0 == 0 || bnd.x1 == map.width-1 || bnd.y1 == map.height-1) map = map.Grow('.');
                moves.Clear();
                for (int y = 1; y < map.height-1; y++)
                {
                    for (int x = 1; x < map.width-1; x++)
                    {
                        if (map[x, y] == '.') continue; // Skip empty
                        if (map.Count('.', new(x-1, y-1, x+1, y+1)) == 8) continue; // Empty all around - do not move
                        if (cons[start%4](x, y)) continue; // Try 1st direction
                        if (cons[(start+1)%4](x, y)) continue; // Try 2nd direction..
                        if (cons[(start+2)%4](x, y)) continue;
                        if (cons[(start+3)%4](x, y)) continue;
                    }
                }
                if (moves.Count == 0) break;
                foreach (var move in moves)
                {
                    if (move.Value.Count > 1) continue;
                    map[move.Value[0].x, move.Value[0].y] = '.';
                    map[move.Key.x, move.Key.y] = '#';
                }
                start++;
            }
            return start+1;
        }
    }
}
