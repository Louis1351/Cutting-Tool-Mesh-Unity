using UnityEngine;

public class CuttingTool : MonoBehaviour
{
    [SerializeField]
    Material mat = null;

    [SerializeField]
    bool showDebugLines = false;

    private SliceData data1;
    private SliceData data2;
    private Vector3 lastMousePos;
    private Vector3 center;

    private bool hasClicked;
    private int lftBtn;

    // Use this for initialization
    void Start()
    {
        data1 = new SliceData();
        data2 = new SliceData();

        hasClicked = false;

        lastMousePos = Vector2.zero;
        lftBtn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        data1.ShowDebugLines = showDebugLines;
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
                data1.Clear();
                data2.Clear();

                center = hit.point;
                data1.CtmPlane.Set3Points(
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f) + Camera.main.transform.forward));

                MeshFilter mf = hit.transform.GetComponent<MeshFilter>();
                MeshRenderer mr = hit.transform.GetComponent<MeshRenderer>();
                Vector3 finalPoint;

                for (int i = 0; i < mf.mesh.triangles.Length; i += 3)
                {
                    Vector3 V1 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i]]);
                    Vector3 V2 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i + 1]]);
                    Vector3 V3 = hit.transform.TransformPoint(mf.mesh.vertices[mf.mesh.triangles[i + 2]]);

                    data2.CtmPlane.Set3Points(V1, V2, V3);

                    data1.AddNewSlVector(V1, Vector3.zero, Color.magenta, i, true);
                    data1.AddNewSlVector(V2, Vector3.zero, Color.magenta, i, true);
                    data1.AddNewSlVector(V3, Vector3.zero, Color.magenta, i, true);

                    Vector3 pointOnSliceVec;
                    Vector3 sliceDir;

                    if (!SlicedMeshLibrary.IntersectionPlanToPlan(out pointOnSliceVec, out sliceDir, data1.CtmPlane, data2.CtmPlane))
                        continue;

                    bool drawSlice = false;
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V2, V1, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        data1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta, i);
                    }
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V3, V2, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        data1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta, i);
                    }
                    if (SlicedMeshLibrary.IntersectionVectorToVector(out finalPoint, V1, V3, pointOnSliceVec, sliceDir))
                    {
                        drawSlice = true;
                        data1.AddNewSlVector(finalPoint, Vector3.zero, Color.magenta, i);
                    }

                    if (drawSlice)
                        data1.AddNewSlVectorDebug(pointOnSliceVec, sliceDir, Color.green);

                }

                data1.CleanUnusedIntersections();
                SlicedMeshLibrary.GenerateMeshes(mf, mr, hit.transform, data1);
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

        if (data1 != null)
            data1.drawOnGizmos();

    }
}
