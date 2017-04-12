using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Qtue : MonoBehaviour {

    
    private GameObject perfect;
    public string currentPlayer;
    private float normalWidth = 50.0f;
    private float perfectWidth = 20.0f;
    private float scaleNumber = 1.0f;
    private RectTransform rectTransform;
    private float startTime;
    public float speed = 3.0f;
    public bool isMoving = false;
    // Use this for initialization
    void Start () {
        perfect = gameObject.transform.parent.FindChild("Perfect").gameObject;
        rectTransform = gameObject.GetComponent<RectTransform>();
        LevelManager.manager.onGameOver.AddListener(StopMove);
        
    }

	
	// Update is called once per frame
	void Update () {
        if(!isMoving) Move();
    }

    void StopMove()
    {
        isMoving = true;
    }

    private void Move()
    {

        rectTransform.transform.localPosition += Vector3.up * speed;
        
        if (rectTransform.transform.localPosition.y >= perfect.GetComponent<RectTransform>().transform.localPosition.y - perfectWidth
            && rectTransform.transform.localPosition.y <= perfect.GetComponent<RectTransform>().transform.localPosition.y + perfectWidth
            && Input.GetButtonDown(currentPlayer + gameObject.tag))    
        {
            Destroy(gameObject);
            scaleNumber = 1.0f;
            QTUEManager.instance.InvokeScale(scaleNumber, gameObject.transform.parent.gameObject);
        }
        else if (rectTransform.transform.localPosition.y >= perfect.GetComponent<RectTransform>().transform.localPosition.y - normalWidth
            && rectTransform.transform.localPosition.y <= perfect.GetComponent<RectTransform>().transform.localPosition.y + normalWidth
            && Input.GetButtonDown(currentPlayer + gameObject.tag))
        {

            Destroy(gameObject);
            scaleNumber = .5f;
            QTUEManager.instance.InvokeScale(scaleNumber, gameObject.transform.parent.gameObject);

        }
        else if (rectTransform.transform.localPosition.y > perfect.GetComponent<RectTransform>().transform.localPosition.y + 150)
        {
            Destroy(gameObject, .1f);
            scaleNumber = 0f;
            QTUEManager.instance.InvokeScale(scaleNumber, gameObject.transform.parent.gameObject);
        }
    }
}
