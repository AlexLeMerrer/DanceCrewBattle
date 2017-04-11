using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Qtue : MonoBehaviour {

    private GameObject perfect;
    private RectTransform rectTransform;
    private float startTime;
    public float speed = 2.0f;
    // Use this for initialization
    void Start () {
        perfect = gameObject.transform.parent.FindChild("Perfect").gameObject;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    private void Move()
    {

        rectTransform.transform.localPosition += Vector3.down * speed;
        if (rectTransform.transform.localPosition.y < perfect.GetComponent<RectTransform>().transform.localPosition.y - 150)
        {
            //UIManager.InputReaction(gameObject.transform.parent.gameObject, "Miss !");
            Destroy(gameObject, .1f);
        }
    }
}
