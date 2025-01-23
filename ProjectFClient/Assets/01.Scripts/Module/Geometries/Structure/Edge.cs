using System;
using H00N.Extensions;
using UnityEngine;

namespace H00N.Geometry2D
{
    public struct Edge
    {
        public Vector2 vertex0;
        public Vector2 vertex1;

        public Edge(Vector2 vertex0, Vector2 vertex1)
        {
            this.vertex0 = vertex0;
            this.vertex1 = vertex1;
        }

        public Edge(float x0, float y0, float x1, float y1)
        {
            vertex0 = new Vector2(x0, y0);
            vertex1 = new Vector2(x1, y1);
        }

        public readonly Vector2 this[int index] => index switch {
            0 => vertex0,
            1 => vertex1,
            _ => vertex1
        };

        public readonly bool IntersectWith(Vector2 start, Vector2 end)
        {
            return MathExtensions.IntersectWith(this[0], this[1], start, end);
        }

        public readonly bool IntersectWith(Edge other)
        {
            return MathExtensions.IntersectWith(this[0], this[1], other[0], other[1]);
        }

        public readonly bool CrossingWith(Edge other)
        {
            return 
                MathExtensions.IntersectWith(this[0], this[1], other[0], other[1]) && 
                !(this[0] == other[0] || this[0] == other[1] || this[1] == other[0] || this[1] == other[1]);
        }

        public override readonly bool Equals(object obj)
        {
            Edge edge = (Edge)obj;
            return this == edge;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(vertex0, vertex1);
        }

        public static bool operator ==(Edge left, Edge right)
        {
            return 
                (left.vertex0 == right.vertex0 && left.vertex1 == right.vertex1) ||
                (left.vertex0 == right.vertex1 && left.vertex1 == right.vertex0);
        }

        public static bool operator !=(Edge left, Edge right)
        {
            return !(left == right);
        }
    }
}