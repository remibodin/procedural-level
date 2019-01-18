using UnityEngine;
using System.Collections.Generic;

namespace Nrtx.Geometry
{
    public class QuadMesh
    {
        public enum Face
        {
            None = 0x000000,
            Top = 0x000001,
            Bottom = 0x000010,
            Left = 0x000100,
            Right = 0x001000,
            Front = 0x010000,
            Back = 0x100000
        }

        private List<int> _triangles;
        private List<Vector3> _vertices;
        private List<Vector2> _uv;
        private List<Color> _colors;

        public Mesh Mesh { get; private set; }
        public bool Dirty { get; private set; }

        public QuadMesh()
        {
            Mesh = new Mesh();
            Dirty = false;
            _triangles = new List<int>();
            _vertices = new List<Vector3>();
            _uv = new List<Vector2>();
            _colors = new List<Color>();
        }

        public void AddFace(Vector3 center, Face face, Color color)
        {
            if ( (face & Face.Top) == Face.Top)
            {
                _vertices.Add(new Vector3(-0.5f, +0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, +0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, +0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, +0.5f, -0.5f) + center);

                _uv.Add(new Vector2(0f, 1f));
                _uv.Add(new Vector2(1f, 1f));
                _uv.Add(new Vector2(1f, 0f));
                _uv.Add(new Vector2(0f, 0f));

                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
            }
            if ( (face & Face.Bottom) == Face.Bottom)
            {
                _vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, -0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, -0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, -0.5f, +0.5f) + center);

                _uv.Add(new Vector2(0f, 0f));
                _uv.Add(new Vector2(1f, 0f));
                _uv.Add(new Vector2(1f, 1f));
                _uv.Add(new Vector2(0f, 1f));

                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
            }
            if ( (face & Face.Left) == Face.Left)
            {
                _vertices.Add(new Vector3(-0.5f, +0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, +0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, -0.5f, +0.5f) + center);

                _uv.Add(new Vector2(1f, 1f));
                _uv.Add(new Vector2(1f, 0f));
                _uv.Add(new Vector2(0f, 0f));
                _uv.Add(new Vector2(0f, 1f));

                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
            }
            if ( (face & Face.Right) == Face.Right)
            {
                _vertices.Add(new Vector3(+0.5f, +0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, +0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, -0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, -0.5f, -0.5f) + center);

                _uv.Add(new Vector2(1f, 0f));
                _uv.Add(new Vector2(1f, 1f));
                _uv.Add(new Vector2(0f, 1f));
                _uv.Add(new Vector2(0f, 0f));

                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
            }
            if ( (face & Face.Front) == Face.Front)
            {
                _vertices.Add(new Vector3(+0.5f, +0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, +0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, -0.5f, +0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, -0.5f, +0.5f) + center);

                _uv.Add(new Vector2(1f, 1f));
                _uv.Add(new Vector2(0f, 1f));
                _uv.Add(new Vector2(0f, 0f));
                _uv.Add(new Vector2(1f, 0f));

                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
            }
            if ( (face & Face.Back) == Face.Back)
            {
                _vertices.Add(new Vector3(-0.5f, +0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, +0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(+0.5f, -0.5f, -0.5f) + center);
                _vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f) + center);

                _uv.Add(new Vector2(0f, 1f));
                _uv.Add(new Vector2(1f, 1f));
                _uv.Add(new Vector2(1f, 0f));
                _uv.Add(new Vector2(0f, 0f));

                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
                _colors.Add(color);
            }
            Dirty = true;
        }

        public void ClearData()
        {
            _vertices.Clear();
            _uv.Clear();
            _colors.Clear();
            _triangles.Clear();
            Dirty = true;
        }

        public void Build()
        {
            Mesh.Clear();

            for(int triangleId = 0; triangleId < _vertices.Count; triangleId += 4)
            {
                _triangles.Add(triangleId + 0);
                _triangles.Add(triangleId + 1);
                _triangles.Add(triangleId + 3);
                
                _triangles.Add(triangleId + 1);
                _triangles.Add(triangleId + 2);
                _triangles.Add(triangleId + 3);
            }

            Mesh.vertices = _vertices.ToArray();
            Mesh.uv = _uv.ToArray();
            Mesh.colors = _colors.ToArray();
            Mesh.triangles = _triangles.ToArray();
            Mesh.RecalculateNormals();
            Dirty = false;
        }
    }
}