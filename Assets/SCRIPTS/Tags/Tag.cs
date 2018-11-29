using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour
{
    private TagController tagController;
    public string id;
    public string title;
    public string uid;

    // Use this for initialization
    void Start()
    {
        tagController = GameObject.FindGameObjectWithTag("TagController").GetComponent<TagController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && uid != "")
        {
            tagController.TriggerTag(uid);
        }
    }
}
