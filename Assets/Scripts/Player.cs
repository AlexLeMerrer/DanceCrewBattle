﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GAF.Core;

public class Player : MonoBehaviour {

    public delegate void State();
    public State behaviour;

    private static Player Player1;
    private static Player Player2;

    //Liste des joueurs
    //public static List<Player> list = new List<Player>();
    public float velocity = 3;
    public float gainInfluence = 1f;
    public InfluenceCircle influenceAsset;
    public GAFMovieClip animationAsset;
    
    // TO MOVE
    private float topLimit;
    private float bottomLimit;
    private float rightLimit;
    private float leftLimit;

    private float toleranceMove = .1f;

    void Awake()
    {
        QTUEManager.instance.scalefactorP1.AddListener(scaleCircle);
    }

    void Start () {

        if (Player2 != null)    Player1 = this;
        else                    Player2 = this;
        
        topLimit =      LevelManager.manager.GetComponent<RectTransform>().rect.height/2 - GetComponent<Renderer>().bounds.size.y;
        bottomLimit =   -LevelManager.manager.GetComponent<RectTransform>().rect.height / 2 + GetComponent<Renderer>().bounds.size.y;
        rightLimit =    LevelManager.manager.GetComponent<RectTransform>().rect.width / 2 - GetComponent<Renderer>().bounds.size.x;
        leftLimit =     -LevelManager.manager.GetComponent<RectTransform>().rect.width/2 + GetComponent<Renderer>().bounds.size.x;

        setModeIdle();

        if (ControllerManager.instance != null && Player1 == this)
            ControllerManager.instance.onAxis1.AddListener(ControlMove);
        else if(ControllerManager.instance != null)
            ControllerManager.instance.onAxis2.AddListener(ControlMove);
    }
	
	void Update ()
    {
        CheckInfluence();
        behaviour();
    }

    #region State Machine
        #region idle
        private void setModeIdle()
        {
            behaviour = Idle;
            //animationAsset.GetComponent<GAFMovieClip>().setSequence("idle", true);
        }
        private void Idle()
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                setModeMove();
        }
        #endregion
        #region Move
        private void setModeMove()
        {
            behaviour = Move;
            animationAsset.GetComponent<GAFMovieClip>().setSequence("move", true);

        }
        private void Move()
        {   
            //if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
            //  setModeIdle();
        }
        #endregion
    #endregion

    #region Methods
    private void ControlMove(float pHorizontal, float pVertical)
    {
        Vector3 newPos = transform.position + Time.fixedDeltaTime * velocity * new Vector3(pHorizontal, pVertical, 0);
        transform.position = new Vector3(Mathf.Clamp(newPos.x, leftLimit, rightLimit),
                                         Mathf.Clamp(newPos.y, bottomLimit, topLimit),
                                         newPos.z);
    }


    // PROVISOIRE
    private void CheckInfluence()
    {
        if(Input.GetButtonDown("Fire1"))
            influenceAsset.SetModeGrow(gainInfluence);
    }

    private void scaleCircle(float scaleFactor)
    {
        influenceAsset.SetModeGrow(scaleFactor);
    }
    
    #endregion

}
