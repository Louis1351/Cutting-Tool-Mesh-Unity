
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Face
{
    private List<Edge> edges;

    public List<Edge> Edges { get => edges; set => edges = value; }

    public Face()
    {
        edges = new List<Edge>();
    }

    public List<Vector3> GetDistinctsPoints()
    {
        List<Vector3> points = null;
        foreach (Edge e in edges)
        {
            if (points == null)
                points = e.Points;
            else points = points.Concat(e.Points).ToList();
        }
        points = points.Distinct().ToList();

        return points;
    }

    public void Remove(Vector3 _vertex)
    {
        foreach (Edge e in edges)
        {
            if (e.Points.Count == 2)
            {
                if (SlicedMeshLibrary.IsEqualTo(_vertex, e.Points[0], 0.001f))
                {
                    e.Points.RemoveAt(0);
                }
                else if (SlicedMeshLibrary.IsEqualTo(_vertex, e.Points[1], 0.001f))
                {
                    e.Points.RemoveAt(1);
                }
            }
            else
            {
                if (SlicedMeshLibrary.IsEqualTo(_vertex, e.Points[0], 0.001f))
                {
                    e.Points.RemoveAt(0);
                }
            }
        }
    }
}
