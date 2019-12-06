
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Face /*: MeshData*/
{
    private List<Triangle> triangles;
    private int triangleID;
    private int indice;
    public List<Triangle> Triangles { get => triangles; set => triangles = value; }
    public int TriangleID { get => triangleID; set => triangleID = value; }
    public int debugFaceId;
    public Dictionary<int, Vector3> Vertices;
    public Face(int _indice = 0)
    {
        triangles = new List<Triangle>();
        Vertices = new Dictionary<int, Vector3>();
        triangles.Add(new Triangle());
        triangleID = 0;
        indice = _indice;
    }

    public void AddVertex(Vector3 _vertex)
    {
        bool isNewVertex = false;
        if (!Vertices.ContainsValue(_vertex))
        {
            isNewVertex = true;
            Vertices.Add(indice++, _vertex);
        }

        int indiceID = Vertices.FirstOrDefault(x => x.Value == _vertex).Key;

        if (triangleID >= 1 && isNewVertex && triangles[triangleID].Indices.Count == 0)
        {
            //Debug.Log("Face num " + debugFaceId + " triangle num " + triangleID + " add indice " + triangles[triangleID - 1].Indices[2] + " indices count " + triangles[triangleID].Indices.Count + " vertex " + _vertex);
            //Debug.Log("Face num " + debugFaceId + " triangle num " + triangleID + " add indice " + indiceID + " indices count " + triangles[triangleID].Indices.Count + " vertex " + _vertex);
            //Debug.Log("Face num " + debugFaceId + " triangle num " + triangleID + " add indice " + triangles[triangleID - 1].Indices[0] + " indices count " + triangles[triangleID].Indices.Count + " vertex " + _vertex);

            
            triangles[triangleID].Indices.Add(indiceID);
            triangles[triangleID].Indices.Add(triangles[triangleID - 1].Indices[0]);
            triangles[triangleID].Indices.Add(triangles[triangleID - 1].Indices[2]);
        }
        else
        {
            //Debug.Log("Face num " + debugFaceId + " triangle num " + triangleID + " add indice " + indiceID + " indices count " + triangles[triangleID].Indices.Count + " vertex " + _vertex);
            triangles[triangleID].Indices.Add(indiceID);
        }

        if (triangles[triangleID].Indices.Count % 3 == 0
            && triangles[triangleID].Indices.Count != 0)
        {
            triangleID++;
            triangles.Add(new Triangle());


        }


    }

    public Triangle GetCurrentTriangle()
    {
        return triangles[triangleID];
    }
    public void SetStartingIndice(int _indice)
    {
        indice = _indice;
    }
    public int GetCurrentIndice()
    {
        return indice;
    }
}