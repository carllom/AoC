using System.Text;

namespace aoc2k21
{
    internal class Day24 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var originalProg = File.ReadAllLines(indatafile).Select(l => l).ToArray();
            var deps = GenerateDependencies(originalProg);

            // Generate max value
            var maxSerial = new int[14];
            for (int i = 0; i < 14; i++)
            {
                if (deps[i] == null) // Digit is not dependant on others - find dependencies and maximize
                {
                    var d = deps.FirstOrDefault(d => d.HasValue && d.Value.idx == i);
                    maxSerial[i] = Math.Min(9, 9-d.Value.offset); 
                }
                else // Calculate value from dependency
                {
                    maxSerial[i] = maxSerial[deps[i].Value.idx] + deps[i].Value.offset;
                }
            }
            var asd = Execute(originalProg, maxSerial); // Validate
            return (asd[3] != 0) ? -1 : maxSerial.Aggregate(0L, (acc, i) => acc*10 + i);
        }

        public long Task2(string indatafile)
        {
            var originalProg = File.ReadAllLines(indatafile).Select(l => l).ToArray();
            var deps = GenerateDependencies(originalProg);

            // Generate max value
            var minSerial = new int[14];
            for (int i = 0; i < 14; i++)
            {
                if (deps[i] == null) // Digit is not dependant on others - find dependencies and maximize
                {
                    var d = deps.FirstOrDefault(d => d.HasValue && d.Value.idx == i);
                    minSerial[i] = Math.Max(1, 1-d.Value.offset);
                }
                else // Calculate value from dependency
                {
                    minSerial[i] = minSerial[deps[i].Value.idx] + deps[i].Value.offset;
                }
            }
            var asd = Execute(originalProg, minSerial); // Validate
            return (asd[3] != 0) ? -1 : minSerial.Aggregate(0L, (acc, i) => acc*10 + i);
        }

        private (int idx,int offset)?[] GenerateDependencies(string[] programData)
        {
            // Program is symmetrical in blocks of 18 instructions - blocks differ only in parameters a and b (offset 5 and 15)
            var a_s = new int[14];
            var b_s = new int[14];
            var d_s = new int[14];
            // Extract program parameters
            for (var i = 0; i < programData.Length; i+=18)
            {
                d_s[i/18] = int.Parse(programData[i+4].Split().Last()); // d @ offset 4
                a_s[i/18] = int.Parse(programData[i+5].Split().Last()); // a @ offset 5
                b_s[i/18] = int.Parse(programData[i+15].Split().Last()); // b @ offset 15
            }

            var dependency = new (int idx, int offset)?[14]; // Data dependencies between digits
            var depStack = new Stack<(int idx, int offset)>();
            // Generate data dependencies
            for (var i = 0; i<14; i++)
            {
                if (d_s[i] == 1)
                    depStack.Push((i, b_s[i]));
                else
                {
                    var dep = depStack.Pop();
                    dependency[i] = (dep.idx, dep.offset+a_s[i]);
                }
            }
            return dependency;
        }

        /// <summary>
        /// Reverse-engineered and recreated the algorithm somewhat more readable.
        /// It is not entirely 1:1 functionally compatible with regards to overflow in stack cells, but works as a structural example.
        /// </summary>
        /// <param name="a_s">"A" constants</param>
        /// <param name="b_s">"B" constants</param>
        /// <param name="d_s">"D" z-stack pop toggle, 26 for pop, 1 for no pop</param>
        /// <param name="input">Serial number</param>
        /// <returns>true if serial number is valid</returns>
        private bool MonadAlgorithm(int[] a_s, int[] b_s, int[] d_s, int[] input)
        {
            var w_s = new Queue<int>(input);
            var z_s = new Stack<int>();
            var z = 0;
            for (int i = 0; i < 14; i++)
            {
                var w = w_s.Dequeue();
                var x = z + a_s[i];
                if (d_s[i] == 26) z = z_s.Pop();
                if (w != x)
                {
                    z_s.Push(z);
                    z = w + b_s[i];
                }
            }
            return z == 0 && z_s.Count == 0;
        }

        private const bool STATE_OUTPUT = false; // Print ALU state after every instruction

        private long[] Execute(string[] program, IEnumerable<int> idut)
        {
            var input = new Queue<int>(idut);
            var regs = new long[4]; // w,x,y,z
            foreach (var ins in program)
            {
                Execute(regs, ins, input);
            }
            return regs;
        }

        private void Execute(long[] regs, string instruction, Queue<int> input)
        {
            var tokens = instruction.Split();
            var a = tokens.Length > 1 ? tokens[1][0]-'w' : 0;
            var b = tokens.Length > 2 ?
                char.IsLetter(tokens[2][0]) ? regs[tokens[2][0]-'w'] : int.Parse(tokens[2]) :
                0;
            switch (tokens[0])
            {
                case "inp": regs[a] = input.Dequeue(); if (STATE_OUTPUT) Console.WriteLine();  break;
                case "add": regs[a] += b; break;
                case "mul": regs[a] *= b; break;
                case "div": regs[a] /= b; break;
                case "mod": regs[a] %= b; break;
                case "eql": regs[a] = regs[a] == b ? 1 : 0; break;
                _: break;
            }
            if (STATE_OUTPUT) DumpState(instruction, regs);
        }

        private void DumpState(string instruction, long[] regs)
        {
            var z = regs[3];
            var sb = new StringBuilder("[");
            while (z > 26) { sb.AppendFormat("{0}|", z%26); z/=26; }
            sb.AppendFormat("{0}]", z%26);
            Console.WriteLine($"{instruction,-10} w={regs[0],2}, x={regs[1],2}, y={regs[2],2}, z={regs[3],2}{sb}");
        }
    }
}
