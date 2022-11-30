using System.Diagnostics;

namespace aoc2021
{
    internal class Day19 : IAocTask
    {
        private Point3[] offsets = Array.Empty<Point3>(); // Offsets are needed for Task#2 - cannot be bothered to recalculate them.

        public long Task1(string indatafile)
        {
            var indata = new Queue<(Point3[] points, int id)>(ReadScanners(indatafile)); // Scanner clouds are pre-sorted
            offsets = new Point3[indata.Count];
            Point3[] totCloud = indata.Dequeue().points;
            offsets[0] = Point3.Zero; // Scanner 0 is @ (0,0,0)

            while (indata.Any())
            {
                var scCloud = indata.Dequeue();
                Point3? matchingOffset = null;

                Console.Write($"Checking scanner #{scCloud.id}.. ");
                for (int rotation = 0; rotation < 24; rotation++)
                {
                    var nCloud = Sort(Rotate(scCloud.points, rotation)).ToArray(); // "Next cloud" rotated and sorted
                    var offListX = GetOffsets(totCloud, nCloud, p => p.X);

                    foreach (var off in offListX)
                    {
                        var pairs = new List<(Point3 p0, Point3 pn)>();
                        var offsetsY = new Dictionary<int, List<(Point3, Point3)>>();
                        foreach (var newP in nCloud)
                        {
                            foreach (var x in totCloud) 
                            {
                                if (x.X != newP.X + off) continue;
                                var p = (x, newP);
                                pairs.Add(p);

                                var offy = x.Y - newP.Y;
                                if (!offsetsY.ContainsKey(offy)) offsetsY[offy] = new List<(Point3 p0, Point3 pn)>(new[] { p });
                                else offsetsY[offy].Add(p);
                            }
                        }
                        if (pairs.Count >= 12)
                        {
                            if (offsetsY.Count > 1)
                            {
                                pairs = offsetsY.Values.OrderByDescending(o => o.Count).First();
                            }
                            if (pairs.Count < 12) continue; // This was not a matching offset combination

                            var offsetsZ = new Dictionary<int, List<(Point3, Point3)>>();
                            foreach (var pair in pairs)
                            {
                                var offz = pair.p0.Z - pair.pn.Z;
                                if (!offsetsZ.ContainsKey(offz)) offsetsY[offz] = new List<(Point3, Point3)>(new[] { pair });
                                else offsetsZ[offz].Add(pair);
                            }
                            if (offsetsZ.Count > 1)
                            {
                                pairs = offsetsZ.Values.OrderByDescending(o => o.Count).First();
                            }
                            if (pairs.Count < 12) continue; // This was not a matching offset combination

                            // pairs now contain the filtered point pair set having the same offsets for all axis
                            matchingOffset = pairs[0].p0 - pairs[0].pn;
                            break;
                        }
                    }

                    if (matchingOffset != null) // found a match
                    {
                        var normalized = nCloud.Select(p => p + matchingOffset); // Normalize to offsets from (0,0,0)
                        totCloud = totCloud.Concat(normalized).Distinct().ToArray(); // Merge the points
                        offsets[scCloud.id] = matchingOffset;
                        break; // Try next cloud in queue
                    }
                }
                if (matchingOffset != null)
                {
                    Console.WriteLine($"found offset @ {matchingOffset}. Beacon count is now {totCloud.Length}");
                }
                else 
                {
                    indata.Enqueue(scCloud); // Scanner cloud does not overlap reinsert into queue and try later
                    Console.WriteLine("failed");
                }
            }
            return totCloud.Length;
        }

        private HashSet<int> GetOffsets(Point3[] a, Point3[] b, Func<Point3,int> axis)
        {
            var offList = new HashSet<int>();
            foreach (var ap in a)
            {
                foreach (var bp in b) offList.Add(axis(ap) - axis(bp));
            }
            return offList;
        }

        private IEnumerable<Point3> Rotate(IEnumerable<Point3> points, int rComb) => points.Select(p => p.Rotate(rComb));
        private IEnumerable<Point3> Sort(IEnumerable<Point3> points) => points.OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Z);

        public long Task2(string indatafile)
        {
            var max = 0;
            for (int i = 0; i < offsets.Length - 1; i++)
            {
                for (int j = i; j < offsets.Length; j++)
                {
                    max = Math.Max(max, offsets[i].Distance(offsets[j]));
                }
            }
            return max;
        }

