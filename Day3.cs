namespace aoc2k21
{
    internal class Day3 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile);
            var result = new int[indata[0].Length];
            foreach (var line in indata)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    result[i] += line[i] == '1' ? 1 : 0;
                }
            }
            result = result.Reverse().ToArray();
            var half = indata.Length / 2;
            long gamma = 0;
            for (int j = 0; j < result.Length; j++)
            {
                gamma +=  (result[j]> half ? 1 : 0) << j;
            }
            var epsilon = ~gamma & (1 << result.Length) - 1; // epsilon is bitwise negated gamma (masked to epsilon bitwidth)

            return gamma * epsilon;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile);
            var half = indata.Length / 2;
            var bitcounts = new int[indata[0].Length];
            foreach (var line in indata)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    bitcounts[i] += line[i] == '1' ? 1 : 0;
                }
            }

            IEnumerable<string> o2values = indata;
            for (int j = 0; j < bitcounts.Length; j++)
            {
                if (o2values.Count() <= 1) break;
                var match = o2values.Count(v => v[j] == '1') >= o2values.Count(v => v[j] == '0') ? '1' : '0';
                o2values = o2values.Where(v => v[j] == match).ToList(); // ToList forces expression evaluation + enumeration
            }

            IEnumerable<string> co2values = indata;
            for (int j = 0; j < bitcounts.Length; j++)
            {
                if (co2values.Count() <= 1) break;
                var match = co2values.Count(v => v[j] == '1') < co2values.Count(v => v[j] == '0') ? '1' : '0';
                co2values = co2values.Where(v => v[j] == match).ToList(); // ToList forces expression evaluation + enumeration
            }

            var o2value = Convert.ToInt64(o2values.First(),2);
            var co2value = Convert.ToInt64(co2values.First(), 2);
            return o2value * co2value;
        }
    }

    public static class Ext
    {
        public static string Streverse(this string s)
        {
            var arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
