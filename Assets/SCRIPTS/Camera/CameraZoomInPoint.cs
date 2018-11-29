using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomInPoint : MonoBehaviour {
    private Collider coll;
    private CameraManager camManager;
	void Start () {
		camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
        coll = GetComponent<Collider>();
	}

    void OnTriggerEnter (Collider other)
    {
        print("trigger" + other.gameObject.tag);
        if (camManager != null && other.gameObject.tag == "Player" && coll != null)
        {
            camManager.SetBirdsEyeTarget(coll);
        }
    }
    void OnTriggerExit (Collider other)
    {
        if (camManager != null && other.gameObject.tag == "Player" && coll != null)
        {
            camManager.SetBirdsEyeTarget();
        }
    }
}
