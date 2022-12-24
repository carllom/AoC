using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace common
{
    public class Cell<T>
    {
        public int x;
        public int y;
        public T value;
        public Cell(int x, int y, T value) { this.x = x; this.y = y; this.value = value; }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Cell<T>) return false;
            var o = (Cell<T>)obj;
            return x == o.x && y == o.y;
        }
        public override int GetHashCode() => HashCode.Combine(x, y);
        public int MDist(Cell<T> cell) => Math.Abs(x-cell.x) + Math.Abs(y-cell.y);
        public override string ToString() => $"{value} [{x},{y}]";
    }

    public class Map<T>
    {
        private T[,] map; // [y,x]
        public readonly int width;
        public readonly int height;

        public Map(T[,] map) { this.map = map; width = map.GetLength(1); height = map.GetLength(0); }
        public Map(int width, int height) : this(new T[height, width]) { }
        public Map(T value, int width, int height) : this(width, height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[y, x] = value;
                }
            }
        }
        public Map(T[][] arr) : this(arr[0].Length, arr.Length)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[y, x] = arr[y][x];
                }
            }
        }

        public static Map<T> Copy(Map<T> orig)
        {
            Map<T> copy = new(orig.width, orig.height);
            for (int y = 0; y < orig.height; y++)
            {
                for (int x = 0; x < orig.width; x++)
                {
                    copy.map[y,x] =  orig.map[y,x];
                }
            }
            return copy;
        }
        public static Map<T> Copy(Map<T> orig, Func<T,T> copyfunc)
        {
            var vt = typeof(T).IsValueType;
            Map<T> copy = new(orig.width, orig.height);
            for (int y = 0; y < orig.height; y++)
            {
                for (int x = 0; x < orig.width; x++)
                {
                    copy.map[y, x] = copyfunc(orig.map[y, x]);
                }
            }
            return copy;
        }

        public Rectangle Bounds(Func<T,bool> contains)
        {
            var r = new Rectangle(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var m = map[y, x];
                    if (contains(m))
                    {
                        r.x0 = Math.Min(r.x0, x); r.x1 = Math.Max(r.x1, x);
                        r.y0 = Math.Min(r.y0, y); r.y1 = Math.Max(r.y1, y);
                    }
                }
            }
            return r;
        }

        public long Count(T val, Rectangle? bounds) 
        {
            var b = (bounds == null) ? new Rectangle(0,0, width-1, height-1) : bounds.Value;
            long count = 0; ;
            for (int y = b.y0; y <= b.y1; y++)
            {
                for (int x = b.x0; x <= b.x1; x++)
                {
                    if (map[y, x].Equals(val)) count++;
                }
            }
            return count;
        }

        public Map<T> Grow(T edges)
        {
            var newmap = new Map<T>(width+2, height+2);
            for (int x = 0; x < newmap.width; x++)
            {
                newmap.Set(x, 0, edges); newmap.Set(x, newmap.height-1, edges);
            }
            for (int y = 1; y < newmap.height-1; y++)
            {
                for (int x = 0; x < newmap.width; x++)
                {
                    newmap.Set(x,y,(x==0 || x==newmap.width-1) ? edges : map[y-1,x-1]);
                }
            }
            return newmap;
        }

        public IEnumerable<Cell<T>> Neighbors(Cell<T> cell)
        {
            var n = new List<Cell<T>>();
            if (cell.y > 0) n.Add(new Cell<T>(cell.x, cell.y - 1, map[cell.y - 1, cell.x]));
            if (cell.y + 1 < height) n.Add(new Cell<T>(cell.x, cell.y + 1, map[cell.y + 1, cell.x]));
            if (cell.x > 0) n.Add(new Cell<T>(cell.x - 1, cell.y, map[cell.y, cell.x - 1]));
            if (cell.x + 1 < width) n.Add(new Cell<T>(cell.x + 1, cell.y, map[cell.y, cell.x + 1]));
            return n;
        }

        private bool IsIn(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

        public T? Check(int x, int y) => IsIn(x, y) ? map[y, x] : default;
        public T Get(int x, int y) => map[y, x];
        public T Get(Point2 p) => map[p.y, p.x];
        public void Set(int x, int y, T value) => map[y, x] = value;
        public bool Set2(int x, int y, T value) { if (!IsIn(x, y)) return false; map[y, x] = value; return true; }
        public void Set(Cell<T> cell) => map[cell.y, cell.x] = cell.value;
        public void Set(Rectangle r, T value)
        {
            for (int y = r.y0; y <= r.y1; y++)
            {
                for (int x = r.x0; x <= r.x1; x++)
                {
                    map[y, x] = value;
                }
            }
        }
        public void Set(Rectangle r, Func<int,int,T> factory)
        {
            for (int y = r.y0; y <= r.y1; y++)
            {
                for (int x = r.x0; x <= r.x1; x++)
                {
                    map[y, x] = factory(x,y);
                }
            }
        }
        public Cell<T>? Find(T value)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[y, x]!.Equals(value)) return new Cell<T>(x, y, value);
                }
            }
            return null;
        }
        public IEnumerable<Cell<T>> FindAll(T value)
        {
            var elems = new List<Cell<T>>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[y, x]!.Equals(value)) elems.Add(new Cell<T>(x, y, value));
                }
            }
            return elems;
        }
        public IEnumerable<Cell<T>> FindAll(Func<T,bool> filter)
        {
            var elems = new List<Cell<T>>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (filter(map[y, x])) elems.Add(new Cell<T>(x, y, map[y,x]));
                }
            }
            return elems;
        }

        public string[] Dump()
        {
            var m = new List<string>();
            for (int y = 0; y < height; y++)
            {
                StringBuilder sb = new(width);
                for (int x = 0; x < width; x++) sb.Append(map[y, x]);
                m.Add(sb.ToString());
                sb.Clear();
            }
            return m.ToArray();
        }

        public void Each(Action<int, int, T> act)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    act(x, y, map[y,x]);
                }
            }
        }
    }
}
