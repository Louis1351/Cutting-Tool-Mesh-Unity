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

    private CustomPlane ctmPlane;

    private List<SliceVector> 
        slVectorsIntersec, 
        slVectorsLeft, 
        slVectorsRight, 
        slVectorsDebug;

    private List<Face> faces;

    private bool showDebugLines;

    #region assessors
    public CustomPlane CtmPlane { get => ctmPlane; set => ctmPlane = value; }
    public bool ShowDebugLines { get => showDebugLines; set => showDebugLines = value; }
    public List<SliceVector> SlVectorsIntersec { get => slVectorsIntersec; set => slVectorsIntersec = value; }
    public List<SliceVector> SlVectorsLeft { get => slVectorsLeft; set => slVectorsLeft = value; }
    public List<SliceVector> SlVectorsRight { get => slVectorsRight; set => slVectorsRight = value; }
    public List<SliceVector> SlVectorsDebug { get => slVectorsDebug; set => slVectorsDebug = value; }
   
    #endregion

    public SliceData()
    {
        slVectorsIntersec = new List<SliceVector>();
        slVectorsLeft = new List<SliceVector>();
        slVectorsRight = new List<SliceVector>();
        slVectorsDebug = new List<SliceVector>();
        ctmPlane = new CustomPlane();
    }
    public SliceData(Vector3 a, Vector3 b, Vector3 c)
    {
        slVectorsIntersec = new List<SliceVector>();
        slVectorsLeft = new List<SliceVector>();
        slVectorsRight = new List<SliceVector>();
        slVectorsDebug = new List<SliceVector>();
        ctmPlane = new CustomPlane(a, b, c);
    }
    public void Clear()
    {
        slVectorsIntersec.Clear();
        slVectorsLeft.Clear();
        slVectorsRight.Clear();
        slVectorsDebug.Clear();
    }
    public void AddNewSlVectorDebug(Vector3 point, Vector3 direction, Color color)
    {
        SliceVector slv;
        slv.point = point;
        slv.direction = direction.normalized;
        slv.color = color;
        slv.inTriangles = null;

        slVectorsDebug.Add(slv);
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
            if (!ctmPlane.GetSide(point))
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

    #region DRAWFUNCTIONS
    public void drawOnGizmos()
    {
        if (ShowDebugLines)
            DrawSliceVectors(slVectorsDebug, 0.05f, true);

        DrawSliceVectors(slVectorsLeft, 0.05f);
        DrawSliceVectors(slVectorsRight, 0.05f);
        DrawSliceVectors(slVectorsIntersec, 0.05f, true);

        ctmPlane.DrawPlane();
    }
    public void DrawSliceVectors(List<SliceVector> _listSliceVector, float _pointSize, bool _drawLine = false)
    {
        foreach (SliceVector slv in _listSliceVector)
        {
            Gizmos.color = slv.color;
            Gizmos.DrawSphere(slv.point, _pointSize);
            if (_drawLine)
                Gizmos.DrawLine(slv.point - slv.direction * 5000.0f, slv.point + slv.direction * 5000.0f);
        }
    }
    #endregion
}
