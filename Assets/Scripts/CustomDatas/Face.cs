
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
}
