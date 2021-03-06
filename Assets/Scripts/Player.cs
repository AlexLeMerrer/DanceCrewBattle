﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GAF.Core;

public class Player : MonoBehaviour {

    public delegate void State();
    public State MoveBehavior;
    public State DanceBehavior;

    public static Player Player1;
    public static Player Player2;

    //Liste des joueurs
    public float velocity;
    public InfluenceCircle influenceAsset1;
    public InfluenceCircle influenceAsset2;
    private InfluenceCircle influenceAsset;
    private GameObject animationAsset;
    private GAFMovieClip animation;
    private bool isDancing;
    private bool canPlay = false;

    public GameObject[] assetList;

    // TO MOVE
    private float topLimit;
    private float bottomLimit;
    private float rightLimit;
    private float leftLimit;

    private float toleranceMove = .1f;

    public GameObject p1;
    public GameObject p2;

    private float maxAppeal = 1.0f;
    private float passifLostAppeal = 0.005f;
    private float sexAppeal;
    public float getSexAppeal(){ return sexAppeal;}

    void Awake()
    {

    }

    void Start () {
        GameObject animationAsset;

        if (name == "1")
        {
            Player1 = this;
            animationAsset = Instantiate(assetList[GameManager.instance.player1]); //SELECTION RANDOM J1      
            influenceAsset = influenceAsset1;
            influenceAsset2.gameObject.SetActive(false);
            p1.SetActive(true);
        }
        else
        {
            Player2 = this;
            animationAsset = Instantiate(assetList[GameManager.instance.player2]); //SELECTION RANDOM J2
            influenceAsset = influenceAsset2;
            influenceAsset1.gameObject.SetActive(false);
            p2.SetActive(true);

        }
        
        animationAsset.transform.position = transform.position;
        animationAsset.transform.SetParent(gameObject.transform);

        animation = transform.GetChild(transform.childCount - 1).GetComponent<GAFMovieClip>();

        topLimit =      LevelManager.manager.GetComponent<RectTransform>().rect.height/2;
        bottomLimit =   -LevelManager.manager.GetComponent<RectTransform>().rect.height / 2 - .3f;
        rightLimit =    LevelManager.manager.GetComponent<RectTransform>().rect.width / 2;
        leftLimit =     -LevelManager.manager.GetComponent<RectTransform>().rect.width/2;


        if (ControllerManager.instance != null && Player1 == this)
            ControllerManager.instance.onAxis1.AddListener(ControlMove);
        else if(ControllerManager.instance != null)
            ControllerManager.instance.onAxis2.AddListener(ControlMove);

        setModeIdle();
        setModeIdleDance();

        if (QTUEManager.instance != null && Player1 == this)
        {
            QTUEManager.instance.scalefactorP1.AddListener(scaleCircle);
        }
        if (QTUEManager.instance != null && Player2 == this)
        {
            QTUEManager.instance.scalefactorP2.AddListener(scaleCircle);
        }

        if (LevelManager.manager != null) LevelManager.manager.onGameOver.AddListener(DestroyThisShit);
        if (UIManager.manager != null) UIManager.manager.onTimerEnd.AddListener(SetModeVoid);
        if (UIManager.manager != null) UIManager.manager.onCounterOver.AddListener(LetsPlay);

        sexAppeal = 0;
    }
	
	void Update ()
    {
        if(canPlay)
        {
            MoveBehavior();
            DanceBehavior();
        }

        if (sexAppeal > 0)sexAppeal -= passifLostAppeal;
    }

    #region State Machine
        #region idle
        private void setModeIdle()
        {
            MoveBehavior = Idle;
            animation.setSequence("idle"+name, true);
        }
        private void Idle()
        {
            if (Player1 == this && (Input.GetAxis("P1_Horizontal") != 0 || Input.GetAxis("P1_Vertical") != 0) && !isDancing ||
                Player2 == this && (Input.GetAxis("P2_Horizontal") != 0 || Input.GetAxis("P2_Vertical") != 0) && !isDancing) setModeMove();
        }
        #endregion
        #region Move
        private void setModeMove()
        {
            MoveBehavior = Move;
            animation.setSequence("move" + name, true);
        }
        private void Move()
        {
            if (Player1 == this && (Input.GetAxis("P1_Horizontal") == 0 || Input.GetAxis("P1_Vertical") == 0) && IsAnimationEnd() && !isDancing ||
                Player2 == this && (Input.GetAxis("P2_Horizontal") == 0 || Input.GetAxis("P2_Vertical") == 0) && IsAnimationEnd() && !isDancing) setModeIdle();

        }
        #endregion
        #region idleDance
        private void setModeIdleDance()
        {
            DanceBehavior = IdleDance;
            if (Player1 == this && (Input.GetAxis("P1_Horizontal") != 0 || Input.GetAxis("P1_Vertical") != 0) ||
                Player2 == this && (Input.GetAxis("P2_Horizontal") != 0 || Input.GetAxis("P2_Vertical") != 0))
                animation.setSequence("move" + name, true);
            else
                animation.setSequence("idle" + name, true);
            isDancing = false;
        }
        private void IdleDance()
        {
            
        }
        #endregion
        #region void
        private void SetModeVoid()
        {
            MoveBehavior = DoActionVoid;
        }
        private void DoActionVoid()
        {
            canPlay = false;
        }
        #endregion
        #region Dance
        private void setModeDance()
        {
            DanceBehavior = IdleDance;
            animation.setSequence("dance" + name, true);
            isDancing = true;
        }
        private void Dance()
        {

        }
        #endregion
    #endregion

    #region Methods
    private void ControlMove(float pHorizontal, float pVertical)
    {
        if(canPlay)
        {
            Vector3 newPos = transform.position + Time.fixedDeltaTime * velocity * new Vector3(pHorizontal, pVertical, 0);
            transform.position = new Vector3(Mathf.Clamp(newPos.x, leftLimit, rightLimit),
                                             Mathf.Clamp(newPos.y, bottomLimit, topLimit),
                                             newPos.z);
        }
        
    }

    private bool IsAnimationEnd()
    {
        return animation.currentFrameNumber == animation.currentSequence.endFrame;
    }

    private void scaleCircle(float appealGain)
    {
        if (appealGain > 0)
        {
            influenceAsset.SetModeGrow();
            setModeDance();
        }
        else setModeIdle();


        if (sexAppeal + appealGain < maxAppeal)
        {
            sexAppeal += appealGain;
        }
        if (sexAppeal < 0) sexAppeal = 0;


    }

    private void DestroyThisShit()
    {
        p1.SetActive(false);
        p2.SetActive(false);
        Destroy(gameObject);
    }

    private void LetsPlay()
    {
        canPlay = true;
    }
    #endregion

}
