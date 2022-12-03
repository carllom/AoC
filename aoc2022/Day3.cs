using common;

namespace aoc2022
{
    [AocDay(3, Caption = "Rucksack Reorganization")]
    internal class Day3
    {
        [AocTask(1)]
        public int Task1() => AocInput.GetLines(3).Sum(r => Common(r.Substring(0, r.Length/2), r.Substring(r.Length/2)).Sum(Priority));

        [AocTask(2)]
        public int Task2()
        {
            var input = AocInput.GetLines(3);
            var sum = 0;
            for (int i = 0; i < input.Length; i+=3)
            {
                sum += Common(Common(input[i], input[i+1]), input[i+2]).Select(Priority).Sum();
            }
            return sum;
        }

        private string Common(string s1, string s2) => new string(s1.Distinct().Where(s2.Contains).ToArray());

        private int Priority(char item) => (char.IsLower(item) ? item - 'a' : item -'A' + 26) + 1;
    }
}