        private (Point3[] points, int id)[] ReadScanners(string filename)
        {
            StreamReader fs = File.OpenText(filename) ?? throw new Exception();
            List<(Point3[],int)> scanners = new List<(Point3[],int)>();
            var id = 0;
            while(!fs.EndOfStream)
            {
                var name = fs.ReadLine();
                var scanner = new List<Point3>();
                string? l;
                while ((l = fs.ReadLine())?.Length > 0) scanner.Add(Point3.Parse(l));
                scanners.Add((Sort(scanner).ToArray(), id++));
            }
            return scanners.ToArray();
        }
    }

    public class Point3
    {
        public static readonly Point3 Zero = new Point3(0, 0, 0);
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        private Point3(int x,int y, int z) { X = x; Y = y; Z = z; }
        private Point3(int[] coords) { X=coords[0]; Y=coords[1]; Z=coords[2]; }
        public static Point3 Parse(string data) => new Point3(data.Split(',').Select(int.Parse).ToArray());

        public override string ToString() => $"({X,4},{Y,4},{Z,4})";

        public override int GetHashCode() => X+Y+Z;

        public override bool Equals(object? obj)
        {
            var other = obj as Point3;
            if (other == null) return false;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public Point3 Rotate(int rComb)
        {
            Debug.Assert(rComb>=0 && rComb<24, "Valid range for rotation combination is [0..24]");
            return new Point3(vecmul(rots[rComb], this));
        }
        public int Distance(Point3 other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
        public static Point3 operator -(Point3 a, Point3 b) => new Point3(new[] { a.X-b.X, a.Y-b.Y, a.Z-b.Z });
        public static Point3 operator +(Point3 a, Point3 b) => new Point3(new[] { a.X+b.X, a.Y+b.Y, a.Z+b.Z });

        // Rotation algebra
        private static int[][][] rots = new int[24][][];
        private static readonly int[] ortsin = { 0, 1, 0, -1 };
        private static readonly int[] ortcos = { 1, 0, -1, 0 };
        private static int[][] rotx(int a) => new int[][] { new int[] { 1, 0, 0 }, new int[] { 0, ortcos[a], -ortsin[a] }, new int[] { 0, ortsin[a], ortcos[a] } };
        private static int[][] roty(int a) => new int[][] { new int[] { ortcos[a], 0, ortsin[a] }, new int[] { 0, 1, 0 }, new int[] { -ortsin[a], 0, ortcos[a] } };
        private static int[][] rotz(int a) => new int[][] { new int[] { ortcos[a], -ortsin[a], 0 }, new int[] { ortsin[a], ortcos[a], 0 }, new int[] { 0, 0, 1 } };
        private static int[][] mtxmul(int[][] a, int[][] b) => new int[][] { // a[i][n]*b[n][j]
                new int[] { a[0][0]*b[0][0] + a[0][1]*b[1][0] + a[0][2]*b[2][0], a[0][0]*b[0][1] + a[0][1]*b[1][1] + a[0][2]*b[2][1], a[0][0]*b[0][2] + a[0][1]*b[1][2] + a[0][2]*b[2][2] },
                new int[] { a[1][0]*b[0][0] + a[1][1]*b[1][0] + a[1][2]*b[2][0], a[1][0]*b[0][1] + a[1][1]*b[1][1] + a[1][2]*b[2][1], a[1][0]*b[0][2] + a[1][1]*b[1][2] + a[1][2]*b[2][2] },
                new int[] { a[2][0]*b[0][0] + a[2][1]*b[1][0] + a[2][2]*b[2][0], a[2][0]*b[0][1] + a[2][1]*b[1][1] + a[2][2]*b[2][1], a[2][0]*b[0][2] + a[2][1]*b[1][2] + a[2][2]*b[2][2] },
            };
        private static int[] vecmul(int[][] a, Point3 p) => new int[] { a[0][0]*p.X + a[0][1]*p.Y + a[0][2]*p.Z, a[1][0]*p.X + a[1][1]*p.Y + a[1][2]*p.Z, a[2][0]*p.X + a[2][1]*p.Y + a[2][2]*p.Z };

        static Point3()
        {
            for (int i = 0; i < 24; i++)
            {
                var (face, rot) = Math.DivRem(i, 4);
                var m = face switch
                {
                    4 => rotx(1), // bottom
                    5 => rotx(3), // top
                    _ => roty(face) // front, right, back, left
                };
                rots[i] = mtxmul(rotz(rot), m); // 0,90,180,270 (clockwise)
            }
        }
    }
}