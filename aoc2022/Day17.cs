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
            var input = AocInput.GetText(17,false);

            var shaft = new List<string>();
            shaft.Add("+-------+");
            var row = "|.......|";
            for (int i = 0; i<3; i++) { shaft.Add(row); } ;

            int shapeidx = 0, shapeY = shaft.Count, windex = 0, height = 1, shapecount = 0;
            string[]? shape = null;
            while (shapecount < 2022)
            {
                if (shape == null) { // Select shape and add to shaft
                    while (shaft.Count > height+4) shaft.RemoveAt(shaft.Count-1); // truncate shaft
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
                        }
                        height = Math.Max(height, shapeY+shape.Length-1); //height = shaft.Count(k => k.Contains('#'));
                        shape = null;
                        shapecount++;

                        //for (int i = shaft.Count-1; i >=0; i--)
                        //{
                        //    Console.WriteLine(shaft[i]);
                        //}
                        //Console.WriteLine($"top {height}");
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
                //Console.WriteLine($"Used wind {input[windex]}({windex}) (stuck: {stuck})");
                if (!stuck)
                {
                    for (int i = 0; i < shifted.Count; i++)
                    {
                        shaft[shapeY+i] = shifted[i];
                    }
                }
                windex = (windex+1) % input.Length;
            }
            return shaft.Count(k=> k.Contains('#'));
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
        public int Task2()
        {
            var input = AocInput.GetText(17, true);

            var shaft = new List<string>
            {
                "+-------+"
            };
            var row = "|.......|";
            for (int i = 0; i<3; i++) { shaft.Add(row); };

            var winds = input.Length;


            int shapeidx = 0, shapeY = shaft.Count, windex = 0, height = 1, shapecount = 0;

            bool first = true;
            int shape0 = 0, windex0 = 0;

            string[]? shape = null;
            while (true)//shapecount < 2022)
            {
                if (shape == null)
                { // Select shape and add to shaft
                    while (shaft.Count > height+4) shaft.RemoveAt(shaft.Count-1); // truncate shaft
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
                        }
                        height = Math.Max(height, shapeY+shape.Length-1);// shaft.Count(k => k.Contains('#'));
                        shape = null;
                        shapecount++;

                        //for (int i = shaft.Count-1; i >=0; i--)
                        //{
                        //    Console.WriteLine(shaft[i]);
                        //}
                        if (shapecount%1000 == 0)
                            Console.WriteLine($"count {shapecount} top {height}, item {shapecount%4} windindex {windex}");
                        if (first)
                        {
                            shape0 = shapecount%4;
                            windex0 = windex;
                        }
                        else if (shape0 == shapecount%4 && windex0==windex)
                        {
                            Console.WriteLine(height);
                        }
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
                //Console.WriteLine($"Used wind {input[windex]}({windex}) (stuck: {stuck})");
                if (!stuck)
                {
                    for (int i = 0; i < shifted.Count; i++)
                    {
                        shaft[shapeY+i] = shifted[i];
                    }
                }
                windex = (windex+1) % input.Length;
            }
            return shaft.Count(k => k.Contains('#'));
        }
    }
}
