using System.Diagnostics.CodeAnalysis;

namespace common
{
    public struct Point3
    {
        public int x, y, z;
        public Point3(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Point3) return false;
            var o = (Point3)obj;
            return x == o.x && y == o.y && z == o.z;
        }
        public override int GetHashCode() => HashCode.Combine(x, y, z);
        public static Point3 operator -(Point3 a, Point3 b) => new(a.x-b.x, a.y-b.y, a.z-b.z);
        public override string ToString() => $"[{x},{y},{z}]";
        public int ManhDist(Point3 other) => Math.Abs(x-other.x) + Math.Abs(y-other.y) + Math.Abs(z-other.z);
    }

    public struct Block
    {
        public int x0, y0, x1, y1, z0, z1;
        public int width => x1-x0;
        public int height => y1-y0;
        public int depth => z1-z0;

        public Point3 p0 => new Point3(x0, y0, z0);
        public Point3 p1 => new Point3(x1, y1, z1);
        public Block(int x0, int y0, int z0, int x1, int y1, int z1) { this.x0 = x0; this.x1 = x1; this.y0 = y0; this.y1 = y1; this.z0 = z0; this.z1 = z1;}
        public Block(Point3 p0, int width, int height, int depth) { x0 = p0.x; y0 = p0.y; z1 = p0.z; x1 = x0+width; y1 = y0+height; z1 = z0+depth; }
        public Block(Point3 p0, Point3 p1) { x0 = p0.x; y0 = p0.y; z0 = p0.z; x1 = p1.x; y1 = p1.y; z1 = p1.z; }
    }

}
