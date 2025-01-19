using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace H00N.Geometry2D
{
    public class ConstrainedDelaunayTriangle : DelaunayTriangle
    {
        private readonly Edge[] constraintEdges = null;
        public Edge[] ConstraintEdges => constraintEdges;

        public ConstrainedDelaunayTriangle(Edge[] constraintEdges, Vector2[] vertices, float coordinateLimit = 99999) : base(vertices, coordinateLimit) 
        {
            this.constraintEdges = constraintEdges;
        }

        protected override void OnAfterProcessVertices()
        {
            foreach(Edge edge in constraintEdges)
                AddConstraintEdgeToTriangulation(edge);
        }

        private void AddConstraintEdgeToTriangulation(Edge edge)
        {
            List<Triangle> badTriangles = new List<Triangle>();
            for (int i = triangles.Count - 1; i >= 0; --i)
            {
                Triangle triangle = triangles[i];

                // edge가 삼각형의 선분중 하나 이상을 가로지르면 Bad Triangle
                if(triangle.Edges.Any(i => i.CrossingWith(edge)))
                {
                    badTriangles.Add(triangle);
                    triangles.RemoveAt(i);
                }
            }

            foreach (Triangle triangle in badTriangles)
            {
                Edge crossingEdge = new Edge();
                int crossingEdgeCount = 0;
                foreach(Edge other in triangle.Edges)
                {
                    if(edge == other)
                        continue;

                    if(edge.CrossingWith(other) == false)
                        continue;

                    crossingEdgeCount++;
                    crossingEdge = other;
                }

                if (crossingEdgeCount > 2)
                    continue;

                Triangle t0 = new Triangle(edge[0], edge[1], crossingEdge[0]);
                if (triangles.Contains(t0) == false)
                    triangles.Add(t0);

                Triangle t1 = new Triangle(edge[0], edge[1], crossingEdge[1]);
                if (triangles.Contains(t1) == false)
                    triangles.Add(t1);
            }
        }
    }
}