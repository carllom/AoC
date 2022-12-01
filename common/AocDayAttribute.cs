namespace common
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AocDayAttribute : Attribute
    {
        public int Day { get; set; }
        public string Caption { get; set; } = string.Empty;

        public AocDayAttribute(int day)
        {
            Day=day;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AocTaskAttribute : Attribute
    {
        public int TaskNumber { get; set; }
        public AocTaskAttribute(int taskNumber)
        {
            TaskNumber=taskNumber;
        }
    }
}