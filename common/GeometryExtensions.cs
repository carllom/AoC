namespace common
{
    public static class GeometryExtensions
    {
        public static Rectangle Span(this IEnumerable<Point2> points)
        {
            Rectangle span = new(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            foreach (var p in points) 
            { 
                span.x0 = int.Min(span.x0, p.x);
                span.x1 = int.Max(span.x1, p.x);
                span.y0 = int.Min(span.y0, p.y);
                span.y1 = int.Max(span.y1, p.y);
            }
            return span;
        }

        public static Block Span(this IEnumerable<Point3> points)
        {
            Block span = new(int.MaxValue, int.MaxValue, int.MaxValue, int.MinValue, int.MinValue, int.MinValue);
            foreach (var p in points)
            {
                span.x0 = int.Min(span.x0, p.x);
                span.x1 = int.Max(span.x1, p.x);
                span.y0 = int.Min(span.y0, p.y);
                span.y1 = int.Max(span.y1, p.y);
                span.z0 = int.Min(span.z0, p.z);
                span.z1 = int.Max(span.z1, p.z);
            }
            return span;
        }
    }
}
