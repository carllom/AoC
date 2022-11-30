namespace aoc2021
{
    internal class Day9 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

            var totrisk = 0;
            for (int y = 0; y < indata.Length; y++)
            {
                for (int x = 0; x < indata[0].Length; x++)
                {
                    var p = indata[y][x];
                    var lowest = true;
                    if (y > 0) lowest &= p < indata[y - 1][x];
                    if (y < indata.Length - 1) lowest &= p < indata[y + 1][x];
                    if (x > 0) lowest &= p < indata[y][x - 1];
                    if (x < indata[0].Length - 1) lowest &= p < indata[y][x + 1];
                    if (lowest)
                        totrisk += 1+p;
                }
            }

            return totrisk;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
            var map = new (int h, bool basin)[indata.Length, indata[0].Length];
            for (int y = 0; y < indata.Length; y++)
            {
                for (int x = 0; x < indata[0].Length; x++)
                {
                    map[y, x] = (indata[y][x], false);
                }
            }

            var totrisk = 0;
            var ymax = map.GetUpperBound(0);
            var xmax = map.GetUpperBound(1);
            var basins = new List<int>();
            for (int y = 0; y <= ymax; y++)
            {
                for (int x = 0; x <= xmax; x++)
                {
                    var p = map[y,x].h;
                    var lowest = true;
                    if (y > 0) lowest &= p < map[y - 1,x].h;
                    if (y < ymax) lowest &= p < map[y + 1,x].h;
                    if (x > 0) lowest &= p < map[y,x - 1].h;
                    if (x < xmax) lowest &= p < map[y,x + 1].h;
                    if (lowest)
                    {
                        var cmap = ((int h, bool basin)[,])map.Clone();
                        cmap[y, x].basin = true;
                        Basin(cmap);
                        var bcount = 0;
                        for (int yc = 0; yc <= ymax; yc++)
                        {
                            for (int xc = 0; xc <= xmax; xc++)
                            {
                                if (cmap[yc, xc].basin) bcount++;
                            }
                        }
                        basins.Add(bcount);

                    }
                }
            }

            return basins.OrderByDescending(b => b).Take(3).Aggregate(1, (a, b) => a * b);
        }

        private void Basin((int h, bool basin)[,] map)
        {
            var grew = false;
            var ymax = map.GetUpperBound(0);
            var xmax = map.GetUpperBound(1);
            for (int y = 0; y <= ymax; y++)
            {
                for (int x = 0; x <= xmax; x++)
                {
                    var p = map[y, x].h;
                    if (p == 9 || map[y, x].basin) continue;
                    if (y > 0 && map[y - 1, x].basin ||
                        y < ymax && map[y + 1, x].basin ||
                        x > 0 && map[y, x - 1].basin ||
                        x < xmax && map[y, x + 1].basin)
                    {
                        map[y, x].basin = true;
                        grew = true;
                    }
                }
            }
            if (grew) Basin(map);
        }
    }
}
