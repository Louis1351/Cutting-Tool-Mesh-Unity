using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    private List<Vector3> points;
    public List<Vector3> Points { get => points; set => points = value; }

    public Edge()
    {
        points = new List<Vector3>();
    }

    public Edge(Vector3 p1, Vector3 p2)
    {
        points = new List<Vector3>();
        points.Add(p1);
        points.Add(p2);
    }
}
