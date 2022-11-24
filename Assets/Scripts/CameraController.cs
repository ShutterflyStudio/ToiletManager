using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    public float movementTime;
    private bool isSelected;
    public Vector3 zoomAmount;
    private Vector3 targetPosition;
    private Vector3 targetZoom;
    private Transform camera;
    private CinemachineVirtualCamera cmvcam;

    private void Start()
    {
        camera = Camera.main.transform;
        cmvcam = GetComponentInChildren<CinemachineVirtualCamera>();
        targetPosition = transform.position;
        targetZoom = camera.localPosition;


    }

    private void Update()
    {
        HandleInput();

        if (Input.GetKey(KeyCode.R))
        {
            targetZoom += zoomAmount;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            targetZoom -= zoomAmount;
        }
        camera.localPosition = Vector3.Lerp(camera.localPosition, targetZoom, Time.deltaTime * movementTime);

    }

    private void SelectToilet(Transform _target)
    {
        cmvcam.m_Follow = _target;
    }

    private void Deselect()
    {
        cmvcam.m_Follow = null;
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch finger0 = Input.GetTouch(0);

            if (finger0.deltaPosition.y > 0)
                targetPosition += (transform.forward * cameraSpeed);
            else if (finger0.deltaPosition.y < 0)
                targetPosition += (transform.forward * -cameraSpeed);
            else if (finger0.deltaPosition.x > 0)
                targetPosition += (transform.right * cameraSpeed);
            else if (finger0.deltaPosition.x < 0)
                targetPosition += (transform.right * -cameraSpeed);

            if (Input.touchCount == 2)
            {
                Touch finger1 = Input.GetTouch(0);
                Touch finger2 = Input.GetTouch(1);

                Vector2 firstTouchPrevPos = finger1.position - finger1.deltaPosition;
                Vector2 secondTouchPrevPos = finger2.position - finger2.deltaPosition;

                float prevMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
                float curMagnitude = (finger1.position - finger2.position).magnitude;

                float distance = curMagnitude - prevMagnitude;

                targetZoom *= distance;
            }

            if (Input.touchCount != 1)
            {
                isSelected = false;
                return;
            }

            if (finger0.phase == TouchPhase.Began)
            {
                Vector3 position = finger0.position;

                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider.CompareTag("Toilet"))
                    {
                        SelectToilet(hit.transform.parent);
                    }else { Deselect(); }
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementTime * Time.deltaTime);
            camera.localPosition = Vector3.Lerp(camera.localPosition, targetZoom, Time.deltaTime * movementTime);
        }
    }
}
