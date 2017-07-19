using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shape : MonoBehaviour {
	float pi = Mathf.PI;
	// Use this for initialization


	void Start () {
		float eipix = cos (2 * pi / 3f);
		float eipiy = sin (2 * pi / 3f);
		Geo L = GetComponentInChildren<Geo> ();
		/*Geo L2 = Instantiate (L, transform);
		Geo L3 = Instantiate (L, transform);
		Geo L4 = Instantiate (L, transform);
		Geo L5 = Instantiate (L, transform);
		Geo L6 = Instantiate (L, transform);
		Geo L7 = Instantiate (L, transform);
		Vector3 _1 = new Vector3 (1, 2, 0);
		Vector3 w = new Vector3 (eipix, 2, eipiy);
		Vector3 w2 = new Vector3 (eipix, 2, -eipiy);
		Vector3 T = Vector3.up;
		L.v1 = _1;
		L.v2 = w;
		L2.v1 = _1;
		L2.v2 = w2;
		L3.v1 = w;
		L3.v2 = T;
		L4.v1 = w;
		L4.v2 = w2;
		L5.v1 = T;
		L5.v2 = w2;
		L6.v1 = T;
		L6.v2 = _1;
		L7.v1 = T;
		L7.v2 = w;*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	float sin(float x){
		return Mathf.Sin (x);
	}
	float cos(float x){
		return Mathf.Cos (x);
	}

	public class Polytope
	{
		protected Vector3[] vertices;
		protected Vector3[,] edges;
		protected bool[,] edgelist;
		protected bool[,][,] facelist;


		public Polytope(Vector3[] Vertices, bool[,] Edges, bool[,][,] Faces)
		{
			vertices = Vertices;
			edgelist = Edges;
			facelist = Faces;
			for(int i  = 0; i < Vertices.Length; i++)
			{
				for (int j = 0; j < Vertices.Length;j++)
				{
					if (Edges[i,j])
					{
						edges[i,j] = Vertices[j] - Vertices[i];
					}
				}
			}
		}

		public Vector3[] GetVertices()
		{
			return vertices;
		}

		public Vector3[,] GetEdges()
		{
			return edges;
		}

		public bool[,] GetEdgeList()
		{
			return edgelist;
		}

		public bool[,][,] GetFaceList()
		{
			return facelist;
		}
	}





	Vector3 pick(bool x, Vector3 Vec)
	{
		if (x) {
			return Vec;
		}
		return Vector3.zero;
	}

}
