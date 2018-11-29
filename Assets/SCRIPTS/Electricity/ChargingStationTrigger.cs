using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStationTrigger : MonoBehaviour {

    private ChargingStationController controller;
    private GameManager gameManager;
	// Use this for initialization
	void Start () {
		controller = GetComponentInParent<ChargingStationController>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && controller != null)
        {
            controller.SwitchState(ChargingStationController.StationState.Proximity);
        }
        if(gameManager != null)
        {
            gameManager.SetChargingStation(controller);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player" && controller != null)
        {
            controller.SwitchState(ChargingStationController.StationState.Idle);
        }
        if(gameManager != null)
        {
            gameManager.SetChargingStation(null);
        }
    }
}
