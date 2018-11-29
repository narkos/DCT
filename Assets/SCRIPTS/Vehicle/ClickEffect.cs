using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitAndKill());
	}
    IEnumerator WaitAndKill()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
