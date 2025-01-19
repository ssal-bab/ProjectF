using System;
using H00N.Extensions;
using UnityEngine;

namespace H00N.Geometry2D
{
    public struct Triangle
    {
        public Vector2 vertex0;
        public Vector2 vertex1;
        public Vector2 vertex2;

        public readonly Edge[] Edges => new Edge[] {
            new Edge(this[0], this[1]),
            new Edge(this[1], this[2]),
            new Edge(this[2], this[0]),
        };

        public readonly Vector2[] Vertices => new Vector2[] {
            vertex0,
            vertex1,
            vertex2  
        };

        public readonly Vector3[] Vertices3D => new Vector3[] {
            vertex0,
            vertex1,
            vertex2  
        };

        public Triangle(Vector2 vertex0, Vector2 vertex1, Vector2 vertex2)
        {
            this.vertex0 = vertex0;
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }

        public Triangle(float x0, float y0, float x1, float y1, float x2, float y2)
        {
            vertex0 = new Vector2(x0, y0);
            vertex1 = new Vector2(x1, y1);
            vertex2 = new Vector2(x2, y2);
        }

        public readonly Vector2 this[int index] => index switch {
            0 => vertex0,
            1 => vertex1,
            2 => vertex2,
            _ => vertex2
        };

        public readonly Vector2 Circumcenter()
        {
            // 수직이등분점
            Vector2 mid0 = (vertex0 + vertex1) * 0.5f;
            Vector2 mid1 = (vertex0 + vertex2) * 0.5f;

            // 수직이등분선 기울기
            float xIncrease0 = vertex1.x - vertex0.x;
            float yIncrease0 = vertex1.y - vertex0.y;
            float slope0 = -1 * xIncrease0 / yIncrease0;
            
            float xIncrease1 = vertex2.x - vertex0.x;
            float yIncrease1 = vertex2.y - vertex0.y;
            float slope1 = -1 * xIncrease1 / yIncrease1;

            // y = ax + b
            // b = y - ax
            float b0 = mid0.y - slope0 * mid0.x;
            float b1 = mid1.y - slope1 * mid1.x;

            // y = slope0 * x + b0
            // y = slope1 * x + b1
            // slope0 * x + b0 = slope1 * x + b1
            // (slope0 - slope1) * x = b1 - b0
            // x = (b1 - b0) / (slope0 - slope1)
            // y = slope0 * x + b0
            float x, y;
            if (float.IsInfinity(slope0) || float.IsNegativeInfinity(slope0)) // 첫 번째 수직선이 수직선일 경우
            {
                x = mid0.x;
                y = slope1 * x + b1;
            }
            else if (float.IsInfinity(slope1) || float.IsNegativeInfinity(slope1)) // 두 번째 수직선이 수직선일 경우
            {
                x = mid1.x;
                y = slope0 * x + b0;
            }
            else
            {
                // 일반적인 경우
                x = (b1 - b0) / (slope0 - slope1);
                y = slope0 * x + b0;
            }

            return new Vector2(x, y);
        }

        public readonly float Circumradius() => Circumradius(Circumcenter());
        public readonly float Circumradius(Vector2 circumcenter) => (vertex0 - circumcenter).magnitude;
        
        public readonly bool InsideCircumcircle(Vector2 point)
        {
            Vector2 circumcenter = Circumcenter();
            float circumradius = Circumradius(circumcenter);
            return point.InsideCircle(circumcenter, circumradius);
        }

        public override readonly bool Equals(object obj)
        {
            Triangle triangle = (Triangle)obj;
            return this == triangle;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(vertex0, vertex1, vertex2);
        }

        public static bool operator ==(Triangle left, Triangle right)
        {
            return left.vertex0 == right.vertex0 && left.vertex1 == right.vertex1 && left.vertex2 == right.vertex2;
        }

        public static bool operator !=(Triangle left, Triangle right)
        {
            return !(left == right);
        }
    }
}
