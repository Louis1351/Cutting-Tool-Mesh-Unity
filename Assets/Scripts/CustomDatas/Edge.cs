using System.Collections.Generic;
using UnityEngine;
using System;
public class Edge:MeshData
{
    public Edge()
    {
    }

    public Edge(Vector3 p1, Vector3 p2)
    {
        Vertices.Add(p1);
        Vertices.Add(p2);
    }

    public Edge(Vector3 p1)
    {
        Vertices.Add(p1);
    }

}
