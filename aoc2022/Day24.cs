using common;

namespace aoc2022
{
    [AocDay(24, Caption = "Blizzard Basin")]
    internal class Day24
    {
        [AocTask(1)]
        public int Task1()
        {
            var map = ReadMap();
            Point2 target = new(map.width-2, map.height-1); // goal pos
            map.Get(1, 0).dude = 0; // Initial pos

            map = Travel(map, target);
            return map.Get(target.x, target.y).dude;
        }

        [AocTask(2)]
        public int Task2()
        {
            var map = ReadMap();
            Point2 start = new(1, 0), end = new(map.width-2, map.height-1);
            map.Get(start).dude = 0; // Initial pos

            map = Travel(map, end);
            map.Each((x, y, b) => { if (x!=end.x || y != end.y) b.dude = int.MaxValue; }); // Clear all dudes except the winning
            map = Travel(map, start);
            map.Each((x, y, b) => { if (x!=start.x || y != start.y) b.dude = int.MaxValue; });
            map = Travel(map, end);
            return map.Get(end).dude;
        }

        private Map<Bliz> ReadMap()
        {
            var input = AocInput.GetLines(24, false);
            var map = new Map<Bliz>(input[0].Length, input.Length);
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    int state = input[y][x] switch
                    {
                        '.' => 0,
                        '^' => 1,
                        'v' => 2,
                        '>' => 4,
                        '<' => 8,
                        '#' => 16
                    };
                    map.Set(x, y, new Bliz(state));
                }
            }
            return map;
        }

        private Map<Bliz> Travel(Map<Bliz> map, Point2 target)
        {
            while (true)
            {
                var newmap = Tick(map);
                foreach (var c in map.FindAll(b => b.dude != int.MaxValue)) // Find all cells in old map with dudes in them
                {
                    foreach (var n in newmap.Neighbors(c))
                    {
                        if (!n.value.Open || n.value.dude <= c.value.dude+1) continue; // Crushed by blizzard or another dude bested me
                        n.value.dude = c.value.dude + 1; // Its open and I have the least moves
                    }
                    var nc = newmap.Get(c.x, c.y); // Old position in new map
                    if (nc.Open && nc.dude > c.value.dude+1) nc.dude = c.value.dude+1; // wait in last position
                }
                if (newmap.Get(target).dude < int.MaxValue)
                    return newmap;
                map = newmap;
            }
        }

        private Map<Bliz> Tick(Map<Bliz> map)
        {
            Map<Bliz> newMap = Map<Bliz>.Copy(map);
            newMap.Set(1, 0, new Bliz()); newMap.Set(newMap.width-2, newMap.height-1, new Bliz()); // Clear start/goal positions
            newMap.Set(new Rectangle(1, 1, newMap.width-2, newMap.height-2), (int x, int y) => new Bliz());
            for (int y = 1; y < map.height-1; y++)
            {
                for (int x = 1; x < map.width-1; x++)
                {
                    var b = map.Get(x, y);
                    if (b.Up) { var c = newMap.Get(x, ((y-2+newMap.height-2)%(newMap.height-2))+1); c.Up = true; b.Up = false; }
                    if (b.Down) { var c = newMap.Get(x, (y%(newMap.height-2))+1); c.Down = true; b.Down = false; }
                    if (b.Left) { var c = newMap.Get(((x-2+newMap.width-2)%(newMap.width-2))+1, y); c.Left = true; b.Left = false; }
                    if (b.Right) { var c = newMap.Get((x%(newMap.width-2))+1, y); c.Right = true; b.Right = false; }
                }
            }
            return newMap;
        }
    }

    public class Bliz
    {
        public bool Open => state == 0;
        public bool Wall { get => state>=16; set => state = value ? state | 16 : state & 0x0F; }
        public bool Up { get => (state & 1) > 0; set => state = value ? state | 1 : state & 0xE; }
        public bool Down { get => (state & 2) > 0; set => state = value ? state | 2 : state & 0xD; }
        public bool Right { get => (state & 4) > 0; set => state = value ? state | 4 : state & 0xB; }
        public bool Left { get => (state & 8) > 0; set => state = value ? state | 8 : state & 0x7; }
        private int state = 0;
        private static readonly string[] chars = { ".","^","v","2",">","2","2","3","<","2","2","3","2","3","3","4" };
        public override string ToString() => dude < int.MaxValue ? "E" : state >= 16 ? "#" : chars[state];
        public Bliz(int state) { this.state = state; }
        public Bliz() { }

        public int dude = int.MaxValue;
    }
}
