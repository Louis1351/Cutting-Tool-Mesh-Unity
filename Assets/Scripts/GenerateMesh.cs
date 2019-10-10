using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMesh : MonoBehaviour
{
    [SerializeField]
    Material material;

    [SerializeField]
    Vector3 Scaling;

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
       
        customMesh.CreateCube(Scaling);
        customMesh.AssignToMesh(meshFilter);
        customMesh.AssignToSharedMesh(meshCol);
    }

    //Changing colors example 
    /*void Update()
    {
        for (int i = 0; i < customMesh.vertices.Count; i++)
        {
            customMesh.colors[i] = Color.black;
        }
        customMesh.Recalculate();
        meshFilter.mesh = customMesh.mesh;
    }*/
}
