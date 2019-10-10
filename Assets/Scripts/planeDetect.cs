using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeDetect : MonoBehaviour {

    Color[] colors;
	// Use this for initialization
	void Start () {
        
        colors = new Color[24];

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            MeshFilter otherMesh = other.GetComponent<MeshFilter>();
            
            for (int i = 0; i < otherMesh.mesh.vertices.Length;i++)
            {
                Vector3 vertexInWorld = other.transform.TransformPoint(otherMesh.mesh.vertices[i]);
                
                
                if (Vector3.Dot(gameObject.transform.up, vertexInWorld) < 0.0f)
                {
                    colors[i] = Color.red;
                }
                else colors[i] = Color.green;
            }
            otherMesh.mesh.colors = colors; 
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        /*foreach (Ray r in edges)
        {
            Gizmos.DrawRay(r);
        }*/
    }
}
