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



    //equation plane N1x(x - xA) + N1y(y - yA) + N1z(z - zA) + d1 = 0 | A e plane
    //equation plane N2x(x - xB) + N2y(y - yB) + N2z(z - zB) + d2 = 0 | B e plane
    //where X = 0.0f
    //find intersection point p with two planes
    //Debug.Log("x1 " + x1 + " y1 " + y1 + " z1 " + z1 + " d1 " + d1);
    //Debug.Log("x2 " + x2 + " y2 " + y2 + " z2 " + z2 + " d2 " + d2);
    public static bool IntersectionPlanToPlan(ref Vector3 intersection, ref Vector3 sliceDir, SlicePlane plane1, SlicePlane plane2)
    {
        sliceDir = Vector3.Cross(plane1.plane.normal, plane2.plane.normal);

        float x1 = plane1.x;
        float y1 = plane1.y;
        float z1 = plane1.z;
        float d1 = plane1.d;

        float x2 = plane2.x;
        float y2 = plane2.y;
        float z2 = plane2.z;
        float d2 = plane2.d;

        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        if (sliceDir.x != 0.0f)
        {
            x = 0.0f;
            z = ((y2 / y1) * d1 - d2) / (z2 - z1 * y2 / y1);
            y = (-z1 * z - d1) / y1;
        }
        else if (sliceDir.y != 0.0f)
        {
            y = 0.0f;
            z = ((x2 / x1) * d1 - d2) / (z2 - z1 * x2 / x1);
            x = (-z1 * z - d1) / x1;
        }
        else if (sliceDir.z != 0.0f)
        {
            z = 0.0f;
            y = ((x2 / x1) * d1 - d2) / (y2 - y1 * x2 / x1);
            x = (-y1 * y - d1) / x1;
        }
        else
            return false;

        intersection.x = x;
        intersection.y = y;
        intersection.z = z;

        return true;
    }
    public static bool IntersectionVectorToVector(out Vector3 ptIntersection, Vector3 A1, Vector3 A2, Vector3 B1, Vector3 vB)
    {
        float u = 0.0f;
        Vector3 vA = A2 - A1;
        ptIntersection = Vector3.zero;

        float det = (vA.y * vB.x - vA.x * vB.y);
        u = (vA.x * (B1.y - A1.y) + vA.y * (A1.x - B1.x)) / det;

        if (det == 0)
        {
            det = (vA.z * vB.y - vA.y * vB.z);
            u = (vA.y * (B1.z - A1.z) + vA.z * (A1.y - B1.y)) / det;
        }

        if (det == 0)
        {
            det = (vA.z * vB.x - vA.x * vB.z);
            u = (vA.x * (B1.z - A1.z) + vA.z * (A1.x - B1.x)) / det;
        }

        if (det == 0)
            return false;

        ptIntersection.x = B1.x + u * vB.x;
        ptIntersection.y = B1.y + u * vB.y;
        ptIntersection.z = B1.z + u * vB.z;

        float dist = vA.sqrMagnitude;
        float dist1 = (A1 - ptIntersection).sqrMagnitude;
        float dist2 = (A2 - ptIntersection).sqrMagnitude;

        if (dist1 < dist && dist2 < dist)
            return true;
        else
            return false;

        /*pA.x+t*vA.x = pB.x+u*vB.x
          pA.y+t*vA.y = pB.y+u*vB.y
          pA.z+t*vA.z = pB.z+u*vB.z

        t = (pB.x+u*vB.x - pA.x)/vA.x
        vA.y*(pB.x+u*vB.x - pA.x)/vA.x- u*vB.y = pB.y - pA.y
        vA.y*(pB.x+u*vB.x - pA.x) - vA.x*u*vB.y = vA.x*(pB.y - pA.y)
        vA.y*pB.x + vA.y*u*vB.x - vA.y*pA.x - vA.x*u*vB.y = vA.x*(pB.y - pA.y)
        vA.y*u*vB.x - vA.x*u*vB.y = vA.x*(pB.y - pA.y) + vA.y (pA.x - pB.x)
        u*(vA.y*vB.x -vA.x*vB.y) = ""
        u = (vA.x*(pB.y - pA.y) + vA.y (pA.x - pB.x))/(vA.y*vB.x - vA.x*vB.y)*/
    }
}

