using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh
{

    public Mesh mesh;
    public List<Vector3> vertices;
    public List<Color> colors;
    public List<Vector2> UVs;
    public List<int> triangles;

    public string name;
    public Vector3 pivot;

    public CustomMesh()
    {
        mesh = new Mesh();
        vertices = new List<Vector3>();
        UVs = new List<Vector2>();
        triangles = new List<int>();
        colors = new List<Color>();

        name = "customMesh";
        pivot = Vector3.one * 0.5f;
        mesh.name = name;
    }

    public void Clear()
    {
        vertices.Clear();
        colors.Clear();
        UVs.Clear();
        triangles.Clear();
    }
    public void AssignToMesh(MeshFilter meshFilter)
    {
        meshFilter.mesh = mesh;
    }
    public void AssignToSharedMesh(MeshFilter meshFilter)
    {
        meshFilter.sharedMesh = mesh;
    }
    public void AssignToSharedMesh(MeshCollider meshCollider)
    {
        meshCollider.sharedMesh = mesh;
    }
    public void Recalculate()
    {
        mesh.SetVertices(vertices);
        mesh.SetColors(colors);
        mesh.SetUVs(0, UVs);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }
    public void CreateCube(Vector3 scale)
    {
        #region SET VERTICES
        Clear();

        vertices.Add(Vector3.Scale(new Vector3(0, 0, 0) - pivot, scale));//0
        vertices.Add(Vector3.Scale(new Vector3(0, 1, 0) - pivot, scale));//1
        vertices.Add(Vector3.Scale(new Vector3(1, 0, 0) - pivot, scale));//2
        vertices.Add(Vector3.Scale(new Vector3(1, 1, 0) - pivot, scale));//3
        //
        vertices.Add(Vector3.Scale(new Vector3(1, 0, 0) - pivot, scale));//4
        vertices.Add(Vector3.Scale(new Vector3(1, 1, 0) - pivot, scale));//5
        vertices.Add(Vector3.Scale(new Vector3(1, 0, 1) - pivot, scale));//6
        vertices.Add(Vector3.Scale(new Vector3(1, 1, 1) - pivot, scale));//7
        //
        vertices.Add(Vector3.Scale(new Vector3(1, 0, 1) - pivot, scale));//8
        vertices.Add(Vector3.Scale(new Vector3(1, 1, 1) - pivot, scale));//9
        vertices.Add(Vector3.Scale(new Vector3(0, 0, 1) - pivot, scale));//10
        vertices.Add(Vector3.Scale(new Vector3(0, 1, 1) - pivot, scale));//11
        //
        vertices.Add(Vector3.Scale(new Vector3(0, 0, 1) - pivot, scale));//12
        vertices.Add(Vector3.Scale(new Vector3(0, 1, 1) - pivot, scale));//13
        vertices.Add(Vector3.Scale(new Vector3(0, 0, 0) - pivot, scale));//14
        vertices.Add(Vector3.Scale(new Vector3(0, 1, 0) - pivot, scale));//15
        //
        vertices.Add(Vector3.Scale(new Vector3(0, 1, 0) - pivot, scale));//16
        vertices.Add(Vector3.Scale(new Vector3(0, 1, 1) - pivot, scale));//17
        vertices.Add(Vector3.Scale(new Vector3(1, 1, 0) - pivot, scale));//18
        vertices.Add(Vector3.Scale(new Vector3(1, 1, 1) - pivot, scale));//19
        //
        vertices.Add(Vector3.Scale(new Vector3(0, 0, 0) - pivot, scale));//20
        vertices.Add(Vector3.Scale(new Vector3(0, 0, 1) - pivot, scale));//21
        vertices.Add(Vector3.Scale(new Vector3(1, 0, 0) - pivot, scale));//22
        vertices.Add(Vector3.Scale(new Vector3(1, 0, 1) - pivot, scale));//23
        #endregion

        #region SET TRIANGLES
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        triangles.Add(1);
        triangles.Add(3);
        triangles.Add(2);

        triangles.Add(4);
        triangles.Add(5);
        triangles.Add(6);

        triangles.Add(5);
        triangles.Add(7);
        triangles.Add(6);

        triangles.Add(8);
        triangles.Add(9);
        triangles.Add(10);

        triangles.Add(9);
        triangles.Add(11);
        triangles.Add(10);

        triangles.Add(12);
        triangles.Add(13);
        triangles.Add(14);

        triangles.Add(13);
        triangles.Add(15);
        triangles.Add(14);

        triangles.Add(16);
        triangles.Add(17);
        triangles.Add(18);

        triangles.Add(17);
        triangles.Add(19);
        triangles.Add(18);

        triangles.Add(22);
        triangles.Add(21);
        triangles.Add(20);

        triangles.Add(22);
        triangles.Add(23);
        triangles.Add(21);
        #endregion

        for (int i = 0; i < vertices.Count; i++)
        {
            colors.Add(Color.blue);
        }
        Recalculate();
    }
}
