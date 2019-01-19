using UnityEngine;
using Nrtx.Geometry;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldGenerator : MonoBehaviour
{
    public enum ZoneType
    {
        Dirt = 0,
        Stone,
        Sand,
        Count
    }

    // r = uv.x
    // g = uv.y
    // b = size
	Color[] indexColor = {
		new Color(0.0f,  0f, 0.245f),
		new Color(0.252f, 0f, 0.245f),
		new Color(0.502f,  0f, 0.245f),
		new Color(0.752f,  0f, 0.245f),
		new Color(0.0f,  0.252f, 0.245f),
	};

    public bool randomSeed;
    public float seed = 0f;
    public int fieldSize = 30;

    [Header("Zones")]
    public int nbZones = 10;

    [Header("Worms")]
    public int nbWorms = 2;
    public int wormsStepCount = 150;

    [Header("Ref")]
    public GameObject treasureEffect;

    private Nrtx.Noise.Cellular _zones;
    private Nrtx.Noise.Worms _holes;
    private int[,,] _map;
    private Vector3 _treasurePosition;

    private void GenField()
    {
        _map = new int[fieldSize, fieldSize, fieldSize];

        _zones = new Nrtx.Noise.Cellular(seed, nbZones, fieldSize);
        _zones.ComputeField();

        _holes = new Nrtx.Noise.Worms(seed + 100, nbWorms, fieldSize, wormsStepCount);
        _holes.ComputeField();

        BuildMap();
        NoiseBorder();

        
    }

    private void TreasureRoom(Vector3 position, int size)
    {
        for(int x = (int)position.x - size / 2; x < (int)position.x + size / 2; ++x)
		{
			for(int y = (int)position.y - size / 2; y < (int)position.y + size / 2; ++y)
			{
				for(int z = (int)position.z - size / 2; z < (int)position.z + size / 2; ++z)
				{
                    _map[x, y, z] = -1;
                }
            }
        }
        // First block
        _map[(int)position.x, (int)position.y  - size / 2, (int)position.z] = (int)ZoneType.Count;
        // Treasure
        _map[(int)position.x, (int)position.y  - size / 2 + 1, (int)position.z] = (int)ZoneType.Count + 1;

        GameObject.Instantiate(treasureEffect, new Vector3((int)position.x, (int)position.y  - size / 2 + 1, (int)position.z) ,Quaternion.identity);
    }

    private void BuildMap()
    {
        for(int x = 0; x < fieldSize; ++x)
		{
			for(int y = 0; y < fieldSize; ++y)
			{
				for(int z = 0; z < fieldSize; ++z)
				{
                    if (_holes[x, y, z] > 0)
                    {
                        _map[x, y, z] = _zones[x, y, z] % (int)ZoneType.Count;
                    }
                    else
                    {
                        _map[x, y, z] = -1;
                    }
                }
            }
        }
    }

    private void NoiseBorder()
    {
        for(int x = 0; x < fieldSize; ++x)
		{
			for(int y = 0; y < fieldSize; ++y)
			{
				for(int z = 0; z < fieldSize; ++z)
				{
                    if (x == 0 || x == fieldSize - 1 ||
                        y == 0 || y == fieldSize - 1 ||
                        z == 0 || z == fieldSize - 1)
                    {
                        _map[x, y, z] = Nrtx.Utils.Random(new Vector3(x, y, z), seed) > .95f ? (int) ZoneType.Count : _map[x, y, z];
                    }
                }
            }
        }
    }

    private void GenGeometry()
    {
        QuadMesh chunk = new QuadMesh();
        for(int x = 0; x < fieldSize; ++x)
		{
			for(int y = 0; y < fieldSize; ++y)
			{
				for(int z = 0; z < fieldSize; ++z)
				{
                    AddCubeFace(chunk, x, y, z);
                }
            }
        }
        chunk.Build();
        MeshFilter filter = GetComponent<MeshFilter>();
		filter.sharedMesh = chunk.Mesh;
    }

    private void AddCubeFace(QuadMesh chunk, int x, int y, int z)
    {
        if (_map[x, y, z] == -1)
        {
            if (x == 0)
                chunk.AddFace(new Vector3(x - 1, y, z), QuadMesh.Face.Right, indexColor[(int)ZoneType.Count]);
            if (x == fieldSize - 1)
                chunk.AddFace(new Vector3(x + 1, y, z), QuadMesh.Face.Left, indexColor[(int)ZoneType.Count]);
            if (y == 0)
                chunk.AddFace(new Vector3(x, y - 1, z), QuadMesh.Face.Top, indexColor[(int)ZoneType.Count]);
            if (y == fieldSize - 1)
                chunk.AddFace(new Vector3(x, y + 1, z), QuadMesh.Face.Bottom, indexColor[(int)ZoneType.Count]);
            if (z == 0)
                chunk.AddFace(new Vector3(x, y, z - 1), QuadMesh.Face.Front, indexColor[(int)ZoneType.Count]);
            if (z == fieldSize - 1)
                chunk.AddFace(new Vector3(x, y, z + 1), QuadMesh.Face.Back, indexColor[(int)ZoneType.Count]);
            return;
        }

        ZoneType zoneType = (ZoneType)(_map[x, y, z]);
        Color color = indexColor[(int) zoneType % indexColor.Length];
        QuadMesh.Face faces = QuadMesh.Face.None;

        if ( x > 0 && _map[x - 1, y, z] < 0)
            faces |= QuadMesh.Face.Left;
        if ( x < fieldSize - 1 && _map[x + 1, y, z] < 0)
            faces |= QuadMesh.Face.Right;
        if ( y > 0 && _map[x, y - 1, z] < 0)
            faces |= QuadMesh.Face.Bottom;
        if ( y < fieldSize - 1 && _map[x, y + 1, z] < 0)
            faces |= QuadMesh.Face.Top;
        if ( z > 0 && _map[x, y, z - 1] < 0)
            faces |= QuadMesh.Face.Back;
        if ( z < fieldSize - 1 && _map[x, y, z + 1] < 0)
            faces |= QuadMesh.Face.Front;
        
        chunk.AddFace(new Vector3(x, y, z), faces, color);
    }

    void Start()
    {
        if (randomSeed)
        {
            seed = Random.value * 1000f;
        }
        GenField();
        _treasurePosition = _holes.FarZonePosition(6);
        TreasureRoom(_treasurePosition, 6);
        GenGeometry();
    }
}