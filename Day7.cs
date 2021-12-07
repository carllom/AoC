namespace aoc2k21
{
    internal class Day7 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var crabs = File.ReadAllText(indatafile).Split(',').Select(int.Parse).ToArray();

            var mincost = int.MaxValue;
            for (int alignpos = crabs.Min(); alignpos <= crabs.Max(); alignpos++)
            {
                var cost = crabs.Select(crabpos => Math.Abs(crabpos-alignpos)).Sum();
                if (cost < mincost) mincost = cost;
            }

            return mincost;
        }

        // Naive solution - takes a couple of seconds
        public long Task2A(string indatafile)
        {
            var crabs = File.ReadAllText(indatafile).Split(',').Select(int.Parse).ToArray();
            var mincost = int.MaxValue;
            for (int alignpos = crabs.Min(); alignpos <= crabs.Max(); alignpos++)
            {
                var cost = crabs.Select(crabpos => Enumerable.Range(1, Math.Abs(crabpos-alignpos)).Sum()).Sum();
                if (cost < mincost) mincost = cost;
            }
            return mincost;
        }

        // Better solution with precalculated costs
        public long Task2(string indatafile)
        {
            var crabs = File.ReadAllText(indatafile).Split(',').Select(int.Parse).ToArray();
            var mincost = int.MaxValue;
            var maxdist = crabs.Max() - crabs.Min();
            var costs = Enumerable.Range(0, maxdist+1).ToArray(); // [0,1..maxdist] array
            for (int i = 1; i < costs.Length; i++) costs[i] = costs[i-1]+costs[i]; // Pre-calculate costs

            for (int alignpos = crabs.Min(); alignpos <= crabs.Max(); alignpos++)
            {
                var cost = crabs.Select(crabpos => costs[Math.Abs(crabpos-alignpos)]).Sum();
                if (cost < mincost) mincost = cost;
            }
            return mincost;
        }
    }
}