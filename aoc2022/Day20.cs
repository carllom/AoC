using common;
using Microsoft.Win32.SafeHandles;
using System.Xml.Serialization;

namespace aoc2022
{
    [AocDay(20, Caption = "Grove Positioning System")]
    internal class Day20
    {
        [AocTask(1)]
        public long Task1() => GetCoordinate(Swizzle(CreateChain(AocInput.GetLines(20).Select(long.Parse).ToArray())));

        private Link[] CreateChain(long[] input)
        {
            var index = new Link[input.Length];
            index[0] = new Link(0, input, null);
            Link prev = index[0];
            for (int i = 1; i < input.Length; i++)
            {
                index[i] = new Link(i, input, prev);
                prev = index[i];
            }
            prev.next = index[0];
            index[0].prev = prev;
            return index;
        }
        private Link[] Swizzle(Link[] input)
        {
            for (int i = 0; i < input.Length; i++) input[i].Move((int)input[i].modval);
            return input;
        }
        private long GetCoordinate(Link[] input)
        {
            var l = input[0].FindValue(0);
            var result = 0L;
            for (int i = 0; i < 3; i++)
            {
                l = l.FindFwd(1000);
                result+= l.value;
            }
            return result;
        }

        [AocTask(2)]
        public long Task2()
        {
            var input = AocInput.GetLines(20, false).Select(long.Parse).ToArray();
            for (int i = 0; i < input.Length; i++) input[i] = input[i]*811589153;
            var chain = CreateChain(input);
            for (int i = 0; i < 10; i++) chain = Swizzle(chain);
            return GetCoordinate(chain);
        }

        private class Link
        {
            public long value;
            public long modval;
            public Link next;
            public Link prev;
            public Link(int idx, long[] input, Link prev)
            {
                value=input[idx];
                modval = Math.Abs(value)%(input.Length-1);
                if (value < 0) modval = -modval;
                this.prev = prev;
                if (prev != null) prev.next = this;
            }
            public void MoveRight(int steps)
            {
                for (int i = 0; i<steps; i++)
                {
                    var newnext = next.next;
                    next.next = this;
                    next.prev = prev;
                    prev = next;
                    next = newnext;
                    next.prev = this;
                    prev.prev.next = prev;
                }
            }
            public void MoveLeft(int steps)
            {
                for (int i = 0; i<steps; i++)
                {
                    var newprev = prev.prev;
                    prev.prev = this;
                    prev.next = next;
                    next = prev;
                    prev = newprev;
                    prev.next = this;
                    next.next.prev = next;
                }
            }
            public void Move(int steps) { if (steps < 0) MoveLeft(-steps); else MoveRight(steps); }
            public Link FindFwd(int steps)
            {
                if (steps < 0) throw new ArgumentException("must be positive", nameof(steps));
                var l = this;
                while (steps-- > 0) l = l.next;
                return l;
            }
            public Link FindValue(long value)
            {
                if (this.value == value) return this;
                var l = next; 
                while (l.value != value && l != this) l = l.next;
                if (l == this) return null;
                return l;
            }
        }
    }
}
