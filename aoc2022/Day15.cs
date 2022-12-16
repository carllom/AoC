using common;

namespace aoc2022
{
    [AocDay(15, Caption = "")]
    internal class Day15
    {
        [AocTask(1)]
        public int Task1()
        {
            var input = AocInput.GetLines(15, false).Select(l => l.Split(':').Select(c => c.Split("at ")[1])
                .Select(c => { var x = c.Split(',', '='); return new Point2(int.Parse(x[1]), int.Parse(x[3])); }).ToArray()).Select(c => new SbPair(c[0], c[1]));

            Rectangle span = new(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            foreach (var p in input)
            {
                span.x0 = int.Min(span.x0, p.sensor.x - p.dist);
                span.x1 = int.Max(span.x1, p.sensor.x + p.dist);
                span.y0 = int.Min(span.y0, p.sensor.y - p.dist);
                span.y1 = int.Max(span.y1, p.sensor.y + p.dist);
            }

            var y = 2000000;
            var ranges = new List<Range1D>();
            foreach (var sb in input)
            {
                if (!sb.Covers(y)) continue;
                ranges.Add(sb.SpanAt(y));
            }
            ranges.Sort((a, b) => a.from - b.from);
            var nurng = new List<Range1D>();
            var g = ranges[0];
            for (int i = 1; i < ranges.Count; i++)
            {
                var q = g.Add(ranges[i]);
                if (q.Length > 1) nurng.Add(q[0]);
                g = q[q.Length-1];
            }
            nurng.Add(g);
            return nurng.Sum(r => r.Size);
        }

        [AocTask(2)]
        public long Task2()
        {
            var input = AocInput.GetLines(15, false).Select(l => l.Split(':').Select(c => c.Split("at ")[1])
                .Select(c => { var x = c.Split(',', '='); return new Point2(int.Parse(x[1]), int.Parse(x[3])); }).ToArray()).Select(c => new SbPair(c[0], c[1]));

            Rectangle span = new(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            foreach (var p in input)
            {
                span.x0 = int.Min(span.x0, p.sensor.x - p.dist);
                span.x1 = int.Max(span.x1, p.sensor.x + p.dist);
                span.y0 = int.Min(span.y0, p.sensor.y - p.dist);
                span.y1 = int.Max(span.y1, p.sensor.y + p.dist);
            }

            var limit = 4000000;
            for (int y = 0; y <= limit; y++)
            {
                var ranges = new List<Range1D>();
                foreach (var sb in input)
                {
                    if (!sb.Covers(y)) continue;
                    var sp = sb.SpanAt(y);
                    if (sp.to < 0 || sp.from > limit) continue;
                    ranges.Add(sp);
                }

                ranges.Sort((a, b) => a.from - b.from);
                var nurng = new List<Range1D>();
                var g = ranges[0];
                for (int i = 1; i < ranges.Count; i++)
                {
                    var q = g.Add(ranges[i]);
                    if (q.Length > 1) nurng.Add(q[0]);
                    g = q[q.Length-1];
                }
                nurng.Add(g);
                if (nurng.Count > 1)
                {
                    var h = new Range1D(0, limit);
                    var i = h.Subtract(nurng[0])[0].Subtract(nurng[1])[0];
                    return 4000000L*i.from + y;
                }
                if (y%100000==0) Console.Write('.');
                //return nurng.Sum(r => r.Size);
            }

            return default;
        }
    }

    public class SbPair
    {
        public Point2 sensor;
        public Point2 beacon;
        public int dist;
        public int firstY;
        public int lastY;

        public SbPair(Point2 sensor, Point2 beacon) { 
            this.sensor = sensor;
            this.beacon = beacon;
            dist = sensor.ManhDist(beacon);
            firstY = sensor.y - dist;
            lastY = sensor.y + dist;
        }

        public bool Covers(int y) => firstY <= y && lastY >= y;

        public Range1D SpanAt(int y)
        {
            var off = Math.Abs(y-sensor.y);
            if (off > dist) throw new ApplicationException(); // too far away
            var delt = dist - off;
            return new Range1D(sensor.x - delt,sensor.x + delt);
        }
    }
}
