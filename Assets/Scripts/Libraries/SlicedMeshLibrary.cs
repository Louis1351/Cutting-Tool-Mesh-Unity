using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlicedMeshLibrary
{
    /// <summary>
    /// Create two meshes with an old mesh and a sliceData <para/>
    /// </summary>
    public static void GenerateMeshes(MeshFilter _oldMeshF, MeshRenderer _oldMeshR, SliceData _dataPlane, bool _showDebug = false)
    {
        FindNewTriangles(_oldMeshF, ref _dataPlane, _showDebug);
        GeneratePartMesh(_oldMeshF, _oldMeshR, _dataPlane, true);
        GeneratePartMesh(_oldMeshF, _oldMeshR, _dataPlane, false);

        GameObject.Destroy(_oldMeshF.gameObject);
    }
    /// <summary>
    /// Generate The left/right mesh side of the plane
    /// </summary>
    public static void GeneratePartMesh(MeshFilter _oldMeshF, MeshRenderer _oldMeshR, SliceData _dataPlane, bool _isLeft)
    {
        Debug.Log("/////////////StartGeneratePartMesh//////////////");
        GameObject newMesh;
        string name = (_isLeft ? "left Mesh" : "right Mesh");
        CustomMesh mesh = new CustomMesh(out newMesh, name, _oldMeshF.transform, _oldMeshR.material, false);

        int nbFace = 0;
        //Left faces have odd id in the dictionnary 
        //Right faces have even id in the dictionnary 

        // Debug.Log("faces " + _dataPlane.Faces.Count);
        for (int faceID = (_isLeft ? 1 : 0); faceID < _dataPlane.Faces.Count; faceID += 2)
        {
            if (!_dataPlane.Faces.ContainsKey(faceID))
                continue;

            //Starting to generate the custom mesh

            //Debug.Log("face " + faceID + " triangles " + _dataPlane.Faces[faceID].Triangles.Count);

            foreach (FaceVertex vertex in _dataPlane.Faces[faceID].Vertices)
            {
                Vector3 vertexPos = _oldMeshF.transform.InverseTransformPoint(vertex.pos);
                mesh.vertices.Add(vertexPos);
            }

            foreach (Triangle tr in _dataPlane.Faces[faceID].Triangles)
            {
                // Debug.Log("triangle indices " + tr.Indices.Count);
                if (tr.Indices.Count != 3)
                    continue;
                foreach (int indice in tr.Indices)
                {
                    //Debug.Log("indice is " + indice);
                    mesh.triangles.Add(indice);
                }
            }

        }

        Debug.Log("nb vertices " + mesh.vertices.Count);
        Debug.Log("nb triangles " + mesh.triangles.Count);

        mesh.Recalculate();
        mesh.AssignToMesh(newMesh.GetComponent<MeshFilter>());
        mesh.AssignToSharedMesh(newMesh.GetComponent<MeshCollider>());
        Debug.Log("/////////////EndGeneratePartMesh//////////////");
    }

    ///<summary>
    ///equation plane N1x(x - xA) + N1y(y - yA) + N1z(z - zA) + d1 = 0 | A e plane<para/>
    ///equation plane N2x(x - xB) + N2y(y - yB) + N2z(z - zB) + d2 = 0 | B e plane<para/>
    ///x1  + x1 +  y1  + y1 +  z1  + z1 +  d1  + d1 = x2  + x2 +  y2  + y2 +  z2  + z2 +  d2  + d2<para/>
    ///find intersection point p with two planes<para/>
    ///</summary>
    public static bool IntersectionPlanToPlan(out Vector3 _intersection, out Vector3 _sliceDir, CustomPlane _plane1, CustomPlane _plane2)
    {
        _sliceDir = Vector3.Cross(_plane1.Normal, _plane2.Normal);
        _intersection = Vector3.zero;
        float x1 = _plane1.UnKnowns.x;
        float y1 = _plane1.UnKnowns.y;
        float z1 = _plane1.UnKnowns.z;
        float d1 = _plane1.UnKnowns.w;

        float x2 = _plane2.UnKnowns.x;
        float y2 = _plane2.UnKnowns.y;
        float z2 = _plane2.UnKnowns.z;
        float d2 = _plane2.UnKnowns.w;

        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        if (_sliceDir.x != 0.0f)
        {
            x = 0.0f;
            z = ((y2 / y1) * d1 - d2) / (z2 - z1 * y2 / y1);
            y = (-z1 * z - d1) / y1;
        }
        else if (_sliceDir.y != 0.0f)
        {
            y = 0.0f;
            z = ((x2 / x1) * d1 - d2) / (z2 - z1 * x2 / x1);
            x = (-z1 * z - d1) / x1;
        }
        else if (_sliceDir.z != 0.0f)
        {
            z = 0.0f;
            y = ((x2 / x1) * d1 - d2) / (y2 - y1 * x2 / x1);
            x = (-y1 * y - d1) / x1;
        }
        else
            return false;

        _intersection.x = x;
        _intersection.y = y;
        _intersection.z = z;

        return true;
    }
    ///<summary>
    ///Find intersection between two lines<para/>
    ///pA.x+t*vA.x = pB.x+u*vB.x<para/>
    ///pA.y+t*vA.y = pB.y+u*vB.y<para/>
    ///pA.z+t*vA.z = pB.z+u*vB.z<para/>
    ///</summary>
    public static bool IntersectionVectorToVector(out Vector3 _ptIntersection, Vector3 _A1, Vector3 _A2, Vector3 _B1, Vector3 _vB)
    {
        float u = 0.0f;
        Vector3 vA = _A2 - _A1;
        _ptIntersection = Vector3.zero;

        float det = (vA.y * _vB.x - vA.x * _vB.y);
        u = (vA.x * (_B1.y - _A1.y) + vA.y * (_A1.x - _B1.x)) / det;

        if (det == 0)
        {
            det = (vA.z * _vB.y - vA.y * _vB.z);
            u = (vA.y * (_B1.z - _A1.z) + vA.z * (_A1.y - _B1.y)) / det;
        }

        if (det == 0)
        {
            det = (vA.z * _vB.x - vA.x * _vB.z);
            u = (vA.x * (_B1.z - _A1.z) + vA.z * (_A1.x - _B1.x)) / det;
        }

        if (det == 0)
            return false;

        _ptIntersection.x = _B1.x + u * _vB.x;
        _ptIntersection.y = _B1.y + u * _vB.y;
        _ptIntersection.z = _B1.z + u * _vB.z;

        float dist = vA.sqrMagnitude;
        float dist1 = (_A1 - _ptIntersection).sqrMagnitude;
        float dist2 = (_A2 - _ptIntersection).sqrMagnitude;

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
    /// <summary>
    /// Return true if the point is between A and B
    /// </summary>
    public static bool PointBetweenOthersPoints(Vector3 _A, Vector3 _B, Vector3 _point)
    {
        Vector3 v = _B - _A;
        Vector3 u = _point - _A;
        if (Vector3.Cross(u, v) == Vector3.zero)
        {
            if (Vector3.Dot(v, u) > 0)
                return true;
        }
        return false;
    }
    /// <summary>
    /// Fill the left faces and right faces in the Dictionnary
    /// </summary>
    public static void FindNewTriangles(MeshFilter _mf, ref SliceData _data, bool _showDebug)
    {
        List<Vector3> leftvertices = new List<Vector3>();
        List<Vector3> rightvertices = new List<Vector3>();

        CustomPlane secondPlane = new CustomPlane();
        Vector3 pointOnSliceVec;
        Vector3 sliceDir;
        Vector3 finalPoint;
        Debug.Log("start vertices " + _mf.mesh.vertices.Length);
        Debug.Log("start triangles " + _mf.mesh.triangles.Length);
        int currentIndice1 = 0;
        int currentIndice2 = 0;
        //Check each face of the mesh ( one face possesses minimum two triangles )
        for (int i = 0, FaceID = 0; i < _mf.mesh.triangles.Length; i += 3, FaceID += 2)
        {
            Vector3 V1 = _mf.transform.TransformPoint(_mf.mesh.vertices[_mf.mesh.triangles[i]]);
            Vector3 V2 = _mf.transform.TransformPoint(_mf.mesh.vertices[_mf.mesh.triangles[i + 1]]);
            Vector3 V3 = _mf.transform.TransformPoint(_mf.mesh.vertices[_mf.mesh.triangles[i + 2]]);

            if (_showDebug)
            {
                _data.AddNewSlVectorDebug(V1, Vector3.zero, Color.magenta, false, true);
                _data.AddNewSlVectorDebug(V2, Vector3.zero, Color.magenta, false, true);
                _data.AddNewSlVectorDebug(V3, Vector3.zero, Color.magenta, false, true);
            }

            secondPlane.Set3Points(V1, V2, V3);
            //Initialize two Faces
            Face rightFace = new Face(currentIndice1);
            Face leftFace = new Face(currentIndice2);

            _data.AddFace(FaceID, rightFace);
            _data.AddFace(FaceID + 1, leftFace);

            Edge[] edges = {
                        new Edge(V1, V2),
                        new Edge(V2, V3),
                        new Edge(V3, V1) };
            //if the customPlan doesn't cut the triangle, then in that case don't check intersections 
            if (!IntersectionPlanToPlan(out pointOnSliceVec, out sliceDir, _data.CtmPlane, secondPlane))
            {
                foreach (Edge e in edges)
                {
                    if (!_data.CtmPlane.GetSide(e.Vertices[0]))
                        rightFace.AddVertex(e.Vertices[0]);
                    else leftFace.AddVertex(e.Vertices[0]);
                }
                continue;
            }
            int inter = 0;

            foreach (Edge e in edges)
            {
                if (!_data.CtmPlane.GetSide(e.Vertices[0]))
                    rightFace.AddVertex(e.Vertices[0]);
                else leftFace.AddVertex(e.Vertices[0]);
                //Find the new Vertex which intersects with the triangle
                if (IntersectionVectorToVector(out finalPoint, e.Vertices[0], e.Vertices[1], pointOnSliceVec, sliceDir))
                {
                    if (_showDebug)
                    {
                        _data.AddNewSlVectorDebug(finalPoint, Vector3.zero, Color.magenta, true, false);
                    }


                    
                    rightFace.AddVertex(finalPoint);
                    leftFace.AddVertex(finalPoint);
                    inter++;
                }

                currentIndice1 = rightFace.GetCurrentIndice();
                currentIndice2 = leftFace.GetCurrentIndice();

            }


            if (inter > 0 && _showDebug)
            {
                _data.AddNewSlVectorDebug(pointOnSliceVec, sliceDir, Color.green);
            }
        }

        if (_showDebug)
            _data.CleanUnusedDebugIntersections();
    }

    public static bool IsEqualTo(Vector3 _a, Vector3 _b, float _precision)
    {
        return (Mathf.Abs(_a.x - _b.x) < _precision
            && Mathf.Abs(_a.y - _b.y) < _precision
            && Mathf.Abs(_a.z - _b.z) < _precision);
    }
}

