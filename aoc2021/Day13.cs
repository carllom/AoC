namespace aoc2021
{
    internal class Day13 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var map = ReadMap(indatafile);
            var fold = ReadFolds(indatafile).First();
            map = fold.axis switch
            {
                'x' => FoldX(map,fold.coord),
                'y' => FoldY(map,fold.coord)
            };
            return Count(map);
        }


        public long Task2(string indatafile)
        {
            var map = ReadMap(indatafile);
            var folds = ReadFolds(indatafile);
            foreach (var fold in folds)
            {
                map = fold.axis switch
                {
                    'x' => FoldX(map, fold.coord),
                    'y' => FoldY(map, fold.coord)
                };
            }
            Dump(map);
            return Count(map);
        }

        private int Count(bool[,] map)
        {
            var count = 0;
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y]) count++;
                }
            }
            return count;
        }

        private void Dump(bool[,] map)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x, y] ? '#' : '.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private bool[,] ReadMap(string indata)
        {
            var coords = new List<int[]>();
            var l = "";
            using (var f = File.OpenText(indata))
            {
                while ((l = f.ReadLine())?.Length > 0)
                {
                    coords.Add(l.Split(',').Select(int.Parse).ToArray());
                }
                var xsize = coords.Max(c => c[0])+1; if (xsize % 2 == 0) xsize++;
                var ysize = coords.Max(c => c[1])+1; if (ysize % 2 == 0) ysize++;
                var map = new bool[xsize, ysize];
                foreach (var c in coords)
                {
                    map[c[0], c[1]] = true;
                }
                return map;
            }
        }

        private IEnumerable<(char axis, int coord)> ReadFolds(string indata)
        {
            using (var f = File.OpenText(indata))
            {
                while (f.ReadLine()?.Length > 0) { }
                var folds = new List<(char axis, int coord)>();
                var l = "";
                while ((l = f.ReadLine())?.Length > 0)
                {
                    var fold = l[(l.LastIndexOf('=')-1)..].Split('=');
                    folds.Add((fold[0][0], int.Parse(fold[1])));
                }
                return folds;
            }
        }

        private bool[,] FoldX(bool[,] map, int foldAt)
        {
            var map2 = new bool[foldAt, map.GetLength(1)];
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < foldAt; x++)
                {
                    if (2*foldAt-x > map.GetUpperBound(0)) continue; // Outside
                    map2[x, y] = map[x, y] || map[foldAt*2-x, y];
                }
            }
            return map2;
        }

        private bool[,] FoldY(bool[,] map, int foldAt)
        {
            var map2 = new bool[map.GetLength(0), foldAt];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < foldAt; y++)
                {
                    if (2*foldAt-y > map.GetUpperBound(1)) continue; // Outside
                    map2[x, y] = map[x, y] || map[x, foldAt*2 -y];
                }
            }
            return map2;
        }
    }
}
