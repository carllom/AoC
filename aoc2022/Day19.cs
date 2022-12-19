using common;

namespace aoc2022
{
    [AocDay(19, Caption = "Not Enough Minerals")]
    internal class Day19
    {
        [AocTask(1)]
        public int Task1()
        {
            // 0: ore-robot-cost,
            // 1: clay-robot-cost
            // 2: obsidian-robot-cost-ore 3: obsidian-robot-cost-clay
            // 4: geode-robot-cost-ore 5: geode-robot-cost-obsidian
            var input = AocInput.GetLines(19).Select(l => l.Split(new char[] { ':','.',' ' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(r => new[] { r[6], r[12], r[18], r[21], r[27], r[30] })
                .Select(r => r.Select(int.Parse).ToArray()).ToList();

            var quality = 0;
            for (int i = 0; i < input.Count; i++)
            {
                var data = new OreData(input[i]);
                quality += (i+1) * OptimizeGeode(new List<OreData>() { data }, 24);
            }
            return quality;
        }

        [AocTask(2)]
        public int Task2()
        {
            var input = AocInput.GetLines(19).Select(l => l.Split(new char[] { ':', '.', ' ' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(r => new[] { r[6], r[12], r[18], r[21], r[27], r[30] })
                .Select(r => r.Select(int.Parse).ToArray()).ToList();

            var maxgeodes = new int[3];
            for (int i = 0; i < 3; i++)
            {
                var data = new OreData(input[i]);
                maxgeodes[i] = OptimizeGeode(new List<OreData>() { data }, 32);
            }
            return maxgeodes[0]*maxgeodes[1]*maxgeodes[2];
        }

        // Search breadth-first
        private int OptimizeGeode(List<OreData> data, int minutes)
        {
            if (minutes == 0) // No time left - how many geodes do we have at most?
            {
                var max = data.Max(o => o.stash[3]);
                data = data.Where(o => o.stash[3] == max).ToList();
                return max;
            }

            var georob = new List<OreData>();
            var boughtgeo = false;
            var newrob = new List<OreData>();
            foreach (var od in data)
            {
                if (od.CanBuild(3) == 0) // Always prefer to buy geode immediately
                {
                    od.Tick();
                    od.Buy(3);
                    georob.Add(od);
                    boughtgeo = true;
                    continue;
                }
                else if (!boughtgeo) // No point in trying for other robots if we bought a geode robot this round
                {
                    for (int i = 2; i >= 0; i--) // Try to buy robots, prioritizing obsidian first, then clay, then ore
                    {
                        if (od.CanBuild(i) == 0 && od.CouldBuild(i) > 0) // Build if you can, but only if you could not do it last time (this is the most significant optimization)
                        {
                            var newData = new OreData(od);
                            newData.Tick();
                            newData.Buy(i);
                            newrob.Add(newData);
                        }
                    }
                }
                od.Tick();
            }

            if (georob.Any()) return OptimizeGeode(georob, minutes - 1);
            data.AddRange(newrob);
            return OptimizeGeode(data, minutes - 1);
        }
    }

    public class OreData
    {
        // 0: ore, 1: clay, 2: obs, 3:geode
        public int[] stash = new int[4]; // mineral quantities
        public int[] generators = new int[4]; // # of robots for each kind
        public int[][] costs = new int[4][]; // cost matrix

        public OreData(int[] recipe)
        {
            for (int i = 0; i < costs.Length; i++) costs[i] = new int[4];
            costs[0][0] = recipe[0]; // ore-robot-cost
            costs[1][0] = recipe[1]; // clay-robot-cost
            costs[2][0] = recipe[2]; // obsidian-robot-cost-ore
            costs[2][1] = recipe[3]; // obsidian-robot-cost-clay
            costs[3][0] = recipe[4]; // geode-robot-cost-ore
            costs[3][2] = recipe[5]; // geode-robot-cost-obsidian
            generators[0] = 1; // start with 1 ore-collecting robot
        }

        public OreData(OreData od) // Clone
        {
            for (int i = 0; i < 4; i++)
            {
                stash[i]=od.stash[i];
                generators[i]=od.generators[i];
            }
            costs = od.costs; // ref-copy costs as they are read-only
        }

        public int CanBuild(int type) // How long until you can build a robot of "type" with current generators?
        {
            int[] cost = costs[type];
            var waittime = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (cost[i] == 0) continue;
                if (generators[i] == 0) return -i;
                waittime[i] = Convert.ToInt32(Math.Ceiling(Math.Max(0d, cost[i] - stash[i]) / generators[i]));
            }
            return waittime.Max();
        }

        public int CouldBuild(int type) // Could you build it last step already?
        {
            int[] cost = costs[type];
            var waittime = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (cost[i] == 0) continue;
                if (generators[i] == 0) return -i;
                waittime[i] = Convert.ToInt32(Math.Ceiling(Math.Max(0d, cost[i] - (stash[i]-generators[i])) / generators[i]));
            }
            return waittime.Max();
        }

        public void Tick() 
        {
            for (int i = 0; i < 4; i++) stash[i] += generators[i];
        }

        public void Buy(int type)
        {
            for (int i = 0; i < 4; i++)
            {
                stash[i] -= costs[type][i];
                if (stash[i] < 0) throw new ApplicationException("No credit!!");
            }
            generators[type]++;
        }
    }
}