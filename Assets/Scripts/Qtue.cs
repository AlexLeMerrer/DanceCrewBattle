using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Qtue : MonoBehaviour {

    
    private GameObject perfect;
    public GameObject prefabRose;
    public GameObject prefabBleu;
    public string currentPlayer;
    private float normalWidth = 50.0f;
    private float perfectWidth = 20.0f;
    private float scaleNumber = 1.0f;
    private RectTransform rectTransform;
    private float startTime;
    public float speed = 3.0f;
    public bool isMoving = false;

    private bool isActive;
    // Use this for initialization
    void Start () {
        perfect = gameObject.transform.parent.FindChild("Perfect").gameObject;
        rectTransform = gameObject.GetComponent<RectTransform>();
        LevelManager.manager.onGameOver.AddListener(StopMove);
        isActive = true;
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
            if (currentPlayer == "P1_")
            {
                Instantiate(prefabRose);
                Destroy(prefabRose, 0.5f);
            }
            else
            {
                Instantiate(prefabBleu);
                Destroy(prefabBleu, 0.5f);
            }
            Destroy(gameObject,0.5f);
            scaleNumber = .1f;

            if (gameObject.transform.parent.gameObject.name == "QTE 1")
                UIManager.manager.SetQteMsg(0, true);
            else
                UIManager.manager.SetQteMsg(0, false);

            QTUEManager.instance.InvokeScale(scaleNumber, gameObject.transform.parent.gameObject);
        }
        else if (rectTransform.transform.localPosition.y >= perfect.GetComponent<RectTransform>().transform.localPosition.y - normalWidth
            && rectTransform.transform.localPosition.y <= perfect.GetComponent<RectTransform>().transform.localPosition.y + normalWidth
            && Input.GetButtonDown(currentPlayer + gameObject.tag))
        {

            Destroy(gameObject);
            scaleNumber = .05f;

            if (gameObject.transform.parent.gameObject.name == "QTE 1")
                UIManager.manager.SetQteMsg(1, true);
            else
                UIManager.manager.SetQteMsg(1, false);

            QTUEManager.instance.InvokeScale(scaleNumber, gameObject.transform.parent.gameObject);

        }
        else if (rectTransform.transform.localPosition.y > perfect.GetComponent<RectTransform>().transform.localPosition.y + 150 && isActive)
        {
            isActive = false;
            Destroy(gameObject, .1f);
            scaleNumber = -0.2f;

            if(gameObject.transform.parent.gameObject.name == "QTE 1")
                UIManager.manager.SetQteMsg(2, true);
            else
                UIManager.manager.SetQteMsg(2, false);

            QTUEManager.instance.InvokeScale(scaleNumber, gameObject.transform.parent.gameObject);


        }
    }
}
