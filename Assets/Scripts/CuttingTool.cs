using UnityEngine;

[ExecuteInEditMode]
public class CuttingTool : MonoBehaviour
{
    [SerializeField]
    Material mat;

    [SerializeField]
    bool showDebugLines;

    private SliceData slData1;
    private SliceData slData2;
    private Vector3 lastMousePos;
    private Vector3 center;

    private bool hasClicked;
    private int lftBtn;

    // Use this for initialization
    void Start()
    {
        slData1 = new SliceData();
        slData2 = new SliceData();

        hasClicked = false;
        lastMousePos = Vector2.zero;
        lftBtn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        slData1.ShowDebugLines = showDebugLines;
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
                slData1.Clear();
                slData2.Clear();

                center = hit.point;
                slData1.CtmPlane.Set3Points(
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f) + Camera.main.transform.forward));

                MeshFilter mf = hit.transform.GetComponent<MeshFilter>();
                MeshRenderer mr = hit.transform.GetComponent<MeshRenderer>();
                Vector3 finalPoint;

                for (int i = 0; i < mf.mesh.triangles.Length; i += 3)
                {
                    Vector3 localNormal = mf.mesh.normals[mf.mesh.triangles[i]];
                    Vector3 normalTriangle = hit.transform.TransformVector(localNormal);

                    Vector3 V1 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i]]);
                    Vector3 V2 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i + 1]]);
                    Vector3 V3 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i + 2]]);

                    slData2.CtmPlane.Set3Points(V1, V2, V3);

                    slData1.AddNewSlVector(V1, Vector3.zero, Color.magenta, i, true);
                    slData1.AddNewSlVector(V2, Vector3.zero, Color.magenta, i, true);
                    slData1.AddNewSlVector(V3, Vector3.zero, Color.magenta, i, true);

                    Vector3 pointOnSliceVec;
                    Vector3 sliceDir;

                    if (!SlicedMeshLibrary.IntersectionPlanToPlan(out pointOnSliceVec, out sliceDir, slData1.CtmPlane, slData2.CtmPlane))
                        continue;

                    bool drawSlice = false;
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V2, V1, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        slData1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta, i);
                    }
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V3, V2, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        slData1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta, i);
                    }
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V1, V3, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        slData1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta, i);
                    }

                    if (drawSlice)
                        slData1.AddNewSlVectorDebug(pointOnSliceVec, sliceDir, Color.green);

                }

                slData1.CleanUnusedIntersections();
                SlicedMeshLibrary.GenerateLeftMesh(mf, mr, hit.transform, slData1);
                SlicedMeshLibrary.GenerateRightMesh(mf, mr, hit.transform, slData1);
            }

        }

    }

    /// <summary>
    /// DRAW EVERYTHING ///
    /// </summary>
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
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center, 0.1f);*/

        if (slData1 != null)
            slData1.drawOnGizmos();

    }
}
