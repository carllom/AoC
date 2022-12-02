using common;
using System.Linq;

namespace aoc2022
{
    [AocDay(2, Caption = "Rock Paper Scissors")]
    internal class Day2
    {
        private static readonly Dictionary<string, int> outcomes = new Dictionary<string, int>
        {
            { "A X", 1+3 },{ "B X", 1+0 },{ "C X", 1+6 },
            { "A Y", 2+6 },{ "B Y", 2+3 },{ "C Y", 2+0 },
            { "A Z", 3+0 },{ "B Z", 3+6 },{ "C Z", 3+3 }
        };

        [AocTask(1)]
        public int Task1() => AocInput.GetLines(2).Sum(r => outcomes[r]);

        private static readonly Dictionary<string, int> outcomes2 = new Dictionary<string, int>
        {
            { "A X", 3+0 },{ "B X", 1+0 },{ "C X", 2+0 },
            { "A Y", 1+3 },{ "B Y", 2+3 },{ "C Y", 3+3 },
            { "A Z", 2+6 },{ "B Z", 3+6 },{ "C Z", 1+6 }
        };

        [AocTask(2)]
        public int Task2() => AocInput.GetLines(2).Sum(r => outcomes2[r]);
    }
}
