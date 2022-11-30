namespace aoc2021
{
    internal class Day25 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var map = File.ReadAllLines(indatafile).Select(l => l.ToCharArray()).ToArray();

            var moved = true;
            int step = 0, sizeX = map[0].Length, sizeY = map.Length;
            while (!(step%2==0 && !moved))
            {
                if (step%2 == 0)
                {
                    moved=false;
                }
                var next = new string[sizeY].Select(r => new string('.', sizeX).ToCharArray()).ToArray();
                
                for (int y = 0; y <map.Length; y++)
                {
                    for (int x = 0; x <map[y].Length; x++)
                    {
                        var sc = map[y][x];
                        if (step%2 == 0 && sc == '>' && map[y][(x+1)%sizeX] == '.')
                        {
                            next[y][(x+1)%sizeX] = '>';
                            moved = true;
                        }
                        else if (step%2 == 1 && sc == 'v' && map[(y+1)%sizeY][x] == '.')
                        {
                            next[(y+1)%sizeY][x] = 'v';
                            moved = true;
                        }
                        else if (map[y][x] != '.' && next[y][x] == '.')
                        {
                            next[y][x] = map[y][x];
                        }
                    }
                }
                map = next;
                step++;
            }
            //foreach (var l in map) { Console.WriteLine(new String(l)); }
            return step/2;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l).ToArray();
            return 0;
        }
    }
}
