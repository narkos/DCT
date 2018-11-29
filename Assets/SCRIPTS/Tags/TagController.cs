using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TagController : MonoBehaviour {
    private SocketManager socketManager;
	void Start () {
        socketManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SocketManager>();
	}

	// Update is called once per frame
	void Update () {

	}

    public void TriggerTag(string uid)
    {
        print (uid);
        if(socketManager != null)
        {
            socketManager.TriggerTag(uid);
            return;
        }
        print("[ERROR] No socket manager found");
    }

}
