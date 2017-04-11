using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceCircle : MonoBehaviour {

    public delegate void State();
    public State behaviour;

    private bool isActive;

    public float startCircleScale;
    public float maxCircleScale;
    public float circleGrowSpeed = .25f;
    public float circleReduceSpeed = .1f;

    [SerializeField]
    public GameObject spotlight;

    private float circleScale;
    private float circleActualGrow;
    
    void Start () {
        startCircleScale = circleScale;
    }
	
	void Update () {
        if (isActive) behaviour();
        lightBehavior();
    }

    #region State Machine
    #region Grow
    public void SetModeGrow(float addScale)
    {
        behaviour = GrowBehavior;
        isActive = true;
        if (circleScale < 2)                circleScale = 2;
        circleScale += addScale;
        circleActualGrow = transform.localScale.x;
    }
    private void GrowBehavior()
    {
        if (circleActualGrow < circleScale)
        {
            circleActualGrow += circleGrowSpeed;
            setScale(circleActualGrow);
        }
        else
        {
            circleActualGrow = circleScale;
            setScale(circleActualGrow);
            SetModeReduce();
        }
    }
    #endregion

    #region Reduce
    public void SetModeReduce()
    {
        behaviour = ReduceBehavior;
        isActive = true;
    }
    private void ReduceBehavior()
    {
        if (circleActualGrow > startCircleScale)
        {
            circleScale -= circleReduceSpeed;
            circleActualGrow -= circleReduceSpeed;
            setScale(circleActualGrow);
        }
        else
            SetModeVoid();
    }
    #endregion

    #region Void
    public void SetModeVoid()
    {
        behaviour = VoidBehavior;
    }
    private void VoidBehavior()
    {

    }
    #endregion
    #endregion

    #region Methods

    private void setScale(float pScale)
    {

        transform.localScale = new Vector3(pScale,
                                           pScale,
                                           0    );
    }

    private void lightBehavior()
    {
        spotlight.GetComponent<Light>().spotAngle = circleActualGrow*10f;
        spotlight.GetComponent<Light>().intensity = circleActualGrow/5;
        spotlight.transform.position.Set(spotlight.transform.position.x, spotlight.transform.position.y,circleActualGrow / 2);
    }
    #endregion
    

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Character")
        {
            coll.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
