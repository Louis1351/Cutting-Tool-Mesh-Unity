
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct FaceVertex
{
    public int id;
    public Vector3 pos;

    public FaceVertex(int _id, Vector3 _pos)
    {
        id = _id;
        pos = _pos;
    }
}
public class Face 
{
    private List<Triangle> triangles;
    private int triangleID;
    private int indice;

    public int debugFaceId;//to do remove
 
    private List<FaceVertex> vertices;
    #region assessors
    public List<Triangle> Triangles { get => triangles; set => triangles = value; }
    public int TriangleID { get => triangleID; set => triangleID = value; }
    public List<FaceVertex> Vertices { get => vertices; }
    #endregion

    public Face(int _indice = 0)
    {
        triangles = new List<Triangle>();
        vertices = new List<FaceVertex>();
        triangles.Add(new Triangle());
        triangleID = 0;
        indice = _indice;
    }

    private bool Contain(Vector3 _vertexPos)
    {
        bool find = false;
        foreach (FaceVertex vertex in vertices)
        {
            if (vertex.pos == _vertexPos)
                return true;
        }
        return find;
    }

    public void RemoveVertex(Vector3 _vertex)
    {

    }

    public void AddVertex(Vector3 _vertex)
    {
        bool isNewVertex = false;
        int indiceID = 0;

        if (!Contain(_vertex))
        {
            isNewVertex = true;
            //Vertices.Add(indice++, _vertex);
            vertices.Add(new FaceVertex(indice++, _vertex));
        }

        indiceID = vertices.FirstOrDefault(x => x.pos == _vertex).id;

        if (triangleID >= 1 && isNewVertex)
        {
            //Debug.Log("Face num " + debugFaceId + " triangle num " + triangleID + " add indice " + triangles[triangleID - 1].Indices[2] + " indices count " + triangles[triangleID].Indices.Count + " vertex " + _vertex);
            //Debug.Log("Face num " + debugFaceId + " triangle num " + triangleID + " add indice " + indiceID + " indices count " + triangles[triangleID].Indices.Count + " vertex " + _vertex);
            //Debug.Log("Face num " + debugFaceId + " triangle num " + triangleID + " add indice " + triangles[triangleID - 1].Indices[0] + " indices count " + triangles[triangleID].Indices.Count + " vertex " + _vertex);

            triangles[triangleID].Indices.Add(triangles[triangleID - 1].Indices[2]);
            triangles[triangleID].Indices.Add(indiceID);
            triangles[triangleID].Indices.Add(triangles[triangleID - 1].Indices[0]);
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
    public void CleanUnusedTriangles()
    {
        foreach (Triangle tr in triangles.ToArray())
        {
            if (tr.Indices.Count == 0)
            {
                triangles.Remove(tr);
                triangleID--;
            }
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