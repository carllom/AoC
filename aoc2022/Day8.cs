using common;

namespace aoc2022
{
    [AocDay(8, Caption = "Treetop Tree House")]
    internal class Day8
    {
        [AocTask(1)]
        public int Task1()
        {
            var ipt = AocInput.GetLines(8);
            var visible = (ipt.Length-2)*2 + ipt[0].Length * 2; // edges always visible
            for (int y = 1; y < ipt.Length-1; y++)
            {
                for (int x = 1; x < ipt[0].Length-1; x++)
                {
                    var found = true;
                    for (int l = 0; l < x; l++) // scan left (direction does not matter since it is "any")
                    {
                        if (ipt[y][l] >= ipt[y][x]) { found = false; break; }; // any >= our lookout point ? 
                    }
                    if (found) { visible++; continue; } // if we are visible in any direction we can check next
                    found = true;
                    for (int r = x+1; r < ipt[y].Length; r++) // scan right
                    {
                        if (ipt[y][r] >= ipt[y][x]) { found = false; break; };
                    }
                    if (found) { visible++; continue; }
                    found = true;
                    for (int u = 0; u < y; u++) // scan up
                    {
                        if (ipt[u][x] >= ipt[y][x]) { found = false; break; };
                    }
                    if (found) { visible++; continue; }
                    found = true;
                    for (int d = y+1; d < ipt.Length; d++) // scan down
                    {
                        if (ipt[d][x] >= ipt[y][x]) { found = false; break; };
                    }
                    if (found) { visible++; }
                }
            }
            return visible;
        }

        [AocTask(2)]
        public int Task2()
        {
            var ipt = AocInput.GetLines(8);
            var maxScenic = 0;
            for (int y = 1; y < ipt.Length-1; y++)
            {
                for (int x = 1; x < ipt[0].Length-1; x++)
                {
                    int scrU = 0, scrD = 0, scrL = 0, scrR = 0;
                    for (int l = x-1; l >= 0; l--) // scan left
                    {
                        scrL++; // add to score
                        if (ipt[y][l] >= ipt[y][x]) break;
                    }
                    for (int r = x+1; r < ipt[y].Length; r++) // scan right
                    {
                        scrR++;
                        if (ipt[y][r] >= ipt[y][x]) break;
                    }
                    for (int u = y-1; u >= 0; u--) // scan up
                    {
                        scrU++;
                        if (ipt[u][x] >= ipt[y][x]) break;
                    }
                    for (int d = y+1; d < ipt.Length; d++) // scan down
                    {
                        scrD++;
                        if (ipt[d][x] >= ipt[y][x]) break;
                    }
                    var scenic = scrL*scrR*scrU*scrD;
                    if (scenic > maxScenic) { maxScenic = scenic; }
                }
            }
            return maxScenic;
        }
    }
}