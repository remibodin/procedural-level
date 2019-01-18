using UnityEngine;

namespace Nrtx.Noise
{
    public class Cellular
    {
        private float _seed;
        private Vector3[] _points;
        private int [,,] _field;
        private int _fieldSize;

        public int NbPoints
        {
            get { return _points.Length; }
        }

        public int FieldSize
        {
            get { return _fieldSize; }
        }

        public int this[int x, int y, int z]
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

        public Cellular(float seed, int nbPoints, int fieldSize)
        {
            _seed = seed;
            _points = new Vector3[nbPoints];
            _fieldSize = fieldSize;
            _field = new int[FieldSize, FieldSize, FieldSize];
        }

        public void ComputeField()
        {
            GenerateRandomPoints();
            for(int x = 0; x < FieldSize; ++x)
            {
                for(int y = 0; y < FieldSize; ++y)
                {
                    for(int z = 0; z < FieldSize; ++z)
                    {
                        _field[x, y, z] = IndexOfClosestPoint(x, y, z);
                    }
                }
            }
        }

        private void GenerateRandomPoints()
        {
            Vector3 fieldSize3f = Vector3.one * FieldSize; // caching Vector3
            for(int i = 0; i < NbPoints; ++i)
            {
                _points[i] = Nrtx.Utils.RandomPosition(i, fieldSize3f, _seed);
            }
        }

        private int IndexOfClosestPoint(int px, int py, int pz)
        {
            int index = -1;
            float minDistance = Mathf.Infinity;
            for(int i = 0; i < NbPoints; ++i)
            {
                float currentDistance = Vector3.Distance(_points[i], new Vector3(px, py, pz));
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    index = i;
                }
            }
            return index;
        }
    }
}
