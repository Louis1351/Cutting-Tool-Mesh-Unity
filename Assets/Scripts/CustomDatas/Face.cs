
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///<summary>A FaceVertex is a vertex which has one unique indice and position</summary>
public struct FaceVertex
{
    public int ind;
    public Vector3 pos;

    public FaceVertex(int _ind, Vector3 _pos)
    {
        ind = _ind;
        pos = _pos;
    }
}
public class Face
{
    private List<Triangle> triangles;
    private int triangleID;
    private int indice;

    public int debugFaceId;

    private List<FaceVertex> vertices;
    #region assessors
    public List<Triangle> Triangles { get => triangles; set => triangles = value; }
    public int TriangleID { get => triangleID; set => triangleID = value; }
    public List<FaceVertex> Vertices { get => vertices; }
    #endregion

    ///<summary>Create a face with triangles. The int paremeter corresponds to the starting indice</summary>
    public Face(int _indice = 0)
    {
        triangles = new List<Triangle>();
        vertices = new List<FaceVertex>();
        triangles.Add(new Triangle());
        triangleID = 0;
        indice = _indice;
    }
    ///<summary>is the vertex (position) is contained in the vertices face</summary>
    public bool Contain(Vector3 _vertexPos)
    {
        bool find = false;
        foreach (FaceVertex vertex in vertices)
        {
            if (vertex.pos == _vertexPos)
                return true;
        }
        return find;
    }
    ///<summary>Add new vertex in the face or not if the last Face already has it.<para/>
    ///Add the corresponding indice to the current triangle </summary>
    public void AddVertex(Vector3 _vertex, Face _lastFace = null)
    {
        bool isNewVertex = false;
        int indiceID = 0;

        if (_lastFace != null && _lastFace.Contain(_vertex))
        {
            isNewVertex = true;
            indiceID = _lastFace.Vertices.FirstOrDefault(x => x.pos == _vertex).ind;
        }
        else
        {
            if (!Contain(_vertex))
            {
                isNewVertex = true;
                vertices.Add(new FaceVertex(indice++, _vertex));
            }

            indiceID = vertices.FirstOrDefault(x => x.pos == _vertex).ind;
        }
        //Add new complete triangle if the face has already get a full triangle 
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

        //Create a new Triangle if we have more than three indices
        if (triangles[triangleID].Indices.Count % 3 == 0
            && triangles[triangleID].Indices.Count != 0)
        {
            triangleID++;
            triangles.Add(new Triangle());
        }
    }

    public void AddIndice(int _indice)
    {
        triangles[triangleID].Indices.Add(_indice);

        //Create a new Triangle if we have more than three indices
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