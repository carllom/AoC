using System.Diagnostics.CodeAnalysis;

namespace common
{
    public struct Point2
    {
        public int x, y;
        public Point2(int x, int y) { this.x = x; this.y = y; }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Point2) return false;
            var o = (Point2)obj;
            return x==o.x && y == o.y;
        }
        public override int GetHashCode() => HashCode.Combine(x, y);
        public static Point2 operator -(Point2 a, Point2 b) => new(a.x-b.x, a.y-b.y);
        public override string ToString() => $"[{x},{y}]";
        public int ManhDist(Point2 other) => Math.Abs(x-other.x) + Math.Abs(y-other.y);
    }

    public struct Rectangle
    {
        public int x0, y0, x1, y1;
        public int width => x1-x0;
        public int height => y1-y0;

        public Point2 p0 => new Point2(x0, y0);
        public Point2 p1 => new Point2(x1, y1);
        public Rectangle(int x0, int y0, int x1, int y1) { this.x0 = x0; this.x1 = x1; this.y0 = y0; this.y1 = y1; }
        public Rectangle(Point2 p0, int width, int height) { x0 = p0.x; y0 = p0.y; x1 = x0+width; y1 = y0+height; }
        public Rectangle(Point2 p0, Point2 p1) { x0 = p0.x; y0 = p0.y; x1 = p1.x; y1 = p1.y; }
    }

    public struct Range1D
    {
        public int from;
        public int to;
        public Range1D(int from, int to) { this.from = from; this.to = to; }


        public bool Contains(Range1D other) => from <= other.from && to >= other.to;
        public bool Overlaps(Range1D other) => other.from <= to && other.to >= from;

        public Range1D[] Subtract(Range1D other)
        {
            if (!Overlaps(other)) return new[] { this };
            if (other.Contains(this)) return new Range1D[0];

            if (other.from > from && other.to < to) return new Range1D[] { new(from, other.from-1), new(other.to+1, to) }; // Cut in the middle

            if (other.from < from) // Cut left side
            {
                return new[] { new Range1D(Math.Max(from, other.to+1), to) };
            }
            else // cut right side
            {
                return new[] { new Range1D(from, Math.Min(to, other.from-1)) };
            }

        }

        public Range1D[] Add(Range1D other)
        {
            //if (Contains(other)) return new[] { this };
            if (!Overlaps(other)) return new [] { this, other };
            return new[] { new Range1D(Math.Min(from, other.from), Math.Max(to, other.to)) };
        }

        public int Size => to - from;
    }
}
