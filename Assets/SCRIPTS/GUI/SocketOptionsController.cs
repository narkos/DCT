using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocketOptionsController : MonoBehaviour {
    public InputField urlField;
    private GameManager gameManager;
	// Use this for initialization
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }
    void OnEnable()
    {
        if (gameManager == null)
        {
            gameManager = GetComponentInParent<GameManager>();
        }
        if (gameManager != null && urlField != null)
        {
            print("setts");
            urlField.text = gameManager.GetSimulatorURL();
        }
    }

}
