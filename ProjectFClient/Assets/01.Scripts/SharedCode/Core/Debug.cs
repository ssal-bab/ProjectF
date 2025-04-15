namespace H00N
{
    public static partial class Debug
    {
        public static partial void Log(string message);
        public static partial void LogError(string message);
        public static partial void LogWarning(string message);

        public static partial void Log(object message);
        public static partial void LogError(object message);
        public static partial void LogWarning(object message);
    }
}