namespace aoc2k21
{
    internal class Day2 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.Split()).ToArray();

            var depth = 0;
            var horiz = 0;
            foreach (var item in indata)
            {
                var amount = int.Parse(item[1]);
                switch (item[0])
                {
                    case "up":
                        depth -= amount;
                        break;
                    case "down":
                        depth += amount;
                        break;
                    case "forward":
                        horiz += amount;
                        break;
                }
            }
            return depth*horiz;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.Split()).ToArray();

            var depth = 0;
            var horiz = 0;
            var aim = 0;
            foreach (var item in indata)
            {
                var amount = int.Parse(item[1]);
                switch (item[0])
                {
                    case "up":
                        aim -= amount;
                        break;
                    case "down":
                        aim += amount;
                        break;
                    case "forward":
                        horiz += amount;
                        depth += aim * amount;
                        break;
                }
            }
            return depth*horiz;
        }
    }
}
