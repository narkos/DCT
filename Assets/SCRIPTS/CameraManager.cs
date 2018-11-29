using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public enum CameraMode { BirdsEye, Orbit }
    private CameraMode cameraMode = CameraMode.BirdsEye;
    public BoxCollider worldCollider;
    private Camera cam;
    private Quaternion worldViewRotation = Quaternion.Euler(90, 0, 0);
    private Vector3 worldViewPosition;
    public float worldViewZoom = 50.0f;

    // Orbit
    private Vector3 orbitCenter = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 orbitAxis = Vector3.up;
    public float orbitRadius = 120.0f;
    public float orbitRadiusCorrectionSpeed = 0.5f;
    public float orbitRotationSpeed = 10.0f;
    public float orbitAlignToDirectionSpeed = 0.5f;


    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        GetWorldPosition(worldCollider);
    }


    void Update()
    {
        if (cameraMode == CameraMode.BirdsEye)
        {
            LerpBirdsEye();
        }
        else
        {
            OrbitCamera();
        }
    }

    void LerpBirdsEye()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, worldViewRotation, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, new Vector3(worldViewPosition.x, worldViewPosition.y, worldViewPosition.z), Time.deltaTime);
    }

    void GetWorldPosition(Collider collider)
    {
        if (collider != null)
        {
            float radius = (collider.bounds.max - collider.bounds.center).magnitude;
            float fov = cam.fieldOfView;
            float d = radius / Mathf.Sin(Mathf.Deg2Rad * (fov * 0.5f));
            float worldDistance = d + cam.nearClipPlane;
            worldViewPosition = new Vector3(collider.transform.position.x, collider == worldCollider ? worldDistance - worldViewZoom : worldDistance, collider.transform.position.z);
        }
    }

    public void SetCameraMode(CameraMode mode)
    {
        cameraMode = mode;
    }

    public void SetBirdsEyeTarget(Collider collider = null)
    {
        if (collider != null)
        {
            GetWorldPosition(collider);
        }
        else
        {
            GetWorldPosition(worldCollider);
        }
    }

    private void OrbitCamera()
    {
        transform.RotateAround(orbitCenter, orbitAxis, orbitRotationSpeed * Time.deltaTime);
        Vector3 desiredPosition = (transform.position - orbitCenter).normalized * orbitRadius + orbitCenter;
        desiredPosition = Vector3.Slerp(transform.position, desiredPosition, Time.deltaTime * orbitRadiusCorrectionSpeed);
        desiredPosition = new Vector3(desiredPosition.x, Mathf.Max(desiredPosition.y, 10.0f), desiredPosition.z);
        transform.position = desiredPosition;

        transform.LookAt(orbitCenter, Vector3.up);
    }
}
