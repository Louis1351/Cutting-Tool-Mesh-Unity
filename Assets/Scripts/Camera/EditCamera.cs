using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditCamera : MonoBehaviour
{
   
    private Camera currentCam;

    [SerializeField] Transform target;
    [SerializeField]
    Vector3 offSet;
    [SerializeField]
    float speedMovement = 10.0f;
    [SerializeField]
    float scrollSpeed = 5.0f;
    [SerializeField]
    float zoomMin = 5.0f;
    [SerializeField]
    float zoomMax = 20.0f;
    [SerializeField]
    float angleMax = 89.0f;
    [SerializeField]
    float damping = 10.0f;
    [SerializeField]
    float XrotationSpeed = 70.0f;
    [SerializeField]
    float YrotationSpeed = 70.0f;

    Vector3 destination;

    float rotX = 0.0f;
    float rotY = 0.0f;

    float correctedOffsetZ;

    public Camera CurrentCam { get => currentCam; }

    // Start is called before the first frame update
    void Start()
    {
        currentCam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        CalculateNewPosition();
        if (Input.GetMouseButton(2))
            RotateAroundTarget();
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, destination, speedMovement);
        currentCam.transform.LookAt(target.transform.position);
    }

    void RotateAroundTarget()
    {
        rotX += Input.GetAxis("Mouse X") * XrotationSpeed;
        rotY += Input.GetAxis("Mouse Y") * YrotationSpeed;
        rotY = Mathf.Clamp(rotY, -angleMax, angleMax);
    }
    void Zoom()
    {
        offSet.z -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        offSet.z = Mathf.Clamp(offSet.z, zoomMin, zoomMax);
    }
    void CalculateNewPosition()
    {
        destination = Quaternion.Euler(rotY, rotX, 0.0f) * Vector3.forward * offSet.z + target.transform.position;
    }
}
