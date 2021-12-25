namespace aoc2k21
{
    internal class Day24 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var originalProg = File.ReadAllLines(indatafile).Select(l => l).ToArray();

            // Program is symmetrical in blocks of 18 instructions - blocks differ only in parameters a and b (offset 5 and 15)
            var a_s = new int[14];
            var b_s = new int[14];
            for (var i = 0; i < originalProg.Length; i+=18)
            {
                a_s[i/18] = int.Parse(originalProg[i+5].Split().Last()); // a @ offset 5
                b_s[i/18] = int.Parse(originalProg[i+15].Split().Last()); // b @ offset 15
            }

            var z = 0;
            for (var j=a_s.Length-1; j>=0; j--)
            {
                var zexp = z%26+a_s[j];
                var znext = new int[9];
                for (var w = 1; w<10; w++)
                {
                    znext[w-1] = z*(w != zexp ? 26 : 1) + (w != zexp ? w+b_s[j] : 0); // Block 1 is kind of the reverse of block 1
                    // z is multiplied by its later modulo if w does not match modulo+a, otherwise z is kept
                    // so, if w == modZ+a => z = z*1 + 0 (z is kept as-is) (that will never happen, will it?
                    //     if w != modZ+a => z*= 26 + w+b (new zmod is w+b)
                    // in reverse: for z to end on 0
                    //  zmod_prev must be either w-a (and z=0)
                    // or 
                }

                var w1 = z-b_s[j]; // w != zexp  (w+b)
                var w2 = z-a_s[j]; // w == zexp  (0)
            }






            var test = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            Execute(originalProg, test);

            var lowserial = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            var highserial = new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
            var counter = 0;
            var hack = (string[])originalProg.Clone();
            for (int i=0; i< hack.Length; i++)
            {
                var patched = hack[i];
                hack[i] = ""; // Patch instruction
                var hackSerial = (int[])lowserial.Clone();
                while (hackSerial[^1]<9)
                {
                    var oRegs = Execute(originalProg, lowserial);
                    var hRegs = Execute(hack, lowserial);
                    if (oRegs[0]!=hRegs[0] || oRegs[1]!=hRegs[1] || oRegs[2]!=hRegs[2] || oRegs[3]!=hRegs[3])
                    {
                        hack[i] = patched; // Patch gave difference - restore instruction
                        break;
                    }
                    Hackrement(hackSerial);
                }
            }


            foreach (var line in hack.Where(i => i.Length > 0)) Console.WriteLine(line);


            return lowserial.Aggregate(0L, (acc, d) => acc*10+d);
        }

        private void Hackrement(int[] serial)
        {
            for (int i = 0; i < serial.Length; i++)
            {
                if (serial[i] == 9) continue;
                serial[i]++;
                if (serial[i] < 10) break;
            }
        }

        private void Increment(int[] serial)
        {
            for (int i = serial.Length-1; i >= 0; i--)
            {
                serial[i]++;
                if (serial[i] < 10) break;
                serial[i]=1;
            }
        }

        private void Decrement(int[] serial)
        {
            for (int i = serial.Length-1; i >= 0; i--)
            {
                serial[i]--;
                if (serial[i] > 0) break;
                serial[i]=9;
            }
        }

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
                case "inp": regs[a] = input.Dequeue(); break;
                case "add": regs[a] += b; break;
                case "mul": regs[a] *= b; break;
                case "div": regs[a] /= b; break;
                case "mod": regs[a] %= b; break;
                case "eql": regs[a] = regs[a] == b ? 1:0; break;
                _ : break;
            }
        }


        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l).ToArray();
            return 0;
        }
    }
}
