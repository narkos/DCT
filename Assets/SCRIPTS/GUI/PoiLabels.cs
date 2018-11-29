using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PoiLabels : MonoBehaviour {

    public string label = "Label";
    public Vector2 rectSize = new Vector2(200.0f, 100.0f);
    public Vector2 offset;
    public GUIStyle labelStyle;
    private Rect rect;
	// Update is called once per frame
    void Start()
    {
        rect = new Rect(0, 0, 200.0f, 100.0f);
    }
	void Update () {

	}

    void OnGUI()
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position + new Vector3(offset.x, offset.y, 0.0f));
        //rect.position = new Vector2(screenPoint.x - offset.x - rectSize.x, Screen.height - screenPoint.y - offset.y);

        GUI.Label(new Rect(screenPoint.x - rectSize.x, Screen.height - screenPoint.y, rectSize.x, rectSize.y), label, labelStyle);
        //GUI.Label(new Rect(screenPoint.x - offset.x - rectSize.x, Screen.height - screenPoint.y - offset.y, rectSize.x, rectSize.y), label, labelStyle);
    }

}
