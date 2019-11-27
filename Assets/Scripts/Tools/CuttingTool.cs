using UnityEngine;

public class CuttingTool : MonoBehaviour
{
    [SerializeField]
    Material mat = null;

    [SerializeField]
    bool showDebugLines = false;

    private SliceData data;

    private Vector3 lastMousePos;
    private Vector3 center;

    private bool hasClicked;
    private int lftBtn;

    // Use this for initialization
    void Start()
    {
        data = new SliceData();
        hasClicked = false;

        lastMousePos = Vector2.zero;
        lftBtn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        data.ShowDebugLines = showDebugLines;
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
                data.Clear();

                center = hit.point;
                data.CtmPlane.Set3Points(
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f) + Camera.main.transform.forward));

                MeshFilter mf = hit.transform.GetComponent<MeshFilter>();
                MeshRenderer mr = hit.transform.GetComponent<MeshRenderer>();

                SlicedMeshLibrary.GenerateMeshes(mf, mr, hit.transform, data);
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
        if (data != null)
            data.drawOnGizmos();
    }
}
