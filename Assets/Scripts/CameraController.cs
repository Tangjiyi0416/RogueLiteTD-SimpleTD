using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float panSpeed = 5f;
    public float secondPanSpeed = 10f;
    public float zoomSpeed = 100f;
    public float zoomMin = -3f;
    public float zoomMax = 5f;
    public int borderWidth = 12;
    [Range(0, 1f)]
    public float panSmoothness = 0f;
    [Range(0, 1f)]
    public float zoomSmoothness = 0f;
    Vector3 targetPos;
    float targetZoom = 0;
    float currentZoom = 0;
    private void Start()
    {
        targetPos = transform.position;
        cameraTransform = transform.GetChild(0).GetComponent<Transform>();
    }
    void Update()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        float speed = GameManager.Instance.gameDeltaTime * (Input.GetKey(KeyCode.LeftShift) ? secondPanSpeed : panSpeed);

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - borderWidth)
        {
            targetPos.z += speed;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= borderWidth)
        {
            targetPos.x -= speed;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= borderWidth)
        {
            targetPos.z -= speed;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - borderWidth)
        {
            targetPos.x += speed;
        }

        if (targetPos == transform.position) return;
        if (panSmoothness == 0)
            transform.position = targetPos;
        else
            transform.position = Vector3.Lerp(transform.position, targetPos, Mathf.Pow(5 * panSmoothness + 2.3f, -3));

    }
    private void Zoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            targetZoom += Input.mouseScrollDelta.y * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, zoomMin, zoomMax);
        }
        if (targetZoom == currentZoom) return;
        if (zoomSmoothness == 0)
            currentZoom = targetZoom;
        else
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Mathf.Pow(5 * zoomSmoothness + 2.3f, -3));
        cameraTransform.localPosition = currentZoom * cameraTransform.forward;
    }
}
