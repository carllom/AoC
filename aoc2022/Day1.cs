using common;

namespace aoc2022
{
    [AocDay(1, Caption = "Calorie Counting")]
    internal class Day1
    {
        [AocTask(1)]
        public int Task1()
        {
            return Calories().Max();
        }

        [AocTask(2)]
        public int Task2()
        {
            return Calories().OrderByDescending(c => c).Take(3).Sum();
        }

        private IEnumerable<int> Calories()
        {
            var l = new List<int>();
            var count = 0;
            foreach (var cals in AocInput.GetLines(1))
            {
                if (cals == string.Empty)
                {
                    l.Add(count);
                    count = 0;
                }
                else
                {
                    count += int.Parse(cals);
                }
            }
            return l;
        }
    }
}
