using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlicedMeshLibrary
{

    public static void GenerateLeftMesh(MeshFilter oldMeshF, MeshRenderer oldMeshR, Transform tr, SliceData dataPlane)
    {
        GameObject newMesh;
        CustomMesh mesh = new CustomMesh(out newMesh, "left Mesh", tr, oldMeshR.material);
        //Debug.Log("before nb left vertices " + dataPlane.slVectorsLeft.Count);
        List<SliceData.SliceVector> allVectors = dataPlane.SlVectorsIntersec.Concat(dataPlane.SlVectorsLeft).ToList();
        Dictionary<int, List<int>> VerticesTriangles = new Dictionary<int, List<int>>();

        for (int i = 0; i < allVectors.Count; i++)
        {
            mesh.vertices.Add(tr.InverseTransformPoint(allVectors[i].point));
            //Debug.Log("//////////////");
            foreach (int triangleID in allVectors[i].inTriangles)
            {
                //Debug.Log(triangleID);
                List<int> ltmp = null;

                if (VerticesTriangles.ContainsKey(triangleID))
                    ltmp = VerticesTriangles[triangleID];

                if (ltmp == null)
                    ltmp = new List<int>();

                ltmp.Add(i);

                if (!VerticesTriangles.ContainsKey(triangleID))
                    VerticesTriangles.Add(triangleID, ltmp);
                else VerticesTriangles[triangleID] = ltmp;
            }
        }


       
        foreach (KeyValuePair<int, List<int>> keyValue in VerticesTriangles)
        {
            //Debug.Log("//////////////");
            if (keyValue.Value.Count == 3)
            {
                foreach (int vertex in keyValue.Value)
                {
                    //Debug.Log("vertex " + vertex);
                    mesh.triangles.Add(vertex);
                }     
            }
        }

        //Debug.Log("nb left vertices " + allVectors.Count);
        //Debug.Log("nb vertices " + mesh.vertices.Count);
        //Debug.Log("nb triangles " + mesh.triangles.Count);

        mesh.Recalculate();
        mesh.AssignToMesh(newMesh.GetComponent<MeshFilter>());
        mesh.AssignToSharedMesh(newMesh.GetComponent<MeshCollider>());
    }
    public static void GenerateRightMesh(MeshFilter oldMeshF, MeshRenderer oldMeshR, Transform tr, SliceData dataPlane)
    {
        GameObject newMesh;
        CustomMesh mesh = new CustomMesh(out newMesh, "right Mesh", tr, oldMeshR.material);
        //Debug.Log("before nb left vertices " + dataPlane.slVectorsLeft.Count);
        List<SliceData.SliceVector> allVectors = dataPlane.SlVectorsIntersec.Concat(dataPlane.SlVectorsRight).ToList();
        Dictionary<int, List<int>> VerticesTriangles = new Dictionary<int, List<int>>();

        for (int i = 0; i < allVectors.Count; i++)
        {
            mesh.vertices.Add(tr.InverseTransformPoint(allVectors[i].point));
            //Debug.Log("//////////////");
            foreach (int triangleID in allVectors[i].inTriangles)
            {
                //Debug.Log(triangleID);
                List<int> ltmp = null;

                if (VerticesTriangles.ContainsKey(triangleID))
                    ltmp = VerticesTriangles[triangleID];

                if (ltmp == null)
                    ltmp = new List<int>();

                ltmp.Add(i);

                if (!VerticesTriangles.ContainsKey(triangleID))
                    VerticesTriangles.Add(triangleID, ltmp);
                else VerticesTriangles[triangleID] = ltmp;
            }
        }



        foreach (KeyValuePair<int, List<int>> keyValue in VerticesTriangles)
        {
            //Debug.Log("//////////////");
            if (keyValue.Value.Count == 3)
            {
                foreach (int vertex in keyValue.Value)
                {
                    //Debug.Log("vertex " + vertex);
                    mesh.triangles.Add(vertex);
                }
            }
        }

        //Debug.Log("nb left vertices " + allVectors.Count);
        //Debug.Log("nb vertices " + mesh.vertices.Count);
        //Debug.Log("nb triangles " + mesh.triangles.Count);

        mesh.Recalculate();
        mesh.AssignToMesh(newMesh.GetComponent<MeshFilter>());
        mesh.AssignToSharedMesh(newMesh.GetComponent<MeshCollider>());
    }
    ///<summary>
    ///equation plane N1x(x - xA) + N1y(y - yA) + N1z(z - zA) + d1 = 0 | A e plane<para/>
    ///equation plane N2x(x - xB) + N2y(y - yB) + N2z(z - zB) + d2 = 0 | B e plane<para/>
    ///x1  + x1 +  y1  + y1 +  z1  + z1 +  d1  + d1 = x2  + x2 +  y2  + y2 +  z2  + z2 +  d2  + d2<para/>
    ///find intersection point p with two planes<para/>
    ///</summary>
    public static bool IntersectionPlanToPlan(out Vector3 intersection, out Vector3 sliceDir, CustomPlane plane1, CustomPlane plane2)
    {
        sliceDir = Vector3.Cross(plane1.Normal, plane2.Normal);
        intersection = Vector3.zero;
        float x1 = plane1.UnKnowns.x;
        float y1 = plane1.UnKnowns.y;
        float z1 = plane1.UnKnowns.z;
        float d1 = plane1.UnKnowns.w;

        float x2 = plane2.UnKnowns.x;
        float y2 = plane2.UnKnowns.y;
        float z2 = plane2.UnKnowns.z;
        float d2 = plane2.UnKnowns.w;

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
    ///<summary>
    ///Find intersection between two lines<para/>
    ///pA.x+t*vA.x = pB.x+u*vB.x<para/>
    ///pA.y+t*vA.y = pB.y+u*vB.y<para/>
    ///pA.z+t*vA.z = pB.z+u*vB.z<para/>
    ///</summary>
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
    public static bool PointBetweenOthersPoints(Vector3 A, Vector3 B, Vector3 point)
    {
        Vector3 v = B - A;
        Vector3 u = point - A;
        if (Vector3.Cross(u, v) == Vector3.zero)
        {
            if (Vector3.Dot(v, u) > 0)
                return true;
        }
        return false;
    }
}

