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
        //Used only to show the intersection lines
        data.ShowDebugLines = showDebugLines;

        //Get the last position of the mouse
        if (!hasClicked && Input.GetMouseButtonDown(lftBtn))
        {
            hasClicked = true;
            lastMousePos = Input.mousePosition;
        }

        //Try to cut the mesh
        if (hasClicked && Input.GetMouseButtonUp(lftBtn))
        {
            hasClicked = false;
            Vector3 sliceCenter = (lastMousePos + Input.mousePosition) / 2.0f;

            RaycastHit hit0, hit1, hit2;
            Ray rayCenter = Camera.main.ScreenPointToRay(sliceCenter);
            Ray rayP1 = Camera.main.ScreenPointToRay(lastMousePos);
            Ray rayP2 = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(rayP1, out hit1);
            Physics.Raycast(rayP2, out hit2);

            //Find if an object can be cut or not
            if (Physics.Raycast(rayCenter, out hit0)
                && hit1.transform != hit0.transform
                && hit2.transform != hit0.transform)
            {
                data.Clear();

                center = hit0.point;
                data.CtmPlane.Set3Points(
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)),
                    Camera.main.ScreenToWorldPoint(new Vector3(lastMousePos.x, lastMousePos.y, 1.0f) + Camera.main.transform.forward));

                MeshFilter mf = hit0.transform.GetComponent<MeshFilter>();
                MeshRenderer mr = hit0.transform.GetComponent<MeshRenderer>();

                SlicedMeshLibrary.GenerateMeshes(mf, mr, data, showDebugLines);
            }

        }

    }

    /// <summary>
    /// DRAW EVERYTHING 
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
