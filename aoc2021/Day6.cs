namespace aoc2021
{
    internal class Day6 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var school = File.ReadAllText(indatafile).Split(',').Select(int.Parse).ToList();

            // Naive solution - keep track of each fish
            for (int d = 0; d < 80; d++)
            {
                var newborn = new List<int>();
                for (int i = 0; i < school.Count; i++) // for each lanternfish
                {
                    if (school[i] > 0)
                        school[i]--; // count down reproductive cycle
                    else
                    {
                        newborn.Add(8); // newborn lanternfish
                        school[i] = 6; // reset reproductive cycle
                    }
                }
                school.AddRange(newborn);
            }
            return school.Count;
        }

        public long Task2(string indatafile)
        {
            var day0 = File.ReadAllText(indatafile).Split(',').Select(long.Parse).ToList();

            // # of fish per step in reproductive cycle
            var cycles = new long[9];

            // load initial state
            foreach (var item in day0)
            {
                cycles[item]++;
            }
            
            for (int d = 0; d < 256; d++)
            {
                var nextday = new long[9];

                for (int j = 1; j < 9; j++) // for each "age group"
                {
                    nextday[j-1] = cycles[j]; // count down reproductive cycle
                }
                nextday[8] = cycles[0]; // newborn lanternfish
                nextday[6]+= cycles[0]; // reset reproductive cycle
                cycles = nextday;
            }
            return cycles.Sum();
        }

        // Alternative, reduced allocation
        public long Task2B(string indatafile)
        {
            var day0 = File.ReadAllText(indatafile).Split(',').Select(long.Parse).ToList();

            // # of fish per step in reproductive cycle
            var cycles = new long[9];

            // load initial state
            foreach (var item in day0)
            {
                cycles[item]++;
            }

            for (int d = 0; d < 256; d++)
            {
                var cycle0 = cycles[0]; // save step 0 (will reset to 6)

                for (int j = 1; j < 9; j++) // for each "age group"
                {
                    cycles[j-1] = cycles[j]; // count down reproductive cycle
                }
                cycles[8] = cycle0; // newborn lanternfish
                cycles[6]+= cycle0; // reset reproductive cycle
            }
            return cycles.Sum();
        }
    }
}
