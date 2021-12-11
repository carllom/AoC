using System.Text;

namespace aoc2k21
{
    internal class Day11 : IAocTask
    {
        public long Task1(string indatafile)
        {
            int[][] octomap = File.ReadAllLines(indatafile).Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
            var flashCount = 0;
            for (int step = 0; step < 100; step++)
            {
                for (int y = 0; y < octomap.Length; y++) // Increase energy for current step
                {
                    for (int x = 0; x < octomap[0].Length; x++)
                    {
                        octomap[y][x]+=1;
                    }
                }
                flashCount += Flash(octomap);
                //Console.WriteLine($"After step {step+1}:"); Print(octomap);
            }


            return flashCount;
        }

        public long Task2(string indatafile)
        {
            int[][] octomap = File.ReadAllLines(indatafile).Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
            var ylen = octomap.Length;
            var xlen = octomap[0].Length;
            var flashCount = 0;
            var step = 0;
            while (flashCount < xlen*ylen)
            {
                step++;
                // Increase energy for current step
                for (int y = 0; y < ylen; y++)
                {
                    for (int x = 0; x < xlen; x++)
                    {
                        octomap[y][x]+=1;
                    }
                }
                flashCount = Flash(octomap);
                //Console.WriteLine($"After step {step}:"); Print(octomap);
            }
            return step;
        }

        private int Flash(int[][] octomap)
        {
            var ylen = octomap.Length;
            var xlen = octomap[0].Length;
            var flashed = 0;
            for (int y = 0; y < ylen; y++)
            {
                for (int x = 0; x < xlen; x++)
                {
                    if (octomap[y][x] > 9)
                    {
                        octomap[y][x] = 0; // Flash
                        flashed++;

                        for (int yd = -1; yd < 2; yd++) // iterate over neighbours (3x3 block centered on x,y)
                        {
                            for (int xd = -1; xd < 2; xd++)
                            {
                                if (xd == 0 && yd == 0 || x+xd < 0 || y+yd < 0 || x+xd >= xlen || y+yd >= ylen) continue; // skip center and outside of map
                                if (octomap[y+yd][x+xd] == 0) continue; // Already flashed this step
                                octomap[y+yd][x+xd]+=1; // Energize neighbour
                            }
                        }
                    }
                }
            }
            if (flashed > 0) flashed += Flash(octomap); // Recurse on flash energized neighbours
            return flashed;
        }

        private void Print(int[][] octomap) => Array.ForEach(octomap, line => Console.WriteLine(line.Aggregate(new StringBuilder(line.Length), (sb, i) => i == 0 ? sb.Append('.') : i >= 10 ? sb.Append('F') : sb.Append(i)).ToString()));
    }
}
