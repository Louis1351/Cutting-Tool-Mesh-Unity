using UnityEngine;

public class CustomPlane
{
    private Plane plane;
    private Vector3 a;
    private Vector3 b;
    private Vector3 c;
    private Vector4 unknowns;

    private float DebugLineDist;

    #region assessors
    public Vector3 A { get => a; }
    public Vector3 B { get => b; }
    public Vector3 C { get => c; }
    public Vector3 Normal { get => plane.normal; }
    public Vector4 UnKnowns { get => unknowns; }
    #endregion

    public CustomPlane()
    {
        DebugLineDist = 10.0f;
    }

    public CustomPlane(Vector3 _a, Vector3 _b, Vector3 _c)
    {

        a = _a;
        b = _b;
        c = _c;
        DebugLineDist = 10.0f;

        plane.Set3Points(a, b, c);
        ComputeUnknowns();
    }

    public void Set3Points(Vector3 _a, Vector3 _b, Vector3 _c)
    {
        a = _a;
        b = _b;
        c = _c;

        plane.Set3Points(a, b, c);
        ComputeUnknowns();
    }

    public void ComputeUnknowns()
    {
        unknowns.x = plane.normal.x;
        unknowns.y = plane.normal.y;
        unknowns.z = plane.normal.z;
        unknowns.w = -(unknowns.x * a.x + unknowns.y * a.y + unknowns.z * a.z);
    }

    public bool GetSide(Vector3 _point)
    {
        return plane.GetSide(_point);
    }

    public void DrawPlane()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(a, a + (c - a) * DebugLineDist);
        Gizmos.DrawLine(a, a + (b - a) * DebugLineDist);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(a, 0.05f);
        Gizmos.DrawSphere(b, 0.05f);
        Gizmos.DrawSphere(c, 0.05f);
        Gizmos.DrawLine(a, a + plane.normal);
    }
}
