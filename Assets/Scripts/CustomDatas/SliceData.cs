using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SliceData
{
    public struct DebugVector
    {
        public Vector3 direction;
        public Vector3 point;
        public Color color;
    }

    private CustomPlane ctmPlane;

    private List<DebugVector>
        slVectorsIntersec,
        slVectorsDebug;

    private Dictionary<int, Face> faces;

    private bool showDebugLines;

    #region assessors
    public CustomPlane CtmPlane { get => ctmPlane; set => ctmPlane = value; }
    public bool ShowDebugLines { get => showDebugLines; set => showDebugLines = value; }
    public List<DebugVector> SlVectorsIntersec { get => slVectorsIntersec; set => slVectorsIntersec = value; }
    public List<DebugVector> SlVectorsDebug { get => slVectorsDebug; set => slVectorsDebug = value; }
    public Dictionary<int, Face> Faces { get => faces; }
    #endregion

    public SliceData()
    {
        slVectorsIntersec = new List<DebugVector>();
        slVectorsDebug = new List<DebugVector>();

        faces = new Dictionary<int, Face>();
        ctmPlane = new CustomPlane();
    }
    public SliceData(Vector3 a, Vector3 b, Vector3 c)
    {
        slVectorsIntersec = new List<DebugVector>();
        slVectorsDebug = new List<DebugVector>();

        faces = new Dictionary<int, Face>();
        ctmPlane = new CustomPlane(a, b, c);
    }
    public void Clear()
    {
        faces.Clear();
        slVectorsIntersec.Clear();
        slVectorsDebug.Clear();
    }
    public void AddNewSlVectorDebug(Vector3 _point, Vector3 _direction, Color _color, bool _isInter = false, bool _checkSide = false)
    {
        List<DebugVector> tmp = slVectorsDebug;
        DebugVector slv;
        slv.point = _point;
        slv.direction = _direction.normalized;
        slv.color = _color;
        // slv.inTriangles = null;

        if (_isInter)
            tmp = slVectorsIntersec;

        if (_checkSide)
        {
            if (!ctmPlane.GetSide(_point))
            {
                slv.color = Color.blue;
            }
            else
            {
                slv.color = Color.red;
            }
        }
        tmp.Add(slv);
    }
    public void AddFace(int _FaceID, Face _face)
    {
        if (!faces.ContainsValue(_face))
            faces.Add(_FaceID, _face);
    }
    public void AddEdge(int _FaceID, Edge _edge, int idVertex = 0)
    {
        if (!ctmPlane.GetSide(_edge.Points[idVertex]))
        {
            if (!faces[_FaceID].Edges.Contains(_edge))
                faces[_FaceID].Edges.Add(_edge);
        }
        else
        {
            if (!faces[_FaceID + 1].Edges.Contains(_edge))
                faces[_FaceID + 1].Edges.Add(_edge);
        }

    }
    public void AddSeperateEdges(int _FaceID, Edge _edge, Vector3 _intersection)
    {
        Edge newEdge1 = new Edge(_edge.Points[0], _intersection);
        Edge newEdge2 = new Edge(_intersection, _edge.Points[1]);

        AddEdge(_FaceID, newEdge1, 0);
        AddEdge(_FaceID, newEdge2, 1);
    }

    public void CleanUnusedDebugIntersections()
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
        DrawSliceVectors(slVectorsDebug, 0.05f, ShowDebugLines);
        DrawSliceVectors(slVectorsIntersec, 0.05f, false);

        ctmPlane.DrawPlane();
    }
    public void DrawSliceVectors(List<DebugVector> _listSliceVector, float _pointSize, bool _drawLine = false)
    {
        foreach (DebugVector slv in _listSliceVector)
        {
            Gizmos.color = slv.color;
            Gizmos.DrawSphere(slv.point, _pointSize);
            if (_drawLine)
                Gizmos.DrawLine(slv.point - slv.direction * 5000.0f, slv.point + slv.direction * 5000.0f);
        }
    }
    #endregion
}
