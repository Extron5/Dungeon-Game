using UnityEngine;
using System;
using System.Collections.Generic;

public class ExtrudeShape : MonoBehaviour {

    // Use this for initialization
    [Serializable]
    public struct vert2D
    {
        public Vector3 point;
        public Vector3 normal;
        public float uCoord;
    }
    public List<vert2D> vert2Ds;
    public int[] lines;

	void Start ()
    { 
        lines = new int[] { 0, 1, 2, 3, 3, 4, 4, 5 };
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
