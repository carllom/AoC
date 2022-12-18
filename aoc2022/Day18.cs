using common;

namespace aoc2022
{
    [AocDay(18, Caption = "Boiling Boulders")]
    internal class Day18
    {
        [AocTask(1)]
        public long Task1()
        {
            var input = AocInput.GetLines(18).Select(l => { var c = l.Split(','); return new Point3(int.Parse(c[0]), int.Parse(c[1]), int.Parse(c[2])); }).ToHashSet();
            return Surface(input);
        }

        [AocTask(2)]
        public long Task2()
        {
            var input = AocInput.GetLines(18).Select(l => { var c = l.Split(','); return new Point3(int.Parse(c[0]), int.Parse(c[1]), int.Parse(c[2])); }).ToHashSet();
            var span = input.Span();
            var outer = new Block(span.x0-1, span.y0-1, span.z0-1, span.x1+1, span.y1+1, span.z1+1); // Outer shell is 1 larger on all sides. This enures erosion can reach all points later on
            var invinput = Invert(input, outer); // Use negative volume
            var eroded = Erode(invinput, new Point3(outer.p0.x, span.p0.y, span.p0.z)); // Erode/remove points on the outside
            return Surface(input) - Surface(eroded);
        }

        private IEnumerable<Point3> Adjoining(Point3 p, ISet<Point3> points)
        {
            var n = new List<Point3>();
            var p0 = new Point3(p.x-1, p.y, p.z); if (points.Contains(p0)) n.Add(p0);
            var p1 = new Point3(p.x+1, p.y, p.z); if (points.Contains(p1)) n.Add(p1);
            var p2 = new Point3(p.x, p.y-1, p.z); if (points.Contains(p2)) n.Add(p2);
            var p3 = new Point3(p.x, p.y+1, p.z); if (points.Contains(p3)) n.Add(p3);
            var p4 = new Point3(p.x, p.y, p.z-1); if (points.Contains(p4)) n.Add(p4);
            var p5 = new Point3(p.x, p.y, p.z+1); if (points.Contains(p5)) n.Add(p5);
            //foreach (var point in points) if (point.ManhDist(p) == 1) n.Add(point); // Nicer but soo slow...
            return n;
        }

        private long Surface(ISet<Point3> points)
        {
            long surface = 0;
            foreach (var point in points)
            {
                surface += 6 - Adjoining(point, points).Count();
            }
            return surface;
        }

        private ISet<Point3> Invert(ISet<Point3> points, Block bounds)
        {
            var inv = new HashSet<Point3>();
            for (int i = bounds.p0.x; i <= bounds.p1.x; i++)
            {
                for (int j = bounds.p0.y; j <= bounds.p1.y; j++)
                {
                    for (int k = bounds.p0.z; k <= bounds.p1.z; k++)
                    {
                        var p = new Point3(i, j, k);
                        if (!points.Contains(p)) inv.Add(p);
                    }
                }
            }
            return inv;
        }

        private ISet<Point3> Erode(ISet<Point3> points, Point3 startAt)
        {
            points.Remove(startAt);
            var nbr = Adjoining(startAt, points);
            foreach (var n in nbr) points.Remove(n);
            if (nbr.Any()) foreach (var n in nbr) Erode(points, n);
            return points;
        }
    }
}
