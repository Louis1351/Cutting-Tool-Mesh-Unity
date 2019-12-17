using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle 
{
    private List<int> indices;
    public List<int> Indices { get => indices; set => indices = value; }

    ///<summary>Triangle generate a list of indices</summary>
    public Triangle()
    {
        indices = new List<int>();
    }
}
