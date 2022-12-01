using System.Diagnostics;
using System.Reflection;

namespace common
{
    public class AocTaskEnumerator
    {
        private Dictionary<int, (string, Type)> tasks = new Dictionary<int, (string, Type)>();

        public AocTaskEnumerator(Assembly taskAssembly)
        {
            foreach (var t in taskAssembly.GetTypes())
            {
                var aocday = t.GetCustomAttribute<AocDayAttribute>();
                if (aocday == null) continue;
                tasks.Add(aocday.Day, (aocday.Caption, t));
            }
        }

        public void Execute()
        {
            foreach (var day in tasks.Keys.OrderBy(d => d))
            {
                Execute(day);
            }
        }

        public void Execute(int day)
        {
            var (caption, aoctype) = tasks[day];

            var ctor = aoctype.GetConstructor(Array.Empty<Type>());
            if (ctor == null) throw new ApplicationException("AoC 'Day' class must have empty constructor");
            var aocday = ctor.Invoke(null);

            var taskmethods = aoctype
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.GetParameters().Length == 0 && m.GetCustomAttribute<AocTaskAttribute>() != null)
                .Select(m => new { m.GetCustomAttribute<AocTaskAttribute>()!.TaskNumber, MethodInfo = m })
                .OrderBy(tm => tm.TaskNumber);

            Console.WriteLine($"--- Day {day}: {caption} ---");
            foreach (var taskmethod in taskmethods)
            {
                Console.Write($"Executing task {taskmethod.TaskNumber}...");
                Stopwatch s = new();
                s.Start();
                var result = taskmethod.MethodInfo.Invoke(aocday, null);
                s.Stop();
                
                Console.WriteLine($"done. Answer: {result} [Duration: {s.ElapsedMilliseconds}ms]");
            }
            Console.WriteLine();
        }
    }
}
