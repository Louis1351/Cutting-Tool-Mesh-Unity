﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicePlane
{
    public struct SliceVector
    {
        public Vector3 direction;
        public Vector3 point;
    }

    public Plane plane;
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;
    public List<SliceVector> slVectors;

    public float DebugLineDist;

    #region assessors
    #endregion
    public SlicePlane()
    {
        slVectors = new List<SliceVector>();
        a = Vector3.zero;
        b = Vector3.zero;
        c = Vector3.zero;
        DebugLineDist = 10.0f;
    }
    public SlicePlane(Vector3 a, Vector3 b, Vector3 c)
    {
        plane.Set3Points(a, b, c);
        this.a = a;
        this.b = b;
        this.c = c;
        DebugLineDist = 10.0f;
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

        Gizmos.color = Color.green;
        foreach (SliceVector slv in slVectors)
        {
            Gizmos.DrawLine(slv.point, slv.point + slv.direction);
        }
    }

    public void AddNewSlVector(Vector3 point, Vector3 direction)
    {
        SliceVector slv;
        slv.point = point;
        slv.direction = direction;
        slVectors.Add(slv);
    }
}
