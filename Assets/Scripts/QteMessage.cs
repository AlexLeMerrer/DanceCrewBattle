using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QteMessage : MonoBehaviour {

    private float velocity;

	// Use this for initialization
	void Start () {
        velocity = 1f;
        Destroy(gameObject, .5f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        velocity += .05f;
        gameObject.GetComponent<RectTransform>().transform.localPosition += Vector3.up * .75f * velocity;
    }
}
