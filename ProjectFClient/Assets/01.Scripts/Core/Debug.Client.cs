namespace H00N
{
    public static partial class Debug
    {
        public static partial void Log(string message)
        {
            UnityEngine.Debug.Log(message);            
        }

        public static partial void LogError(string message)
        {
            UnityEngine.Debug.LogError(message);            
        }
        
        public static partial void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public static partial void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        public static partial void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);            
        }
        
        public static partial void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
    }
}