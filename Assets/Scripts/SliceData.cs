using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceData
{
    public struct SliceVector
    {
        public Vector3 direction;
        public Vector3 point;
        public List<int> inTriangles;
        public Color color;
    }

    public Plane plane;
    private Vector3 a;
    private Vector3 b;
    private Vector3 c;

    public float x, y, z, d;

    public List<SliceVector> slVectorsIntersec, slVectorsLeft, slVectorsRight;

    public float DebugLineDist;

    #region assessors
    #endregion
    public SliceData()
    {
        slVectorsIntersec = new List<SliceVector>();
        slVectorsLeft = new List<SliceVector>();
        slVectorsRight = new List<SliceVector>();

        a = Vector3.zero;
        b = Vector3.zero;
        c = Vector3.zero;
        DebugLineDist = 10.0f;
    }
    public SliceData(Vector3 a, Vector3 b, Vector3 c)
    {
        plane.Set3Points(a, b, c);
        this.a = a;
        this.b = b;
        this.c = c;
        DebugLineDist = 10.0f;
    }
    public void Clear()
    {
        slVectorsIntersec.Clear();
        slVectorsLeft.Clear();
        slVectorsRight.Clear();
    }
    public void setPoints(Vector3 a, Vector3 b, Vector3 c)
    {
        plane.Set3Points(a, b, c);

        this.a = a;
        this.b = b;
        this.c = c;

        this.x = plane.normal.x;
        this.y = plane.normal.y;
        this.z = plane.normal.z;
        this.d = -(x * a.x + y * a.y + z * a.z);
    }
    public void drawOnGizmos()
    {

        foreach (SliceVector slv in slVectorsLeft)
        {
            Gizmos.color = slv.color;
            Gizmos.DrawSphere(slv.point, 0.05f);
        }

        foreach (SliceVector slv in slVectorsRight)
        {
            Gizmos.color = slv.color;
            Gizmos.DrawSphere(slv.point, 0.05f);
        }

        foreach (SliceVector slv in slVectorsIntersec)
        {
            Gizmos.color = slv.color;
            Gizmos.DrawSphere(slv.point, 0.05f);
            Gizmos.DrawLine(slv.point - slv.direction * 5000.0f, slv.point + slv.direction * 5000.0f);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(a, a + (c - a) * DebugLineDist);
        Gizmos.DrawLine(a, a + (b - a) * DebugLineDist);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(a, 0.05f);
        Gizmos.DrawSphere(b, 0.05f);
        Gizmos.DrawSphere(c, 0.05f);
        Gizmos.DrawLine(a, a + plane.normal);


    }
    public void AddNewSlVector(Vector3 point, Vector3 direction, Color color, int triangleIndex, bool checkSide = false)
    {
        SliceVector slv;
        List<SliceVector> tmp = slVectorsIntersec;
        bool find = false;
        int triangleID = triangleIndex / 3;
        slv.point = point;
        slv.direction = direction.normalized;
        slv.color = color;

        if (checkSide)
        {
            if (!plane.GetSide(point))
            {
                tmp = slVectorsLeft;
                slv.color = Color.red;
            }
            else
            {
                tmp = slVectorsRight;
                slv.color = Color.blue;
            }
        }

        foreach (SliceVector s in tmp)
        {
            if (s.point == point)
            {
                if (!s.inTriangles.Contains(triangleID))
                    s.inTriangles.Add(triangleID);
                find = true;
                break;
            }
        }

        if (!find)
        {
            slv.inTriangles = new List<int>();
            slv.inTriangles.Add(triangleID);
            tmp.Add(slv);
        }
    }

    public void CleanUnusedIntersections()
    {
        for (int i = slVectorsIntersec.Count - 1; i >= 0; i--)
        {
            Vector3 currentPoint = slVectorsIntersec[i].point;
            bool removedCurrent = false;
            for (int j = slVectorsIntersec.Count - 1; j >= 0; j--)
            {
                if (j == i) continue;
                for (int k = j - 1; k >= 0; k--)
                {
                    if (k == i) continue;
                  
                    if (SlicedMeshLibrary.PointBetweenOthersPoints(slVectorsIntersec[j].point, slVectorsIntersec[k].point, currentPoint))
                    {
                        slVectorsIntersec.RemoveAt(i);
                        removedCurrent = true;
                        break;
                    }
                }

                if (removedCurrent)
                    break;
            }

        }
    }
}
