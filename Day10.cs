namespace aoc2k21
{
    internal class Day10 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile);

            var syntaxPoints = 0;
            foreach (var line in indata)
            {
                var stack = new Stack<char>();

                foreach (var token in line)
                {
                    var result = ParseToken(stack, token);
                    if (!result.ok)
                    {
                        syntaxPoints += result.syntax;
                        break;
                    }
                }
            }
            return syntaxPoints;
        }

        private static (bool ok, int syntax) ParseToken(Stack<char> stack, char token)
        {
            if ("[({<".Contains(token)) { stack.Push(token); return (true,0); }
            var tos = stack.Peek();
            switch (token)
            {
                case ')':
                    if (tos == '(') stack.Pop();
                    else return (false,3);
                    break;
                case ']':
                    if (tos == '[') stack.Pop();
                    else return (false,57);
                    break;
                case '}':
                    if (tos == '{') stack.Pop();
                    else return (false,1197);
                    break;
                case '>':
                    if (tos == '<') stack.Pop();
                    else return (false,25137);
                    break;
                default:
                    throw new Exception($"Unexpected token {token}");
            }
            return (true,0);
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile);

            var acPoints = new List<long>();
            foreach (var line in indata)
            {
                var stack = new Stack<char>();
                var sErr = false;
                foreach (var token in line)
                {
                    var result = ParseToken(stack, token);
                    if (!result.ok) { sErr = true; break; }// skip illegal
                }
                if (sErr) continue;

                long linePts = 0;
                while (stack.Count > 0)
                {
                    linePts *= 5;
                    linePts += stack.Pop() switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        '<' => 4,
                        _ => throw new NotImplementedException()
                    };
                }
                acPoints.Add(linePts);
            }
            acPoints.Sort();
            return acPoints[acPoints.Count() / 2];
        }
    }
}
