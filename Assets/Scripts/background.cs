using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour {

    public GameObject[] frames;
    private int currentFrame = 0;
    private int counterFrame = 0;
    public int frameRate = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (counterFrame > frameRate)
        {
            frames[currentFrame].SetActive(false);
            if (currentFrame == frames.Length -1) currentFrame = 0;
            else currentFrame++;
            frames[currentFrame].SetActive(true);

            counterFrame = 0;
        }

        counterFrame++;
	}
}
