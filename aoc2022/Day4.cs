using common;

namespace aoc2022
{
    [AocDay(4, Caption = "Camp Cleanup")]
    internal class Day4
    {
        [AocTask(1)]
        public int Task1() => AocInput.GetLines(4).Select(l => l.Split(',', '-').Select(int.Parse).ToArray()).Where(l => (l[0] >= l[2] & l[1] <=l[3]) || (l[2] >= l[0] & l[3] <=l[1])).Count();

        [AocTask(2)]
        public int Task2() => AocInput.GetLines(4).Select(l => l.Split(',', '-').Select(int.Parse).ToArray()).Where(l => (l[0] >= l[2] & l[0] <=l[3]) || (l[2] >= l[0] & l[2] <=l[1]) || (l[1] >= l[2] & l[1] <=l[3]) || (l[3] >= l[0] & l[3] <=l[1])).Count();
    }
}
