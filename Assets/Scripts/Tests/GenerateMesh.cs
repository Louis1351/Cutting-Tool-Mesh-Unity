using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMesh : MonoBehaviour
{
    [SerializeField]
    Material material = null;

    [SerializeField]
    Vector3 Scaling = Vector3.one;

    private CustomMesh customMesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRd;
    private MeshCollider meshCol;
    private Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        customMesh = new CustomMesh();

        meshRd = gameObject.AddComponent<MeshRenderer>();
        meshCol = gameObject.AddComponent<MeshCollider>();
        meshCol.convex = true;
        rb = gameObject.GetComponent<Rigidbody>();
        meshRd.material = material;

        meshFilter = gameObject.AddComponent<MeshFilter>();

        ///Example cube creation///
        /*customMesh.CreateCube(Scaling);
        customMesh.AssignToMesh(meshFilter);
        customMesh.AssignToSharedMesh(meshCol);*/
    }
}
