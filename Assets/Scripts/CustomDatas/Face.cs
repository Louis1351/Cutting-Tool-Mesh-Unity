
using System.Collections.Generic;
using UnityEngine;

public class Face
{
    private List<Edge> edges;

    public List<Edge> Edges { get => edges; set => edges = value; }

    public Face()
    {
        edges = new List<Edge>();
    }
}
