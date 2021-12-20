using System.Text;

namespace aoc2k21
{
    internal class Day20 : IAocTask
    {
        public long Task1(string indatafile) => Task(indatafile, 2);
        public long Task2(string indatafile) => Task(indatafile, 50);

        private int Task(string indatafile, int count)
        {
            var reader = File.OpenText(indatafile);
            var enhancer = reader.ReadLine().ToCharArray(); ;
            reader.ReadLine();
            var image = new List<string>();
            while (!reader.EndOfStream) { image.Add(reader.ReadLine()); }

            char outside = '.'; // start with dark outside
            for (int i = 0; i < count; i++)
            {
                image = EnhanceImage(enhancer, Grow(image, outside), outside);
                outside = enhancer[outside == '#' ? 511 : 0]; // Calculate new outside value (all pixels are 1s (511) or 0s (0))
            }
            return image.Sum(l => l.ToCharArray().Aggregate(0, (acc, c) => acc + (c == '#' ? 1 : 0)));
        }

        private List<string> EnhanceImage(char[] enhancer, List<string> image, char outside)
        {
            var img2 = new List<string>();
            for (int y = 0; y < image.Count; y++)
            {
                var sb = new StringBuilder(image[y].Length);
                for (int x = 0; x < image[y].Length; x++) sb.Append(enhancer[EnhancerIndex(image, x, y, outside)]);
                img2.Add(sb.ToString());
            }
            return img2;
        }

        private int EnhancerIndex(List<string> image, int x, int y, char outside)
        {
            int index = 0;
            for (var yp = y-1; yp < y+2; yp++)
            {
                for (var xp = x-1; xp < x+2; xp++)
                {
                    if (xp < 0 || yp < 0 || yp >= image.Count || xp >= image[yp].Length) index = index << 1 | (outside == '#' ? 1 : 0); // coordinate is outside input bitmap - use "outside" value
                    else index = index << 1 | (image[yp][xp] == '#' ? 1 : 0);
                }
            }
            return index;
        }

        public List<string> Grow(List<string> image, char outside)
        {
            var result = new List<string>();
            result.Add(new string(outside, image[0].Length + 2));
            for (var y = 0; y < image.Count; y++) result.Add(outside + image[y] + outside);
            result.Add(new string(outside, image[0].Length + 2));
            return result;
        }
    }
}
