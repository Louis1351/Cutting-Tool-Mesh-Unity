using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicePlane : MonoBehaviour
{
    public Plane plane;
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;
    public float DebugLineDist;

    #region assessors
    #endregion
    public SlicePlane()
    {
        plane = new Plane();
        DebugLineDist = 10.0f;
        a = Vector3.zero;
        b = Vector3.zero;
        c = Vector3.zero;
    }
    public SlicePlane(Vector3 a, Vector3 b, Vector3 c)
    {
        plane = new Plane(a, b, c);
        DebugLineDist = 10.0f;
        this.a = a;
        this.b = b;
        this.c = c;
    }
    public void setPoints(Vector3 a, Vector3 b, Vector3 c)
    {
        this.a = a;
        this.b = b;
        this.c = c;

        plane.Set3Points(a, b, c);
    }
    public void drawOnGizmos()
    {
        Gizmos.DrawLine(a, a + (c - a) * DebugLineDist);
        Gizmos.DrawLine(a, a + (b - a) * DebugLineDist);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(a, 0.05f);
        Gizmos.DrawSphere(b, 0.05f);
        Gizmos.DrawSphere(c, 0.05f);
    }
}
