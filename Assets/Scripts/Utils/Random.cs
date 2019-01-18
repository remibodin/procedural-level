using UnityEngine;

namespace Nrtx
{   
    public class Utils
    {
        public static float Random(Vector2 position, float seed = 0f)
        {
            return (Mathf.Abs(Mathf.Sin(Vector3.Dot(position, new Vector2(12.9898f + seed, 78.233f + seed))) * 43758.5453123f)) % 1;
        }

        public static float Random(Vector3 position, float seed = 0f)
        {
            return (Mathf.Abs(Mathf.Sin(Vector3.Dot(position, new Vector3(12.9898f + seed, 78.233f + seed, 1.165f + seed))) * 43758.5453123f)) % 1;
        }

        public static Vector3 RandomPosition(int pointRank, Vector3 scale, float seed = 0f)
        {
            Vector3 position = Vector3.zero;
            position.x = Random(Vector3.right * pointRank, seed) * scale.x;
            position.y = Random(Vector3.up * pointRank, seed) * scale.y;
            position.z = Random(Vector3.forward * pointRank, seed) * scale.z;
            return position;
        }
    }
}