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
}
