using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GAF.Core;

public class Player : MonoBehaviour {

    public delegate void State();
    public State MoveBehavior;
    public State DanceBehavior;

    private static Player Player1;
    private static Player Player2;

    //Liste des joueurs
    //public static List<Player> list = new List<Player>();
    public float velocity = 3;
    public float gainInfluence = 1f;
    public InfluenceCircle influenceAsset;
    private GameObject animationAsset;
    private GAFMovieClip animation;
    private bool isDancing;

    public GameObject[] assetList;

    // TO MOVE
    private float topLimit;
    private float bottomLimit;
    private float rightLimit;
    private float leftLimit;

    private float toleranceMove = .1f;

    void Awake()
    {
    }

    void Start () {
        GameObject animationAsset;
        if (Player2 != null)
        {
            animationAsset = Instantiate(assetList[0]);
            animationAsset.transform.position = transform.position;
            Player1 = this;
            name = "1";
        }
        else
        {
            animationAsset = Instantiate(assetList[1]);
            animationAsset.transform.position = transform.position;
            Player2 = this;
            name = "2";
        }
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
        if (QTUEManager.instance != null)
        {
            setModeDance();
            QTUEManager.instance.scalefactorP1.AddListener(scaleCircle);
        }
    }
	
	void Update ()
    {
        MoveBehavior();
        DanceBehavior();
    }

    #region State Machine
        #region idle
        private void setModeIdle()
        {
            MoveBehavior = Idle;
            animation.setSequence("idle", true);
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
            animation.setSequence("move", true);
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
                animation.setSequence("move", true);
            else
                animation.setSequence("idle", true);
            isDancing = false;
        }
        private void IdleDance()
        {
            
        }
        #endregion
        #region Dance
        private void setModeDance()
        {
            DanceBehavior = IdleDance;
            animation.setSequence("dance", true);
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
        Vector3 newPos = transform.position + Time.fixedDeltaTime * velocity * new Vector3(pHorizontal, pVertical, 0);
        transform.position = new Vector3(Mathf.Clamp(newPos.x, leftLimit, rightLimit),
                                         Mathf.Clamp(newPos.y, bottomLimit, topLimit),
                                         newPos.z);
    }

    private bool IsAnimationEnd()
    {
        return animation.currentFrameNumber == animation.currentSequence.endFrame;
    }

    private void scaleCircle(float scaleFactor)
    {
        //if (Player1 == this) Debug.Log("Factor P_1" + scaleFactor);
        //if (Player2 == this) Debug.Log("Factor P_2" + scaleFactor);
        influenceAsset.SetModeGrow(scaleFactor);
    }
    
    #endregion

}
