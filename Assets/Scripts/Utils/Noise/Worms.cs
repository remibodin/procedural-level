using UnityEngine;
using System.Collections.Generic;

namespace Nrtx.Noise
{
    
    public class Worms
    {
        public class Hole
        {
            public Vector3 position;
            public float radius;
        }

        private float _seed;
        private int _fieldSize;
        private int _holeLength;
        private Vector3[] _points;
        private float [,,] _field;
        private List<Hole> _holes;

        public int FieldSize
        {
            get { return _fieldSize; }
        }

        public int NbPoints
        {
            get { return _points.Length; }
        }

        public float this[int x, int y, int z]
        {
            get
            {
                if (x < 0 || x > FieldSize - 1 ||
                    y < 0 || y > FieldSize - 1 ||
                    z < 0 || z > FieldSize - 1)
                {
                    return -1;
                }
                return _field[x, y, z];
            }
        }

        public Worms(float seed, int nbPoints, int fieldSize, int holeLength)
        {
            _seed = seed;
            _points = new Vector3[nbPoints];
            _fieldSize = fieldSize;
            _field = new float[FieldSize, FieldSize, FieldSize];
            _holeLength = holeLength;
        }

        public void ComputeField()
        {
            GenerateRandomPoints();

            // Generation des caverne
            _holes = new List<Hole>();
            for(int i = 0; i < NbPoints; ++i)
            {
                // premier trou de 3 de rayon au debut du tunnel
                _holes.Add( new Hole() {
                    position = _points[i],
                    radius = 3f
			    } );
                Vector3 lastPoint = _points[i];
			    Vector3 direction = Vector3.right;
                for (int it = 0; it < _holeLength; ++it)
			    {
                    float degY = (Nrtx.Utils.Random(new Vector2(it, i), _seed) * 90f) - 45f;
                    float degZ = (Nrtx.Utils.Random(new Vector2(i, it), _seed) * 90f) - 45f;
                    
                    direction = Quaternion.Euler(
                        0f,
                        degY, 
                        degZ 
                    ) * direction;
                    
                    lastPoint += direction * 1.5f;
                    lastPoint.x = Mathf.Abs(lastPoint.x) % FieldSize;
                    lastPoint.y = Mathf.Abs(lastPoint.y) % FieldSize;
                    lastPoint.z = Mathf.Abs(lastPoint.z) % FieldSize;
                    _holes.Add( new Hole() 
					{
						position = lastPoint,
						radius = Mathf.Lerp(2.5f, 3.5f, Utils.Random(lastPoint, _seed))
					} );
                }
            }

            // generation du distance field
            BuildDistancefield();
        }

        private void GenerateRandomPoints()
        {
            Vector3 fieldSize3f = Vector3.one * FieldSize; // caching Vector3
            for(int i = 0; i < NbPoints; ++i)
            {
                _points[i] = Nrtx.Utils.RandomPosition(i, fieldSize3f, _seed);
            }
        }

        private float Distance(Vector3 position)
        {
            float minDistance = Mathf.Infinity;
            for (int i = 0; i < _holes.Count; ++i)
            {
                float distance = Vector3.Distance(position, _holes[i].position) - _holes[i].radius;
                minDistance = Mathf.Min(distance, minDistance);
            }
            return minDistance;
        }

        void BuildDistancefield()
        {
            for (int x = 0; x < FieldSize; ++x)
            {
                for (int y = 0; y < FieldSize; ++y)
                {
                    for (int z = 0; z < FieldSize; ++z)
                    {
                        _field[x, y, z] = Distance(new Vector3(x, y, z));
                    }
                }

            }
        }

        public Vector3 FarZonePosition(int zoneSize)
        {
            float maxDistance = 0f;
            Vector3 position = Vector3.zero;
            for (int x = zoneSize; x < FieldSize - zoneSize; ++x)
            {
                for (int y = zoneSize; y < FieldSize - zoneSize; ++y)
                {
                    for (int z = zoneSize; z < FieldSize - zoneSize; ++z)
                    {
                        if (_field[x, y, z] > maxDistance)
                        {
                            maxDistance = _field[x, y, z];
                            position = new Vector3(x, y, z);
                        }
                    }
                }
            }
            return position;
        }
    }
}