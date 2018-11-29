
using UnityEngine;
using UnityEngine.AI;

public class VehicleControls : MonoBehaviour
{
    public enum ControlType { Keyboard, Mouse, Off }
    private NavMeshAgent agent;
    private Camera cam;
    private ControlType controlType = ControlType.Keyboard;

    public GameObject hitEffect;
    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        agent = GetComponent<NavMeshAgent>();
        controlType = ControlType.Off;
    }

    void Update()
    {
        HandleMouseControls();
    }


    private void HandleMouseControls ()
    {
        if (Input.GetMouseButtonDown(0) && agent.isActiveAndEnabled && controlType == ControlType.Mouse)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                if(hitEffect != null)
                {
                    Vector3 fxPosition = hit.point;
                    fxPosition.y += 1.0f;
                    Instantiate(hitEffect, fxPosition, Quaternion.identity);
                }
            }
        }
    }

    public void SetControlType(ControlType type)
    {
        controlType = type;
        if (type == ControlType.Off)
        {
            agent.ResetPath();
        }
    }
}
