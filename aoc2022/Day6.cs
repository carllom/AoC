using common;

namespace aoc2022
{
    [AocDay(6, Caption = "Tuning Trouble")]
    internal class Day6
    {
        [AocTask(1)]
        public int Task1() => FindDistinct(AocInput.GetText(6), 4);

        [AocTask(2)]
        public int Task2() => FindDistinct(AocInput.GetText(6), 14);

        private int FindDistinct(string data, int size)
        {
            for (int i = 0; i < data.Length - size; i++)
            {
                if (data.Skip(i).Take(size).Distinct().Count() == size) return i + size; // Wasteful but elegant
            }
            return int.MinValue;
        }
    }
}
