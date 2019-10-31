using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedMeshLibrary : MonoBehaviour
{
    private CustomMesh mesh;

    public static void GenerateLeftMesh(MeshFilter oldMesh, Transform trans, Plane plane, List<SlicePlane.SliceVector> intersectionPoints)
    {
        List<int> verticesIndex = new List<int>();

        for (int i = 0; i < oldMesh.mesh.vertexCount; i++)
        {

            Vector3 vertex = trans.TransformPoint(oldMesh.mesh.vertices[i]);
            if (plane.GetSide(vertex))
            {
                if (!verticesIndex.Contains(i))
                    verticesIndex.Add(i);
            }
        }
    }

    public static void GenerateRightMesh(MeshFilter oldMesh, Transform trans, Plane plane, List<SlicePlane.SliceVector> intersectionPoints)
    {
        List<int> verticesIndex = new List<int>();

        for (int i = 0; i < oldMesh.mesh.vertexCount; i++)
        {

            Vector3 vertex = trans.TransformPoint(oldMesh.mesh.vertices[i]);
            if (plane.GetSide(vertex))
            {
                if (!verticesIndex.Contains(i))
                    verticesIndex.Add(i);
            }
        }
    }
}
