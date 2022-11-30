namespace aoc2021
{
    internal class Day1 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var depths = File.ReadAllLines(indatafile).Select(l => int.Parse(l)).ToArray();
            int previous = depths[0];
            int numDeeper = 0;
            foreach (var depth in depths)
            {
                if (depth > previous)
                    numDeeper++;
                previous = depth;
            }
            return numDeeper;
        }

        public long Task2(string indatafile)
        {
            var depths = File.ReadAllLines(indatafile).Select(l => int.Parse(l)).ToArray();
            var previous = depths[0]+depths[1]+depths[2];
            var numDeeper = 0;
            for (int i = 3; i < depths.Length; i++)
            {
                var depth = depths[i]+depths[i-1]+depths[i-2];
                if (depth > previous)
                    numDeeper++;
                previous=depth;
            }
            return numDeeper;
        }
    }
}
