using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float zoomSpeed = 1f;
    public float minZoom = 2f;
    public float maxZoom = 15f;
    private Vector3 offset;
    private Camera cam;
    void Start()
    {
        offset = transform.position - target.position;
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3);

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            ZoomIn();
        }
        else if (Input.GetKey(KeyCode.KeypadMinus))
        {
            ZoomOut();
        }
    }
    private void ZoomIn()
    {
        float newZoom = cam.orthographicSize - zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    private void ZoomOut()
    {
        float newZoom = cam.orthographicSize + zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

}
