namespace aoc2021
{
    internal class Day16 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllText(indatafile);
            var (packets, _) = ParsePackets(indata, new Queue<byte>());
            return SumVer(packets);
        }

        private int SumVer(IEnumerable<Packet> packets)
        {
            if (packets == null || packets.Count() == 0) return 0;
            var sum = 0;
            foreach (var packet in packets)
            {
                sum += packet.version;
                sum += SumVer(packet.subPackets);
            }
            return sum;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllText(indatafile);
            var (packets, _) = ParsePackets(indata, new Queue<byte>());
            return EvalExpr(packets.First()); // Only one top packet
        }

        private long EvalExpr(Packet expr)
        {
            return expr.type switch
            {
                0 => expr.subPackets.Sum(p => EvalExpr(p)), // Sum
                1 => expr.subPackets.Aggregate(1L, (a, b) => a * EvalExpr(b)), // Mult
                2 => expr.subPackets.Aggregate(long.MaxValue, (a, b) => Math.Min(a, EvalExpr(b))), // Min
                3 => expr.subPackets.Aggregate(long.MinValue, (a, b) => Math.Max(a, EvalExpr(b))), // Max
                4 => expr.value, // Lit
                5 => EvalExpr(expr.subPackets[0]) > EvalExpr(expr.subPackets[1]) ? 1 : 0, // GT
                6 => EvalExpr(expr.subPackets[0]) < EvalExpr(expr.subPackets[1]) ? 1 : 0, // LT
                7 => EvalExpr(expr.subPackets[0]) == EvalExpr(expr.subPackets[1]) ? 1 : 0, // LT
                _ => throw new Exception()
            };
        }

        private (IEnumerable<Packet> packets, string data) ParsePackets(string data, Queue<byte> bitQueue)
        {
            var p = new Packet();
            var pList = new List<Packet>(new[] { p });
            data = bitQueue.EnqueueAtLeast(6, data); // At least 2 nybbles needed for packet header
            p.version = bitQueue.DequeueByte(3);
            p.type = bitQueue.DequeueByte(3);
            
            if (p.type == 4) // Literal
            {
                data = bitQueue.EnqueueAtLeast(1, data);
                while (bitQueue.DequeueBinary(1) == 1)
                {
                    data = bitQueue.EnqueueAtLeast(5, data);
                    p.value = p.value << 4 | bitQueue.DequeueBinary(4); // Literal nybble
                }
                data = bitQueue.EnqueueAtLeast(5, data);
                p.value = p.value << 4 | bitQueue.DequeueBinary(4); // Last literal nybble
                return (pList, data); // single literal packet
            } 
            else // Operator
            {
                data = bitQueue.EnqueueAtLeast(16, data); // Fill with 16 bits just to be sure. At most we read 4 bits too many
                var flag = bitQueue.DequeueByte(1);
                var length = flag == 1 ? bitQueue.DequeueBinary(11) : bitQueue.DequeueBinary(15); // 1 => 11, 0 => 15 bits
                p.value = length;
                if (flag == 0) // # of subpacket bits 
                {
                    var remBits = data.Length*4 + bitQueue.Count;
                    var expRemBits = remBits - length; // Expected remaining bits after reading (remaining bits before reading - length)
                    while (remBits > expRemBits) 
                    {
                        var subp = ParsePackets(data, bitQueue);
                        p.subPackets.AddRange(subp.packets);
                        data = subp.data;
                        remBits = data.Length*4 + bitQueue.Count;
                    }
                }
                else // # of subpackets
                {
                    for (int nsp = 0; nsp < length; nsp++)
                    {
                        var subp = ParsePackets(data, bitQueue);
                        p.subPackets.AddRange(subp.packets);
                        data = subp.data;
                    }
                }
                return (pList, data);
            }
        }

        private class Packet
        {
            public byte version;
            public byte type;
            public long value; // Literal or number of sub-packets
            public List<Packet> subPackets = new List<Packet>();
        }
    }

    internal static class Day16Ext
    {
        public static string EnqueueAtLeast(this Queue<byte> q, int numbits, string data)
        {
            var i = 0;
            while (q.Count < numbits) q.EnqueueHex(data[i++]); // consume hex nybbles until at least numbits bits are in the queue
            return data[i..];
        }

        private const string Hex = "0123456789ABCDEF";
        private static Queue<byte> EnqueueHex(this Queue<byte> q, char hexnybble)
        {
            var c = Hex.IndexOf(hexnybble); // offset to binary 0..F
            for (int i = 3; i >= 0; i--) q.Enqueue((byte)((c >> i) & 1));
            return q;
        }

        public static long DequeueBinary(this Queue<byte> q, int numBits)
        {
            long result = 0;
            for (int i = 0; i< numBits; i++) result = (result << 1) | q.Dequeue();
            return result;
        }

        public static byte DequeueByte(this Queue<byte> q, int numBits) => numBits < 9 ? (byte)q.DequeueBinary(numBits) : throw new Exception();
    }
}
