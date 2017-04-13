using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceCircle : MonoBehaviour {

    public delegate void State();
    public State behaviour;

    private bool isActive;
    
    private float circleScaleSpeed = .05f;
    private float actualSize;
    private int lifeCounter;
    private int activeLifetime;

    [SerializeField]
    //public GameObject spotlight;

    private float circleScale;
    private float circleActualGrow;
    
    void Start () {
        SetModeVoid();
    }
	
	void Update () {
        behaviour();
        lightBehavior();
    }
    #region State Machine
    #region Active
    public void SetModeActive()
    {
        behaviour = ActiveBehavior;
        isActive = true;
        actualSize = 1f;
        setScale(actualSize);
        lifeCounter = 0;
        activeLifetime = UIManager.manager.QteRate;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;

    }
    private void ActiveBehavior()
    {
        if (lifeCounter++ > activeLifetime) SetModeReduce();
    }
    #endregion
    #region Void
    public void SetModeVoid()
    {
        behaviour = VoidBehavior;
        isActive = false;
        actualSize = 0f;
        setScale(actualSize);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
    private void VoidBehavior()
    {

    }
    #endregion

    #region Grow
    public void SetModeGrow()
    {
        behaviour = GrowBehavior;
        isActive = false;

    }
    private void GrowBehavior()
    {
        actualSize += circleScaleSpeed;
        if (actualSize < 1)
        {
            setScale(actualSize);
        }
        else
            SetModeActive();
    }
    #endregion

    #region Reduce
    public void SetModeReduce()
    {
        behaviour = ReduceBehavior;
        isActive = false;
    }
    private void ReduceBehavior()
    {
        actualSize -= circleScaleSpeed;
        if (actualSize > 0)
            setScale(actualSize);
        else
            SetModeVoid();
    }
    #endregion
    #endregion

    #region Methods

    private void setScale(float pScale)
    {
        transform.localScale = new Vector3(pScale, pScale, 0);
    }

    //private void ActiveLight()
    //{
    //    spotlight.transform.position.Set(spotlight.transform.position.x, spotlight.transform.position.y, 89);
    //}
    //private void DesactiveLight()
    //{
    //    spotlight.transform.position.Set(spotlight.transform.position.x, spotlight.transform.position.y, 90);
    //}


    private void lightBehavior()
    {
        //spotlight.transform.position.Set(spotlight.transform.position.x, spotlight.transform.position.y,circleActualGrow /10);
    }
    #endregion
}
