using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    private List<Vector3> vertices;
    public List<Vector3> Vertices { get => vertices; set => vertices = value; }

    public MeshData()
    {
        vertices = new List<Vector3>();
    }

    public MeshData(List<Vector3> vertices)
    {
        Vertices = vertices;
    }

    public bool Contain(Vector3 _vertex)
    {
        foreach (Vector3 pt in vertices)
        {
            if (SlicedMeshLibrary.IsEqualTo(pt, _vertex, Mathf.Epsilon))
            {
                return true;
            }
        }
        return false;
    }
}
