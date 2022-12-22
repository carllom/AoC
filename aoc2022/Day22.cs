using common;
using System.Text;

namespace aoc2022
{
    [AocDay(22, Caption = "Monkey Map ")]
    internal class Day22
    {
        [AocTask(1)]
        public int Task1()
        {
            SetupData();
            CreatePortals(new(0, 99), new(49, 99), new(0,200), new(49, 200), 0);
            CreatePortals(new(50, -1), new(99, -1), new(50, 150), new(99, 150), 0);
            CreatePortals(new(100, -1), new(149, -1), new(100, 50), new(149, 50), 0);
            CreatePortals(new(49, 0), new(49, 49), new(150, 0), new(150, 49), 0);
            CreatePortals(new(49, 50), new(49, 99), new(100, 50), new(100, 99), 0);
            CreatePortals(new(-1, 100), new(-1, 149), new(100, 100), new(100, 149), 0);
            CreatePortals(new(-1, 150), new(-1, 199), new(50, 150), new(50, 199), 0);
            var (pos, dir) = FollowPath(new(offsets[0], 0), 0);
            return 1000 * (pos.y+1) + 4 * (pos.x+1) + dir;
        }

        [AocTask(2)]
        public int Task2()
        {
            SetupData();
            CreatePortals(new(0, 99), new(49, 99), new(49, 50), new(49, 99), 1);
            CreatePortals(new(-1, 100), new(-1, 149), new(49, 49), new(49, 0), 2);
            CreatePortals(new(50, 150), new(50, 199), new(50, 150), new(99, 150), -1);
            CreatePortals(new(100, 50), new(100, 99), new(100, 50), new(149, 50), -1);
            CreatePortals(new(-1, 150), new(-1, 199), new(50, -1), new(99, -1), -1);
            CreatePortals(new(0, 200), new(49, 200), new(100, -1), new(149, -1), 0);
            CreatePortals(new(100, 100), new(100, 149), new(150, 49), new(150, 0), 2);
            var (pos, dir) = FollowPath(new(offsets[0], 0), 0);
            return 1000 * (pos.y+1) + 4 * (pos.x+1) + dir;
        }

        private List<int> offsets = new List<int>();
        private List<string> map = new List<string>();
        private string path = string.Empty;
        private Portal[,] portalMap;

        private void SetupData()
        {
            var input = AocInput.GetLines(22, false);

            offsets.Clear(); ;
            map.Clear();
            foreach (var line in input)
            {
                if (line.Length == 0) break;
                var offset = line.LastIndexOf(' ') + 1;
                offsets.Add(offset);
                map.Add(line.Substring(offset));
            }
            path = input.Last();
            portalMap = new Portal[map.Count+2, input.Max(l => l.Length)+2];
        }

        public (Point2,int) FollowPath(Point2 pos, int dir)
        {
            var ins = NextInstruction(0, path);
            while (ins != default)
            {
                if (ins.instruction == -1) dir = (dir-1 + 4) % 4; // Rotate left
                else if (ins.instruction == -2) dir = (dir+1) % 4; // Rotate right
                else // Move
                {
                    for (int i = 0; i < ins.instruction; i++) (pos, dir, _) = Step(map, offsets, pos, dir);
                }
                ins = NextInstruction(ins.newoffset, path);
            }
            return (pos, dir);
        }

        private (int newoffset, int instruction) NextInstruction(int offset, string path)
        {
            StringBuilder sb = new();
            for (int i = offset; i < path.Length; i++)
            {
                if (sb.Length > 0 && !char.IsDigit(path[i])) return (i, int.Parse(sb.ToString()));
                if (path[i] == 'L') return (i+1, -1);
                if (path[i] == 'R') return (i+1, -2);
                sb.Append(path[i]);
            }
            if (sb.Length > 0) return (int.MaxValue, int.Parse(sb.ToString())); // Ends with number
            return default;
        }


        private (Point2,int,bool) Step(List<string> map, List<int> offsets, Point2 pos, int dir)
        {
            int ndir = dir;
            var npos = dir switch {
                0 => new Point2(pos.x+1, pos.y), // right (>)
                1 => new Point2(pos.x, pos.y+1), // down (v)
                2 => new Point2(pos.x-1, pos.y), // left (<)
                3 => new Point2(pos.x, pos.y-1)  // up (^)
            };
            var p = PortalFor(npos);
            bool ok = true;
            if (p != null) (npos,ndir,ok) = Step(map, offsets, p.dest, (dir+p.rot+4)%4);
            if (ok && map[npos.y][npos.x-offsets[npos.y]] == '.') return (npos,ndir,true);
            return (pos, dir,false);
        }

        private class Portal
        {
            public Point2 dest;
            public int rot;
            public Portal(Point2 dest, int rot) { this.dest=dest; this.rot=rot; }
        }
        
        private Portal PortalFor(Point2 p) => portalMap[p.y+1, p.x+1];

        private void CreatePortals(Point2 w0a, Point2 w0b, Point2 w1a, Point2 w1b, int rotation)
        {
            var w0d = w0b-w0a;
            for (int d = 0; d <= Math.Max(w0d.x, w0d.y); d++)
            {
                var w0 = InterpolateHV(w0a, w0b, d);
                var w1 = InterpolateHV(w1a, w1b, d);
                portalMap[w0.y+1, w0.x+1] = new(w1, rotation);
                portalMap[w1.y+1, w1.x+1] = new(w0, -rotation);
            }
        }
        
        private Point2 InterpolateHV(Point2 a, Point2 b, int pos)
        {
            var pd = b-a;
            var nx = pd.x == 0 ? a.x : a.x + Math.Abs(pd.x) * pos / pd.x;
            var ny = pd.y == 0 ? a.y : a.y + Math.Abs(pd.y) * pos / pd.y;
            return new Point2(nx, ny);
        }
    }
}
