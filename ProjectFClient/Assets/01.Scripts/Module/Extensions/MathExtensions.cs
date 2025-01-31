using UnityEngine;

namespace H00N.Extensions
{
    public static class MathExtensions
    {
        public static bool IntersectWith(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, bool allowIncluding = true)
        {
            float p0CCW = p0.CCW(p2, p3);
            float p1CCW = p1.CCW(p2, p3);
            float p2CCW = p2.CCW(p0, p1);
            float p3CCW = p3.CCW(p0, p1);

            // 선분 교차
            if(p0CCW * p1CCW < 0 && p2CCW * p3CCW < 0)
                return true;

            // 포함을 허용하지 않을 경우 교차가 아니라면 return false
            if(allowIncluding == false)
                return false;

            // 포함일 경우
            if (p0CCW == 0 && p0.InsideLineSegment(p2, p3)) return true;
            if (p1CCW == 0 && p1.InsideLineSegment(p2, p3)) return true;
            if (p2CCW == 0 && p2.InsideLineSegment(p0, p1)) return true;
            if (p3CCW == 0 && p3.InsideLineSegment(p0, p1)) return true;

            return false;
        }

        public static bool GetLineIntersection(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, out Vector2 intersection)
        {
            intersection = Vector3.zero;

            float denominator = (p0.x - p1.x) * (p2.y - p3.y) - (p0.y - p1.y) * (p2.x - p3.x);
            if (denominator == 0)
                return false;

            float t1 = ((p0.x - p2.x) * (p2.y - p3.y) - (p0.y - p2.y) * (p2.x - p3.x)) / denominator;
            float t2 = ((p0.x - p2.x) * (p0.y - p1.y) - (p0.y - p2.y) * (p0.x - p1.x)) / denominator;

            if (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1)
            {
                float x = p0.x + t1 * (p1.x - p0.x);
                float y = p0.y + t1 * (p1.y - p0.y);
                intersection = new Vector2(x, y);
                return true;
            }

            return false;
        }
    }
}