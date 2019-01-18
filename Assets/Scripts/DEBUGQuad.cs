using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nrtx.Geometry;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class DEBUGQuad : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MeshFilter filter = GetComponent<MeshFilter>();
		MeshCollider collider = GetComponent<MeshCollider>();
		QuadMesh chunk = new QuadMesh();
		chunk.AddFace(Vector3.zero, QuadMesh.Face.Top | QuadMesh.Face.Left | QuadMesh.Face.Front, Color.green);
		chunk.AddFace(Vector3.zero, QuadMesh.Face.Bottom | QuadMesh.Face.Right , Color.red);
		// chunk.AddFace(Vector3.zero, QuadMesh.Face.Back , Color.blue);
		chunk.Build();
		filter.sharedMesh = chunk.Mesh;
		collider.sharedMesh = chunk.Mesh;
	}
	
}
