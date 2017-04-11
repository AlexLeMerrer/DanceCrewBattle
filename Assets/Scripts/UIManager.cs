﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject[] Qtues;
    public GameObject[] controls;
    public static Text Text1;
    public static Text Text2;
    public float speed;
    private List<GameObject> waitingInput;
    private int counter = 0;
    private GameObject next;
	// Use this for initialization
	void Start () {
        waitingInput = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (counter > 60 * 2)
        {
            foreach (var lQtue in Qtues)
            {
                next = GameObject.Instantiate(controls[Random.Range(0,controls.Length)]);
                next.AddComponent<Qtue>();
                next.transform.SetParent(lQtue.transform);
                next.transform.position = lQtue.transform.FindChild("Start").gameObject.transform.position;
                waitingInput.Add(next);
            }
            
            counter = 0;
        }
        counter++;
	}

    public static void InputReaction(GameObject pParent,string pReaction)
    {
        Text lText = UIManager.Text1;
        if(pParent.name == "QTE 2")
        {
            lText = UIManager.Text2;
        }
        lText.gameObject.SetActive(true);
        lText.text = pReaction;
    }
}
