using System.Diagnostics.CodeAnalysis;

namespace common
{
    public struct Point2
    {
        public int x, y;
        public Point2(int x, int y) { this.x = x; this.y = y; }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is Point2)) return false;
            var o = (Point2)obj;
            return x==o.x && y == o.y;
        }
        public override int GetHashCode() => HashCode.Combine(x, y);
        public static Point2 operator -(Point2 a, Point2 b) => new(a.x-b.x, a.y-b.y);
        public override string ToString() => $"[{x},{y}]";
    }
}
