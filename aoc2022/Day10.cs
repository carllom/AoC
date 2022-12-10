using common;
using System.Text;

namespace aoc2022
{
    [AocDay(10, Caption = "Cathode-Ray Tube")]
    internal class Day10
    {
        [AocTask(1)]
        public int Task1()
        {
            var input = AocInput.GetLines(10).Select(l => l.Split());
            int x = 1, cycle = 0, result = 0;
            foreach (var ins in input)
            {
                if (++cycle % 40 == 20) result += cycle*x; // x = {20,60,100..} is equivalent to x%40==20. pre-increment is important b/c we evaluate *after* the cycle
                if (ins[0] == "addx")
                {
                    if (++cycle % 40 == 20) result += cycle*x; // addx takes an extra cycle - the one above is shared by noop/addx
                    x+=int.Parse(ins[1]);
                }
            }
            return result;
        }

        [AocTask(2)]
        public string Task2()
        {
            var input = AocInput.GetLines(10).Select(l => l.Split());
            int xPos = 1, cycle = 0;
            const int width = 40;
            var line = new StringBuilder(41);
            var screen = new StringBuilder(Environment.NewLine);
            foreach (var ins in input)
            {
                line.Append(Math.Abs(xPos -(cycle++ % width)) < 2 ? '#' : ' '); // if beam position distance to sprite center is 0..1 we set the pixel
                if (ins[0] == "addx")
                {
                    line.Append(Math.Abs(xPos -(cycle++ % width)) < 2 ? '#' : ' ');
                    xPos+=int.Parse(ins[1]);
                }
                if (line.Length >= 40) // we have completed a line - add it to the result
                {
                    screen.AppendLine(line.ToString(0, 40));
                    line.Remove(0, 40); // consume the pixels we have added
                }
            }
            return screen.ToString();
        }
    }
}
