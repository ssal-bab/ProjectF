
namespace ProjectF
{
    public static partial class StringUtility
    {
        public static string ColorTag(string color, int value) => $"<color={color}>{value}</color>";
        public static string ColorTag(string color, float value) => $"<color={color}>{value}</color>";
        public static string ColorTag(string color, string value) => $"<color={color}>{value}</color>";

        public static string BoldTag(int value) => $"<b>{value}</b>";
        public static string BolgTag(float value) => $"<b>{value}</b>";
        public static string BoldTag(string value) => $"<b>{value}</b>";
    }
}