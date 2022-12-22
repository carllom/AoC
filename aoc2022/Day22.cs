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
            var input = AocInput.GetLines(22, false);

            var offsets = new List<int>();
            var map = new List<string>();
            var warpytop = new int[input.Max(l => l.Length)];
            var warpybtm = new int[input.Max(l => l.Length)];
            foreach (var line in input)
            {
                if (line.Length == 0) break;
                var offset = line.LastIndexOf(' ') + 1;
                offsets.Add(offset);
                map.Add(line.Substring(offset));
            }
            var path = input.Last();
            for (int x = 0; x < warpytop.Length; x++)
            {
                bool hasmap = false;
                for (int y = 0; y < map.Count; y++)
                {
                    var outside = x < offsets[y] || x >= offsets[y]+map[y].Length;
                    if (!outside) { hasmap = true; continue; }
                    if (!hasmap) { warpytop[x] = y; continue; }
                    warpybtm[x] = y; break;
                }
                if (warpytop[x] == 0) warpytop[x] = -1;
                if (warpybtm[x] == 0) warpybtm[x] = map.Count;
            }

            Point2 pos = new(offsets[0], 0);
            int dir = 0; // Facing is 0 for right (>), 1 for down (v), 2 for left (<), and 3 for up (^)
            var ins = NextInstruction(0, path);
            while (ins != default)
            {
                if (ins.instruction == -1) dir = (dir-1 + 4) % 4; // Rotate left
                else if (ins.instruction == -2) dir = (dir+1) % 4; // Rotate right
                else // Move
                {
                    for (int i = 0; i < ins.instruction; i++)
                    {
                        switch (dir)
                        {
                            case 0: //  right (>)
                                var rightx = (pos.x-offsets[pos.y]+1) % map[pos.y].Length;
                                if (map[pos.y][rightx] == '.') { pos.x = rightx + offsets[pos.y]; }
                                break;
                            case 1: // down (v)
                                var downy = pos.y+1;
                                if (downy == warpybtm[pos.x]) downy = warpytop[pos.x]+1;
                                if (map[downy][pos.x-offsets[downy]] == '.') pos.y = downy;
                                break;
                            case 2: // left (<)
                                var leftx = (pos.x-offsets[pos.y]+map[pos.y].Length-1) % map[pos.y].Length;
                                if (map[pos.y][leftx] == '.') { pos.x = leftx + offsets[pos.y]; }
                                break;
                            case 3: // up (^)
                                var upy = pos.y-1;
                                if (upy == warpytop[pos.x]) upy = warpybtm[pos.x]-1;
                                if (map[upy][pos.x-offsets[upy]] == '.') pos.y = upy;
                                break;
                        }
                    }
                }
                ins = NextInstruction(ins.newoffset, path);
            }
            return 1000 * (pos.y+1) + 4 * (pos.x+1) + dir;
        }

        private (int newoffset, int instruction) NextInstruction(int offset, string path)
        {
            StringBuilder sb = new();
            for (int i = offset; i < path.Length; i++)
            {
                if (sb.Length > 0 && !char.IsDigit(path[i]))
                    return (i, int.Parse(sb.ToString()));

                if (path[i] == 'L')
                    return (i+1, -1);
                if (path[i] == 'R')
                    return (i+1, -2);

                sb.Append(path[i]);
            }
            if (sb.Length > 0) return (int.MaxValue, int.Parse(sb.ToString())); // Ends with number
            return default;
        }

        [AocTask(2)]
        public int Task2()
        {
            var input = AocInput.GetLines(22, false);

            var offsets = new List<int>();
            var map = new List<string>();
            foreach (var line in input)
            {
                if (line.Length == 0) break;
                var offset = line.LastIndexOf(' ') + 1;
                offsets.Add(offset);
                map.Add(line.Substring(offset));
            }
            var path = input.Last();

            portalMap = new Portal[map.Count+2, input.Max(l => l.Length)+2];
            CreatePortals(new(0, 99), new(49, 99), new(49, 50), new(49, 99), 1);
            CreatePortals(new(-1, 100), new(-1, 149), new(49, 49), new(49, 0), 2);
            CreatePortals(new(50, 150), new(50, 199), new(50, 150), new(99, 150), -1);
            CreatePortals(new(100, 50), new(100, 99), new(100, 50), new(149, 50), -1);
            CreatePortals(new(-1, 150), new(-1, 199), new(50, -1), new(99, -1), -1);
            CreatePortals(new(0, 200), new(49, 200), new(100, -1), new(149, -1), 0);
            CreatePortals(new(100, 100), new(100, 149), new(150, 49), new(150, 0), 2);
            //Example
            //CreatePortals(new(0, 3), new(3, 3), new(11, -1), new(8, -1), 2);
            //CreatePortals(new(4, 3), new(7, 3), new(7, 0), new(7, 3), 1);
            //CreatePortals(new(12, 0), new(12, 3), new(16, 11), new(16, 8), 2);
            //CreatePortals(new(12, 4), new(12, 7), new(15, 7), new(12, 7), 1);
            //CreatePortals(new(-1, 4), new(-1, 7), new(15, 12), new(12, 12), 1);
            //CreatePortals(new(0, 8), new(3, 8), new(11, 12), new(8, 12), 2);
            //CreatePortals(new(4, 8), new(7, 8), new(7, 11), new(7, 8), -1);

            Console.WriteLine();
            for (int y = 0; y < portalMap.GetLength(0); y++)
            {
                for (int x = 0; x < portalMap.GetLength(1); x++)
                {
                    Console.Write((portalMap[y, x] == null) ? '.' : '~');
                }
                Console.WriteLine();
            }

            Point2 pos = new(offsets[0], 0);
            int dir = 0; // Facing is 0 for right (>), 1 for down (v), 2 for left (<), and 3 for up (^)
            var ins = NextInstruction(0, path);
            while (ins != default)
            {
                if (ins.instruction == -1) dir = (dir-1 + 4) % 4; // Rotate left
                else if (ins.instruction == -2) dir = (dir+1) % 4; // Rotate right
                else // Move
                {
                    for (int i = 0; i < ins.instruction; i++)
                    {
                        (pos,dir,_) = Step(map, offsets, pos, dir);
                    }
                }
                ins = NextInstruction(ins.newoffset, path);
            }
            return 1000 * (pos.y+1) + 4 * (pos.x+1) + dir;
        }

        private (Point2,int,bool) Step(List<string> map, List<int> offsets, Point2 pos, int dir)
        {
            Portal p;
            int ndir = dir;
            Point2 npos = dir switch {
                0 => new Point2(pos.x+1, pos.y), // right (>)
                1 => new Point2(pos.x, pos.y+1), // down (v)
                2 => new Point2(pos.x-1, pos.y), // left (<)
                3 => new Point2(pos.x, pos.y-1)  // up (^)
            };
            p = PortalFor(npos);
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
        
        private Portal[,] portalMap;

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
