namespace common
{
    public static class AocInput
    {
        private static string InputDir => Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "input");
        private static string InputPath(int day, int task, bool example) => Path.Combine(InputDir, $"day{day}-{task}{(example ? ".ex" : "")}.input");
        private static string InputPath(int day, bool example) => Path.Combine(InputDir, $"day{day}{(example ? ".ex" : "")}.input");

        /// <summary>
        /// Read input file as lines (when there are separate inputs per task)
        /// </summary>
        /// <param name="day">Day number</param>
        /// <param name="task">Task number</param>
        /// <param name="example">Whether to use example input or not</param>
        /// <returns>Input as array of strings</returns>
        public static string[] GetLines(int day, int task, bool example = false)  => File.ReadAllLines(InputPath(day, task, example));

        /// <summary>
        /// Read input file as lines
        /// </summary>
        /// <param name="day">Day number</param>
        /// <param name="example">Whether to use example input or not</param>
        /// <returns>Input as array of strings</returns>
        public static string[] GetLines(int day, bool example = false) => File.ReadAllLines(InputPath(day, example));

        /// <summary>
        /// Read input file as a string (when there are separate inputs per task)
        /// </summary>
        /// <param name="day">Day number</param>
        /// <param name="task">Task number</param>
        /// <param name="example">Whether to use example input or not</param>
        /// <returns>Input as string</returns>
        public static string GetText(int day, int task, bool example = false) => File.ReadAllText(InputPath(day, task, example));

        /// <summary>
        /// Read input file as string
        /// </summary>
        /// <param name="day">Day number</param>
        /// <param name="example">Whether to use example input or not</param>
        /// <returns>Input as string</returns>
        public static string GetText(int day, bool example = false) => File.ReadAllText(InputPath(day, example));
    }
}
