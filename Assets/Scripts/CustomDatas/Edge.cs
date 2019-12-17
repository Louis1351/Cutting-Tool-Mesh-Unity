using System.Collections.Generic;
using UnityEngine;
using System;
public class Edge:MeshData
{
    ///<summary>Create an edge with zero vertors</summary>
    public Edge()
    {
    }
    ///<summary>Create an edge with two vertors</summary>
    public Edge(Vector3 p1, Vector3 p2)
    {
        Vertices.Add(p1);
        Vertices.Add(p2);
    }
    ///<summary>Create an edge with only one vertor</summary>
    public Edge(Vector3 p1)
    {
        Vertices.Add(p1);
    }

}
