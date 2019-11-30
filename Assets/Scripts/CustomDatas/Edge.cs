using System.Collections.Generic;
using UnityEngine;
using System;
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

    public Edge(Vector3 p1)
    {
        points = new List<Vector3>();
        points.Add(p1);
    }

    public static bool operator ==(Edge e1, Edge e2)
    {
        if(e1.points.Count != e2.points.Count)
        {
            return false;
        }
        else
        {
            if (e1.points.Count == 1)
                return SlicedMeshLibrary.IsEqualTo(e1.points[0], e2.points[0], 0.001f);
            else return SlicedMeshLibrary.IsEqualTo(e1.points[0], e2.points[0], 0.001f) &&
            SlicedMeshLibrary.IsEqualTo(e1.points[1], e2.points[1], 0.001f);
        }
        
    }

    public static bool operator !=(Edge e1, Edge e2)
    {
        if (e1.points.Count != e2.points.Count)
        {
            return true;
        }
        else
        {
            if (e1.points.Count == 1)
                return !SlicedMeshLibrary.IsEqualTo(e1.points[0], e2.points[0], 0.001f);
            else return !SlicedMeshLibrary.IsEqualTo(e1.points[0], e2.points[0], 0.001f) ||
            !SlicedMeshLibrary.IsEqualTo(e1.points[1], e2.points[1], 0.001f);
        }
    }
}
