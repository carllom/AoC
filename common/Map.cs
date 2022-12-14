using System.Diagnostics.CodeAnalysis;
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
        public void Set(int x, int y, T value) => map[y, x] = value;
        public bool Set2(int x, int y, T value) { if (!IsIn(x, y)) return false; map[y, x] = value; return true; }
        public void Set(Cell<T> cell) => map[cell.y, cell.x] = cell.value;
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
    }
}
