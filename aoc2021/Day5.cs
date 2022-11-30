namespace aoc2021
{
    internal class Day5 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var lines = File.ReadAllLines(indatafile).Select(Line.ParseLine).ToArray();
            var xsize = lines.Max(l => Math.Max(l.x1,l.x0));
            var ysize = lines.Max(l => Math.Max(l.y1,l.y0));
            var map = new int[xsize+1, ysize+1]; // inclusive max coordinate

            foreach (var l in lines)
            {
                // 1. Naive solution using separate code for the different cases
                if (l.y0 == l.y1)
                {
                    var xf = Math.Min(l.x0, l.x1);
                    var xt = Math.Max(l.x0, l.x1);
                    for (int x = xf; x <= xt; x++)
                    {
                        map[x, l.y0]++;
                    }
                }
                else if (l.x0 == l.x1)
                {
                    var yf = Math.Min(l.y0, l.y1);
                    var yt = Math.Max(l.y0, l.y1);
                    for (int y = yf; y <= yt; y++)
                    {
                        map[l.x0, y]++;
                    }
                }
                // skip slanted
            }

            return Crossings(map);
        }

        public long Task2(string indatafile)
        {
            var lines = File.ReadAllLines(indatafile).Select(Line.ParseLine).ToArray();
            var xsize = lines.Max(l => Math.Max(l.x1, l.x0));
            var ysize = lines.Max(l => Math.Max(l.y1, l.y0));
            var map = new int[xsize+1, ysize+1]; // inclusive max coordinate

            foreach (var l in lines)
            {
                // 2. Nicer generic solution using step iteration instead of x/y iteration
                var xs = l.x1 == l.x0 ? 0 : l.x1 > l.x0 ? 1 : -1; // x step direction. -1,0 or 1
                var ys = l.y1 == l.y0 ? 0 : l.y1 > l.y0 ? 1 : -1; // y step direction. -1,0 or 1
                var steps = Math.Max(Math.Abs(l.x1-l.x0), Math.Abs(l.y1-l.y0)); // Number of steps. abs(xd)=abs(yd) if slanted
                
                for (int s = 0; s <= steps; s++)
                {
                    map[l.x0+(s*xs), l.y0+(s*ys)]++;
                }
            }

            return Crossings(map);
        }

        private int Crossings(int[,] map)
        {
            var result = 0;
            for (int x = 0; x <= map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    if (map[x, y] > 1) result++;
                }
            }
            return result;
        }
    }

    struct Line
    {
        public Line(int x0, int y0, int x1, int y1) { this.x0=x0; this.y0=y0; this.x1=x1; this.y1=y1; }
        public static Line ParseLine(string l)
        {
            var c = l.Split(" -> ").Select(c => c.Split(',')).ToArray();
            return new Line(int.Parse(c[0][0]), int.Parse(c[0][1]), int.Parse(c[1][0]), int.Parse(c[1][1]));
        }
        public int x0;
        public int y0;
        public int x1;
        public int y1;
    }
}
