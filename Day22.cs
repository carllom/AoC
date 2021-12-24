namespace aoc2k21
{
    internal class Day22 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var steps = ReadCubes(indatafile);

            // Naive "voxel" solution
            var world = new bool[101, 101, 101];
            foreach (var step in steps)
            {
                if (step.min.x < -50 || step.max.x > 50) continue;
                for (var x = step.min.x; x <= step.max.x; x++)
                {
                    if (step.min.y < -50 || step.max.y > 50) continue;
                    for (var y = step.min.y; y <= step.max.y; y++)
                    {
                        if (step.min.z < -50 || step.max.z > 50) continue;
                        for (var z = step.min.z; z <= step.max.z; z++)
                        {
                            world[x+50, y+50, z+50] = step.state; // offset to positive only
                        }
                    }
                }
            }
            var count = 0;
            for (var x = 0; x < world.GetLength(0); x++)
            {
                for (var y = 0; y < world.GetLength(1); y++)
                {
                    for (var z = 0; z < world.GetLength(2); z++)
                    {
                        count += world[x, y, z] ? 1 : 0;
                    }
                }
            }
            return count;
        }

        public long Task2(string indatafile)
        {
            var steps = ReadCubes(indatafile);
            List<Cube> cutCubes = new List<Cube>();
            foreach (var step in steps)
            {
                var intersects = cutCubes.Where(cc => step.Intersects(cc)).ToArray(); // Cubes that intersect the new step
                foreach (var cube in intersects)
                {
                    if (step.Contains(cube))
                    {
                        cutCubes.Remove(cube); // new step contains older cube completely which overwrites it - remove it from list
                    }
                    else
                    {
                        var isec = cube.Subtract(step); // Remove overlapping step from sect
                        if (isec.Any())
                        {
                            cutCubes.Remove(cube);
                            cutCubes.AddRange(isec); // Replace "uncut" cube with cut and bisected cubes
                        }                        
                    }
                }
                if (step.state == true) // No point in adding it if it is "off"
                    cutCubes.Add(step);
            }
            return cutCubes.Sum(c => c.Size);
        }

        private Cube[] ReadCubes(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.Split()).ToArray();
            var cubes = new Cube[indata.Length];
            for (var iidx = 0; iidx < indata.Length; iidx++)
            {
                var c = indata[iidx][1].Split(',').Select(i => i[2..].Split("..").Select(int.Parse).ToArray()).ToArray();
                cubes[iidx] = new Cube(c[0][0], c[0][1], c[1][0], c[1][1], c[2][0], c[2][1], indata[iidx][0] == "on");
            }
            return cubes;
        }

        private struct P3D { 
            public readonly int x;
            public readonly int y;
            public readonly int z;

            public P3D(int x, int y, int z) { this.x=x; this.y=y; this.z=z; }
            public static bool operator <=(P3D a, P3D b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;
            public static bool operator >=(P3D a, P3D b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;
        } 

        private class Cube
        {
            public bool state;
            public readonly P3D min;
            public readonly P3D max;
            public readonly P3D[] corners;
            public Cube(int x0, int x1, int y0, int y1, int z0, int z1, bool state = false) : this(new P3D(x0,y0,z0), new P3D(x1,y1,z1), state) {}

            public Cube(P3D min, P3D max, bool state = false)
            {
                this.min = min;
                this.max = max;
                this.state = state;
                corners = new[]
                {
                    min, new P3D(min.x, min.y, max.z), new P3D(min.x, max.y, min.z), new P3D(min.x, max.y, max.z),
                    new P3D(max.x, min.y, min.z), new P3D(max.x, min.y, max.z), new P3D(max.x, max.y, min.z), max
                };
            }

            public bool Contains(P3D point) => point >= min && point <= max;
            public bool Contains(Cube other) => Contains(other.min) && Contains(other.max);
            public bool Intersects(Cube other) => Intersect(other)?.Size > 0;
            public long Size => (long)(max.x-min.x+1) * (max.y-min.y+1) * (max.z-min.z+1);

            public Cube Intersect(Cube other)
            {
                var xmin = Math.Max(min.x, other.min.x); var xmax = Math.Min(max.x, other.max.x);
                var ymin = Math.Max(min.y, other.min.y); var ymax = Math.Min(max.y, other.max.y);
                var zmin = Math.Max(min.z, other.min.z); var zmax = Math.Min(max.z, other.max.z);
                if (xmin > xmax || ymin > ymax || zmin > zmax) return null;
                return new Cube(xmin, xmax, ymin, ymax, zmin, zmax, state);
            }

            public Cube[] BisectX(int cut) {
                if (cut <= min.x) return new Cube[] { null, this }; // Cube is not bisected. Cut is done on the "lower side" of the index
                if (cut > max.x) return new Cube[] { this, null}; 
                return new Cube[] { new Cube(min.x, cut-1, min.y, max.y, min.z, max.z, state), new Cube(cut, max.x, min.y, max.y, min.z, max.z, state) };
            }
            public Cube[] BisectY(int cut)
            {
                if (cut <= min.y) return new Cube[] { null, this }; // Cube is not bisected. Cut is done on the "lower side" of the index
                if (cut > max.y) return new Cube[] { this, null }; 
                return new Cube[] { new Cube(min.x, max.x, min.y, cut-1, min.z, max.z, state), new Cube(min.x, max.x, cut, max.y, min.z, max.z, state) };
            }
            public Cube[] BisectZ(int cut)
            {
                if (cut <= min.z) return new Cube[] { null, this }; // Cube is not bisected. Cut is done on the "lower side" of the index
                if (cut > max.z) return new Cube[] { this, null };
                return new Cube[] { new Cube(min.x, max.x, min.y, max.y, min.z, cut-1, state), new Cube(min.x, max.x, min.y, max.y, cut, max.z, state) };
            }

            public Cube[] Subtract(Cube cutter)
            {
                var cutQb = cutter.Intersect(this);
                var newQbs = new List<Cube>();
                var bisQb = this;
                for (var crn = 0; crn < cutQb.corners.Length; crn++)
                {
                    var cutp = cutQb.corners[crn];
                    if (!bisQb.Contains(cutp)) continue;

                    var bxLeft = (crn & 4) > 0; // bisect left x slice 
                    var byBottom = (crn & 2) > 0; // bisect top y slice
                    var bzFront = (crn & 1) > 0; // bisect front z slice

                    var bis = new List<Cube>();
                    bis.AddRange(bisQb.BisectX(cutp.x + (bxLeft ? 1 : 0)));
                    bis.AddRange(bis[bxLeft ? 0 : 1].BisectY(cutp.y + (byBottom ? 1 : 0)));
                    bis.AddRange(bis[byBottom ? 2 : 3].BisectZ(cutp.z + (bzFront ? 1 : 0)));
                    bisQb = bis[bzFront ? 4 : 5];
                    newQbs.AddRange(new[] { bis[bxLeft ? 1 : 0], bis[byBottom ? 3 : 2], bis[bzFront ? 5 : 4] });
                }
                return newQbs.Where(q => q != null).ToArray();
            }
        }
    }
}
