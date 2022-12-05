using common;

namespace aoc2022
{
    [AocDay(5, Caption = "Supply Stacks")]
    internal class Day5
    {
        private Stack<char>[] ReadStacks(string[] lines)
        {
            lines = lines.Reverse().ToArray();
            var stacks = new Stack<char>[lines[0].Split(' ',StringSplitOptions.RemoveEmptyEntries).Length];
            for (int i = 0; i < stacks.Length; i++) stacks[i] = new Stack<char>();
            foreach (var line in lines.AsSpan(1..))
            {
                var startAt = 0;
                while (true) {
                    var idx = line.IndexOf('[', startAt);
                    if (idx < 0) break;
                    stacks[idx/4].Push(line[idx+1]);
                    startAt = idx+3;
                }
            }
            return stacks;
        }

        private string ExecuteTask(Action<Stack<char>[], int[]> moveOp)
        {
            var input0 = AocInput.GetLines(5, false);
            var stacks = ReadStacks(input0.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray());
            var moves = input0.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1).Select(l => l.Split()).Select(s => new int[] { int.Parse(s[1]), int.Parse(s[3])-1, int.Parse(s[5])-1 });
            foreach (var m in moves) moveOp(stacks, m);
            return new string(stacks.Select(s => s.Pop()).ToArray());
        }

        [AocTask(1)]
        public string Task1() => ExecuteTask((stacks, op) => { for (int i = 0; i < op[0]; i++) stacks[op[2]].Push(stacks[op[1]].Pop()); });

        [AocTask(2)]
        public string Task2() => ExecuteTask((stacks, op) => {
                var tmp = new Stack<char>();
                for (int i = 0; i < op[0]; i++) tmp.Push(stacks[op[1]].Pop());
                for (int i = 0; i < op[0]; i++) stacks[op[2]].Push(tmp.Pop());
            });
    }
}