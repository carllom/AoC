using common;
using System.Text;

namespace aoc2022
{
    [AocDay(17, Caption = "")]
    internal class Day17
    {
        [AocTask(1)]
        public int Task1()
        {
            var input = AocInput.GetText(17);

            var shaft = new List<string>();
            shaft.Add("+-------+");
            var row = "|.......|";
            for (int i = 0; i<3; i++) { shaft.Add(row); } ;

            int heightStart = 0;
            var blockers = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

            int shapeidx = 0, shapeY = shaft.Count, windex = 0, height = 1, shapecount = 0;
            string[]? shape = null;
            while (shapecount < 2022)
            {
                if (shape == null) { // Select shape and add to shaft
                    while (shaft.Count > height-heightStart+4) shaft.RemoveAt(shaft.Count-1); // truncate shaft
                    shape = shapes[shapeidx]; shapeidx = (shapeidx+1) % shapes.Length;
                    shapeY = shaft.Count;
                    for (int i = 0; i < shape.Length; i++) shaft.Add(shape[i]);
                }
                else
                {
                    var done = false;
                    var merges = new List<string>();
                    for (int i = 0; i < shape.Length; i++) // Move down
                    {
                        var mrg = Merge(shaft[shapeY+i], shaft[shapeY-1+i]);
                        if (mrg == null) { done = true; break; }
                        merges.Add(mrg);
                    }
                    if (done) // Could not move - convert falling to fixed
                    {
                        for (int i = 0; i < shape.Length; i++)
                        {
                            shaft[shapeY+i] = shaft[shapeY+i].Replace('@', '#');
                            var currHeight = heightStart+shapeY+i;
                            for (int j = 0; j < 7; j++)
                            {
                                if (shaft[shapeY+i][j+1] == '#' && blockers[j] < currHeight) blockers[j] = currHeight;
                            }
                        }
                        height = Math.Max(height, heightStart+shapeY+shape.Length-1);
                        shape = null;
                        shapecount++;

                        var minlevel = int.Max(0,blockers.Min() -3); // We cannot remove upto minlevel entirely. There might be a diagonal passage left.
                        if (minlevel > heightStart)
                        {
                            shaft.RemoveRange(1, minlevel-heightStart);
                            heightStart = minlevel;
                        }
                        //for (int i = shaft.Count-1; i >=0; i--) Console.WriteLine($"{shaft[i]} {heightStart+i}");
                        //Console.WriteLine($"top {height}, start@ {heightStart}");
                        continue;
                    }
                    else
                    {
                        shapeY--;
                        for (int i = 0; i < merges.Count; i++)
                        {
                            shaft[shapeY+i] = merges[i];
                        }
                        shaft[shapeY+merges.Count] = shaft[shapeY+merges.Count].Replace('@', '.'); // Clear last shape top
                    }
                }

                var shifted = new List<string>();
                var stuck = false;
                for (int i = 0; i < shape.Length; i++) // Apply wind to shape
                {
                    var shift = Shift(shaft[shapeY+i], input[windex]=='>');
                    if (shift.Equals(shaft[shapeY+i])) { stuck = true; break; }
                    shifted.Add(shift);
                }
                if (!stuck)
                {
                    for (int i = 0; i < shifted.Count; i++)
                    {
                        shaft[shapeY+i] = shifted[i];
                    }
                }
                windex = (windex+1) % input.Length;
            }
            return height;
        }

        private string[][] shapes = new string[][]
        {
            new string[] { "|..@@@@.|" },
            new string[] { "|...@...|", "|..@@@..|", "|...@...|" },
            new string[] { "|..@@@..|", "|....@..|", "|....@..|" },
            new string[] { "|..@....|", "|..@....|", "|..@....|", "|..@....|" },
            new string[] { "|..@@...|", "|..@@...|" },
        };

        private string Shift(string a, bool right) {
            var sb = new char[a.Length];
            sb[0] = a[0]; sb[a.Length-1] = a[a.Length-1];
            if (right)
            {
                for (int i = a.Length-2; i >= 0; i--)
                {
                    if (sb[i+1] == default) sb[i+1]=a[i+1];
                    if (a[i]!='@') continue;
                    if (sb[i+1]!='.') continue;
                    sb[i+1] = a[i]; sb[i]='.';
                }
            }
            else
            {
                for (int i = 1; i < a.Length; i++)
                {
                    if (sb[i-1] == default) sb[i-1]=a[i-1];
                    if (a[i]!='@') continue;
                    if (sb[i-1]!='.') continue;
                    sb[i-1] = a[i]; sb[i]='.';
                }
            }
            return new string(sb);
        }

