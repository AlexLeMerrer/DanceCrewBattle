using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject Qtue;
    public GameObject[] controls;
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
        if (counter > 60 * 5)
        {
            next = GameObject.Instantiate(controls[0]);
            next.transform.SetParent(Qtue.transform);
            next.transform.position = Qtue.transform.FindChild("Start").gameObject.transform.position;
            waitingInput.Add(next);
            counter = 0;
        }
        counter++;
        
        if (waitingInput.Count > 0)
        {
            foreach (var control in controls)
            {
                control.transform.position = Vector3.Lerp(next.transform.position, Qtue.transform.FindChild("Perfect").gameObject.transform.position, 2.0f);
            }
        }
	}
}
