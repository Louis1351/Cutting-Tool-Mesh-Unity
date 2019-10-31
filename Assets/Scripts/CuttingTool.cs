using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CuttingTool : MonoBehaviour
{
    [SerializeField]
    Material mat;

    private SlicePlane slp1;
    private SlicePlane slp2;
    private Vector3 lastMousePos;

    private Vector3 center;

    List<Vector3> points;
    List<Vector3> leftpoints;
    List<Vector3> rightpoints;

    private bool hasClicked;
    private int lftBtn;
    private float distance;

    // Use this for initialization
    void Start()
    {
        slp1 = new SlicePlane();
        slp2 = new SlicePlane();

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
                slp1.slVectors.Clear();

                center = hit.point;
                slp1.setPoints(
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f) + Camera.main.transform.forward));

                MeshFilter mf = hit.transform.GetComponent<MeshFilter>();

                Vector3 finalPoint;
                for (int i = 0; i < mf.mesh.triangles.Length; i += 3)
                {
                    Vector3 normalTriangle = hit.transform.TransformVector(mf.mesh.normals[mf.mesh.triangles[i]]);

                    Vector3 V1 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i]]);
                    Vector3 V2 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i + 1]]);
                    Vector3 V3 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i + 2]]);

                    Vector3 V2V1 = V1 - V2;
                    Vector3 V2V3 = V3 - V2;
                    Vector3 V1V3 = V3 - V1;

                    slp2.setPoints(V1, V2, V3);

                    Vector3 pointOnSliceVec = Vector3.zero;
                    Vector3 sliceDir = Vector3.zero;

                    if (!SlicedMeshLibrary.IntersectionPlanToPlan(ref pointOnSliceVec, ref sliceDir, slp1, slp2))
                        continue;

                    bool drawSlice = false;
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V2, V1, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        slp1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta);
                    }
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V3, V2, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        slp1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta);
                    }
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V1, V3, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        slp1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta);
                    }

                    if (drawSlice)
                        slp1.AddNewSlVector(pointOnSliceVec, sliceDir, Color.green);
                }

                for (int i = 0; i < mf.mesh.vertexCount; i++)
                {

                    Vector3 vertex = hit.transform.TransformPoint(mf.mesh.vertices[i]);
                    if (slp1.plane.GetSide(vertex))
                    {
                        if (!leftpoints.Contains(vertex))
                            leftpoints.Add(vertex);
                    }
                    else
                    {
                        if (!rightpoints.Contains(vertex))
                            rightpoints.Add(vertex);
                    }
                }

                SlicedMeshLibrary.GenerateLeftMesh(mf, transform, slp1.plane, slp1.slVectors);
                SlicedMeshLibrary.GenerateRightMesh(mf, transform, slp1.plane, slp1.slVectors);
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

        if (slp1 != null)
            slp1.drawOnGizmos();
    }

    //To do add into a static class

}
