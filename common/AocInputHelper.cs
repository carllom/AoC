namespace common
{
    public static class AocInput
    {
        private static string InputDir => Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "input");
        private static string InputPath(int day, int task) => Path.Combine(InputDir, $"day{day}-{task}.input");
        private static string InputPath(int day) => Path.Combine(InputDir, $"day{day}.input");

        public static string[] GetLines(int day, int task)  => File.ReadAllLines(InputPath(day, task));
        public static string[] GetLines(int day) => File.ReadAllLines(InputPath(day));
        public static string GetText(int day, int task) => File.ReadAllText(InputPath(day, task));
        public static string GetText(int day) => File.ReadAllText(InputPath(day));
    }
}
