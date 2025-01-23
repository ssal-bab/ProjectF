using System.Linq;
using H00N.Extensions;
using H00N.Geometry2D;
using UnityEngine;

namespace ProjectCoin.Tests
{
    public class TDelaunayTriangle : MonoBehaviour
    {
        private ConstrainedDelaunayTriangle cdt = null;
        private DelaunayTriangle dt = null;

        private Vector2[] vertices = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AD();
                // RandomGeometry();
                // Square();
        }

        private void AD()
        {
            vertices = new Vector2[] {
                new Vector2(-5, -5),                
                new Vector2(-5, 5),                
                new Vector2(5, 5),                
                new Vector2(5, -5),               
                new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
                new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
                new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
                new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
            };

            Edge[] obstacles = new Edge[] {
                new Edge(vertices[4], vertices[5]),
                new Edge(vertices[5], vertices[6]),
                new Edge(vertices[6], vertices[7]),
                new Edge(vertices[7], vertices[4]),
            };

            cdt = new ConstrainedDelaunayTriangle(obstacles, vertices);
            cdt.Triangulation();
            cdt.Triangles.RemoveAll(i => {
                int count = 0;
                foreach(Edge edge in obstacles)
                {
                    if(i.Edges.Contains(edge))
                        count++;
                }

                return count >= 2;
            });

            dt = new DelaunayTriangle(vertices);
            dt.Triangulation();
        }

        private void RandomGeometry()
        {
            vertices = new Vector2[10];
            for(int i  = 0; i < vertices.Length; ++i)
                vertices[i] = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));

            Edge[] obstacles = new Edge[3];
            for(int i = 0; i < obstacles.Length; ++i)
                obstacles[i] = new Edge(vertices[Random.Range(0, vertices.Length)], vertices[Random.Range(0, vertices.Length)]);

            cdt = new ConstrainedDelaunayTriangle(obstacles, vertices);
            cdt.Triangulation();

            dt = new DelaunayTriangle(vertices);
            dt.Triangulation();
        }

        private void Square()
        {
            Vector2[] obstacleVertices = new Vector2[] {
                    new Vector2(-2, -2),
                    new Vector2(-1.5f, 2),
                    new Vector2(2.5f, 2),
                    new Vector2(2, -2)
                };

            vertices = new Vector2[] {
                    new Vector2(-5, -5),
                    new Vector2(-4.5f, 5),
                    new Vector2(5.5f, 5),
                    new Vector2(5, -5),
                    obstacleVertices[0],
                    obstacleVertices[1],
                    obstacleVertices[2],
                    obstacleVertices[3]
                };

            Edge[] obstacles = new Edge[] {
                    new Edge(obstacleVertices[0], obstacleVertices[1]),
                    new Edge(obstacleVertices[1], obstacleVertices[2]),
                    new Edge(obstacleVertices[2], obstacleVertices[3]),
                    new Edge(obstacleVertices[3], obstacleVertices[0]),
                };

            // for (int i = 0; i < vertices.Length; ++i)
            //     vertices[i] = Random.insideUnitCircle * 5f;

            cdt = new ConstrainedDelaunayTriangle(obstacles, vertices);
            cdt.Triangulation();
        }

#if UNITY_EDITOR
        public bool a = true;
        public bool b = true;
        public bool c = true;
        private void OnDrawGizmos()
        {
            if (cdt == null)
                return;

            if (a)
            {
                Gizmos.color = Color.red;
                cdt.Triangles.ForEach(i => {
                    for (int index = 0; index < 3; ++index)
                        Gizmos.DrawLine(i[index], i[(index + 1) % 3]);
                });
            }

            if (c)
            {
                Gizmos.color = Color.yellow;
                dt.Triangles.ForEach(i => {
                    for (int index = 0; index < 3; ++index)
                        Gizmos.DrawLine(i[index], i[(index + 1) % 3]);
                });
            }

            if (b)
            {
                Gizmos.color = Color.green;
                foreach (Edge edge in cdt.ConstraintEdges)
                    Gizmos.DrawLine(edge[0], edge[1]);
            }

            Gizmos.color = Color.black;
            foreach(Vector2 vertex in vertices)
                Gizmos.DrawSphere(vertex, 0.25f);
        }
#endif
    }
}