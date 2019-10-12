using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CuttingTool : MonoBehaviour
{
    [SerializeField]
    Material mat;

    private SlicePlane slp;
    private Vector3 lastMousePos;

    private Vector3 center;
    private Vector3 u;
    private Vector3 v;
    private Vector3 n;

    List<Vector3> points;
    List<Vector3> leftpoints;
    List<Vector3> rightpoints;

    private bool hasClicked;
    private int lftBtn;
    private float distance;

    // Use this for initialization
    void Start()
    {
        slp = new SlicePlane();
        points = new List<Vector3>();
        leftpoints = new List<Vector3>();
        rightpoints = new List<Vector3>();

        hasClicked = false;
        lastMousePos = Vector2.zero;
        lftBtn = 0;
        distance = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasClicked && Input.GetMouseButtonDown(lftBtn))
        {
            hasClicked = true;
            lastMousePos = Input.mousePosition;
        }

        if (hasClicked && Input.GetMouseButtonUp(lftBtn))
        {
            hasClicked = false;
            Vector3 sliceCenter = (lastMousePos + Input.mousePosition) / 2.0f;

            RaycastHit hit, unusedHit;
            Ray rayCenter = Camera.main.ScreenPointToRay(sliceCenter);
            Ray rayP1 = Camera.main.ScreenPointToRay(lastMousePos);
            Ray rayP2 = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayCenter, out hit)
                && !Physics.Raycast(rayP1, out unusedHit)
                && !Physics.Raycast(rayP2, out unusedHit))
            {
                points.Clear();
                leftpoints.Clear();
                rightpoints.Clear();
                slp.slVectors.Clear();

                center = hit.point;
                slp.setPoints(
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f) + Camera.main.transform.forward));

                MeshFilter mf = hit.transform.GetComponent<MeshFilter>();
                float x1 = slp.plane.normal.x;
                float y1 = slp.plane.normal.y;
                float z1 = slp.plane.normal.z;
                float d1 = -(x1 * slp.a.x + y1 * slp.a.y + z1 * slp.a.z);

                //Debug.Log(x1 + "X + " + y1 + "Y + " + z1 + "Z" + " = " + d1);
                int find = 0;
                for (int i = 0; i < mf.mesh.vertexCount; i += 3)
                {
                    
                    Vector3 V1 = hit.transform.TransformPoint(mf.mesh.vertices[i]);
                    Vector3 normalTriangle = hit.transform.TransformVector(mf.mesh.normals[i]);
                    Vector3 dir = Vector3.Cross(normalTriangle, slp.plane.normal);
                    Debug.Log("plane normal -> " + slp.plane.normal);
                    Debug.Log("triangle normal -> " + normalTriangle);
                    Debug.Log("dir -> " + dir);
                  
                    //equation plane N1x(x - xA) + N1y(y - yA) + N1z(z - zA) = 0 | A e plane
                    //equation plane N2x(x - xB) + N2y(y - yB) + N2z(z - zB) = 0 | B e plane
                    //where X = 0
                    //find intersection point
                    ////y = (-c1z -d1) / b1
                    ///z = ((b2/b1)*d1 -d2)/(c2 - c1*b2/b1)
                   
                    float x2 = normalTriangle.x;
                    float y2 = normalTriangle.y;
                    float z2 = normalTriangle.z;
                    float d2 = -(x2 * V1.x + y2 * V1.y + z2 * V1.z);


                    if (dir.x != 0.0f || dir.y != 0.0f || dir.z != 0.0f)//planes aren't coplanar
                    {
                        float dot = Vector3.Dot(dir, dir);
                        Vector3 u1 = d2 * slp.plane.normal;
                        Vector3 u2 = -d1 * normalTriangle;
                        Vector3 p = Vector3.Cross((u1 + u2),dir) / dot;
                       
                        //Debug.Log(x2 + "X + " + y2 + "Y + " + z2 + "Z" + " = " + d2);

                        Debug.Log("p " + p);
                        slp.AddNewSlVector(p, dir);
                        find++;
                    }
                }
                Debug.Log("find " + find);
                for (int i = 0; i < mf.mesh.vertexCount; i++)
                {

                    Vector3 vertex = hit.transform.TransformPoint(mf.mesh.vertices[i]);
                    if (slp.plane.GetSide(vertex))
                    {
                        if (!leftpoints.Contains(vertex))
                            leftpoints.Add(vertex);
                    }
                    else
                    {
                        if (!rightpoints.Contains(vertex))
                            rightpoints.Add(vertex);
                    }

                    Vector3 closestPt = slp.plane.ClosestPointOnPlane(vertex);
                    if (!points.Contains(closestPt))
                        points.Add(closestPt);
                }

                #region todelete
                /* center = hit.point;

                 for (int i = 0; i < mf.mesh.triangles.Length; i += 3)
                 {
                     int vertID0 = mf.mesh.triangles[i];
                     int vertID1 = mf.mesh.triangles[i + 1];
                     int vertID2 = mf.mesh.triangles[i + 2];

                     Vector3 V1 = hit.transform.TransformPoint(mf.mesh.vertices[vertID0]);
                     Vector3 V2 = hit.transform.TransformPoint(mf.mesh.vertices[vertID1]);
                     Vector3 V3 = hit.transform.TransformPoint(mf.mesh.vertices[vertID2]);

                     P.Set3Points(V1, V2, V3);

                     //point is coplanar with V1,V2,V3
                     u = V2 - V1;
                     v = V2 - V3;

                     Vector3 w = V2 - center;
                     n = P.normal;
                     float dot = Vector3.Dot(n, w);

                     if (Mathf.Abs(dot) < 0.01f)
                     {
                         Debug.Log(vertID0 + " " + vertID1 + " " + vertID2);
                         break;
                     }
                 }*/
                #endregion
            }

        }

    }

    void OnPostRender()
    {
        if (hasClicked)
        {
            drawGLLine();
        }
    }
    void drawGLLine()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(new Vector3(lastMousePos.x / Screen.width, lastMousePos.y / Screen.height, 0));
        GL.Vertex(new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0));
        GL.End();

        GL.PopMatrix();
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center, 0.1f);

        Gizmos.color = Color.magenta;
        foreach (Vector3 p in points)
        {
            Gizmos.DrawSphere(p, 0.025f);
        }
        Gizmos.color = Color.red;
        foreach (Vector3 p in leftpoints)
        {
            Gizmos.DrawSphere(p, 0.025f);
        }
        Gizmos.color = Color.blue;
        foreach (Vector3 p in rightpoints)
        {
            Gizmos.DrawSphere(p, 0.025f);
        }

        if (slp != null)
            slp.drawOnGizmos();
    }
}
