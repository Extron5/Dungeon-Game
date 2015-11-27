using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Mesh3D : MonoBehaviour
{
    private Mesh mesh2;
    public ExtrudeShape shape;

    // Use this for initialization
    void Start()
    {
        mesh2 = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh2;
    }

    // Update is called once per frame
    void Update()
    {
        List<CubicBezier3D.OrientedPoint> points = new List<CubicBezier3D.OrientedPoint>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ExtrudeShape>() == null)
            {
                CubicBezier3D.OrientedPoint point = new CubicBezier3D.OrientedPoint();
                
                point.position = transform.GetChild(i).position;
                point.rotation = transform.GetChild(i).rotation;
                points.Add(point);
            }
        }
        Extrude(mesh2, shape, points.ToArray());
    }

    public void Extrude(Mesh mesh2, ExtrudeShape shape, CubicBezier3D.OrientedPoint[] path)
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf.sharedMesh == null)
            mf.sharedMesh = new Mesh();
        Mesh mesh = mf.sharedMesh;

        int vertsInShape = shape.vert2Ds.Count;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.lines.Length * segments;
        int triIndexCount = triCount * 3;

        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];

        /* Generation code goes here */

        
        for (int i = 0; i < path.Length; i++)
        {
            int offset = i * vertsInShape;
            for (int j = 0; j < vertsInShape; j++)
            {
                int id = offset + j;
                vertices[id] = path[i].LocalToWorld(shape.vert2Ds[j].point);
                normals[id] = path[i].LocalToWorldDirection(shape.vert2Ds[j].normal);
                uvs[id] = new Vector2(shape.vert2Ds[j].uCoord, i / ((float)edgeLoops));
            }
        }
        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int[] lines = shape.lines;
            int offset = i * vertsInShape;
            for (int l = 0; l < lines.Length; l += 2)
            {
                int a = offset + lines[l] + vertsInShape;
                int b = offset + lines[l];
                int c = offset + lines[l + 1];
                int d = offset + lines[l + 1] + vertsInShape;
                triangleIndices[ti] = a; ti++;
                triangleIndices[ti] = b; ti++;
                triangleIndices[ti] = c; ti++;
                triangleIndices[ti] = c; ti++;
                triangleIndices[ti] = d; ti++;
                triangleIndices[ti] = a; ti++;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.normals = normals;
        mesh.uv = uvs;

    }

}