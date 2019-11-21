
using System.Collections.Generic;
using UnityEngine;

public class Face
{
    private List<Edge> points;

    public List<Edge> Points { get => points; set => points = value; }

    public Face()
    {
        Points = new List<Edge>();
    }
}
