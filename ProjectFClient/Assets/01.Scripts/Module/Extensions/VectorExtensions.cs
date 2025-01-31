using UnityEngine;

namespace H00N.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 PlaneVector(this Vector3 origin)
        {
            return new Vector2(origin.x, origin.y);
        }

        public static bool InsideCircle(this Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            return (circleCenter - point).sqrMagnitude <= circleRadius * circleRadius;
        }

        public static float CCW(this Vector2 vector, Vector2 otherStart, Vector2 otherEnd)
        {
            // value > 0 => 반시계
            // value = 0 => 포함
            // value < 0 => 시계
            return (otherEnd.x - otherStart.x) * (vector.y - otherStart.y) - (otherEnd.y - otherStart.y) * (vector.x - otherStart.x);
        }

        public static bool InsideLineSegment(this Vector2 point, Vector2 start, Vector2 end)
        {
            return 
                Mathf.Min(end.x, start.x) <= point.x && point.x <= Mathf.Max(end.x, start.x) &&
                Mathf.Min(end.y, start.y) <= point.y && point.y <= Mathf.Max(end.y, start.y);
        }
    }
}