        private string? Merge(string a, string b)
        {
            var sb = new StringBuilder(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i]!='@') { sb.Append(b[i]=='@'?'.':b[i]); continue; }
                if ("@.".Contains(b[i])) { sb.Append(a[i]); continue; }
                return null;
            }
            return sb.ToString();
        }

        [AocTask(2)]
        public long Task2()
        {
            var input = AocInput.GetText(17, false);

            var shaft = new List<string>();
            shaft.Add("+-------+");
            var row = "|.......|";
            for (int i = 0; i<3; i++) { shaft.Add(row); };

            long heightStart = 0;
            var blockers = new long[7] { 0, 0, 0, 0, 0, 0, 0 };

            int shapeidx = 0, windex = 0, shapeY = shaft.Count;
            long height = 1, shapecount = 0;
            string[]? shape = null;

            bool warped = false;
            int warpwarmup = 0;
            var windindexlog = new Dictionary<(int winindex, int shape), (long height, long shapecount)>();
            long targetcount = 1000000000000L;

            while (shapecount < targetcount)
            {
                if (shape == null)
                { // Select shape and add to shaft
                    while (shaft.Count > height-heightStart+4) shaft.RemoveAt(shaft.Count-1); // truncate shaft
                    shape = shapes[shapeidx]; shapeidx = (shapeidx+1) % shapes.Length;
                    shapeY = shaft.Count;
                    for (int i = 0; i < shape.Length; i++) shaft.Add(shape[i]);

                    if (!warped)
                    {
                        if (windindexlog.ContainsKey((windex, shapeidx)))
                        {
                            var lastVal = windindexlog[(windex, shapeidx)];
                            windindexlog[(windex, shapeidx)] = (height, shapecount);
                            warpwarmup++;
                            if (warpwarmup >= 4)
                            {
                                // Shortcut!
                                var dHeight = height-lastVal.height;
                                var dShapes = shapecount-lastVal.shapecount;
                                var numPeriods = (targetcount-shapecount) / dShapes;
                                height += numPeriods * dHeight;
                                heightStart += numPeriods * dHeight;
                                shapecount += numPeriods * dShapes;
                                warped = true;
                                if (shapecount == targetcount) return height;
                            }
                        }
                        else
                        {
                            windindexlog.Add((windex, shapeidx), (height, shapecount));
                        }
                    }
                }
                else
                {
                    var done = false;
                    var merges = new List<string>();
                    for (int i = 0; i < shape.Length; i++) // Move down
                    {
                        var mrg = Merge(shaft[shapeY+i], shaft[shapeY-1+i]);
                        if (mrg == null) { done = true; break; }
                        merges.Add(mrg);
                    }
                    if (done) // Could not move - convert falling to fixed
                    {
                        for (int i = 0; i < shape.Length; i++)
                        {
                            shaft[shapeY+i] = shaft[shapeY+i].Replace('@', '#');
                            var currHeight = heightStart+shapeY+i;
                            for (int j = 0; j < 7; j++)
                            {
                                if (shaft[shapeY+i][j+1] == '#' && blockers[j] < currHeight) blockers[j] = currHeight;
                            }
                        }
                        height = Math.Max(height, heightStart+shapeY+shape.Length-1);
                        shape = null;
                        shapecount++;

                        var minlevel = long.Max(0, blockers.Min() -3);
                        if (minlevel > heightStart)
                        {
                            shaft.RemoveRange(1, (int)(minlevel-heightStart));
                            heightStart = minlevel;
                        }
                        //for (int i = shaft.Count-1; i >=0; i--) Console.WriteLine($"{shaft[i]} {heightStart+i}");
                        //Console.WriteLine($"top {height}, start@ {heightStart}");
                        continue;
                    }
                    else
                    {
                        shapeY--;
                        for (int i = 0; i < merges.Count; i++)
                        {
                            shaft[shapeY+i] = merges[i];
                        }
                        shaft[shapeY+merges.Count] = shaft[shapeY+merges.Count].Replace('@', '.'); // Clear last shape top
                    }
                }

                var shifted = new List<string>();
                var stuck = false;
                for (int i = 0; i < shape.Length; i++) // Apply wind to shape
                {
                    var shift = Shift(shaft[shapeY+i], input[windex]=='>');
                    if (shift.Equals(shaft[shapeY+i])) { stuck = true; break; }
                    shifted.Add(shift);
                }
                if (!stuck)
                {
                    for (int i = 0; i < shifted.Count; i++)
                    {
                        shaft[shapeY+i] = shifted[i];
                    }
                }
                windex = (windex+1) % input.Length;
            }
            return height;
        }
    }
}
