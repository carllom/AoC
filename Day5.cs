using System.Linq;
namespace aoc2k21
{
    internal class Day5 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(Line.ParseLine).ToArray();

            //if (indata.Any(l => l.y1 < l.y0) || indata.Any(l => l.x1 < l.x0)) throw new Exception("input error");

            var maxx = indata.Max(l => Math.Max(l.x1,l.x0));
            var maxy = indata.Max(l => Math.Max(l.y1,l.y0));
            var img = new int[maxx+1, maxy+1]; // inclusive max coordinate

            foreach (var l in indata)
            {
                // hor
                if (l.y0 == l.y1)
                {
                    var xf = Math.Min(l.x0, l.x1);
                    var xt = Math.Max(l.x0, l.x1);
                    for (int x = xf; x <= xt; x++)
                    {
                        img[x, l.y0]++;
                    }
                }
                else if (l.x0 == l.x1)
                {
                    var yf = Math.Min(l.y0, l.y1);
                    var yt = Math.Max(l.y0, l.y1);
                    for (int y = yf; y <= yt; y++)
                    {
                        img[l.x0, y]++;
                    }
                }
                // skip slanted
            }

            var result = 0;
            for (int x = 0; x <= maxx; x++)
            {
                for (int y = 0; y <= maxy; y++)
                {
                    if (img[x, y] > 1) result++;
                }

            }

            return result;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(Line.ParseLine).ToArray();

            //if (indata.Any(l => l.y1 < l.y0) || indata.Any(l => l.x1 < l.x0)) throw new Exception("input error");

            var maxx = indata.Max(l => Math.Max(l.x1, l.x0));
            var maxy = indata.Max(l => Math.Max(l.y1, l.y0));
            var img = new int[maxx+1, maxy+1]; // inclusive max coordinate

            foreach (var l in indata)
            {
                // horizontal
                if (l.y0 == l.y1)
                {
                    var xf = Math.Min(l.x0, l.x1);
                    var xt = Math.Max(l.x0, l.x1);
                    for (int x = xf; x <= xt; x++)
                    {
                        img[x, l.y0]++;
                    }
                }
                else if (l.x0 == l.x1)
                {
                    var yf = Math.Min(l.y0, l.y1);
                    var yt = Math.Max(l.y0, l.y1);
                    for (int y = yf; y <= yt; y++)
                    {
                        img[l.x0, y]++;
                    }
                }
                else
                {
                    var xs = l.x1 > l.x0 ? 1 : -1; // x step direction
                    var ys = l.y1 > l.y0 ? 1 : -1; // y step direction

                    // Step
                    for (int s = 0; s <= Math.Abs(l.x1-l.x0); s++) // xd=yd since diagonal is 45 degrees
                    {
                        img[l.x0+(s*xs), l.y0+(s*ys)]++;
                    }
                }
            }

            var result = 0;
            for (int x = 0; x <= maxx; x++)
            {
                for (int y = 0; y <= maxy; y++)
                {
                    if (img[x, y] > 1) result++;
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
