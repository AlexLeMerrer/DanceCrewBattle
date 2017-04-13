using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class JoystickEvent : UnityEvent<float, float> { }
public class ControllerManager : MonoBehaviour {

    public UnityEvent onAj1;
    public UnityEvent onBj1;
    public UnityEvent onCj1;
    public UnityEvent onDj1;
    public JoystickEvent onAxis1;

    public UnityEvent onAj2;
    public UnityEvent onBj2;
    public UnityEvent onCj2;
    public UnityEvent onDj2;
    public JoystickEvent onAxis2;
    
    #region Lifecycle
    private static ControllerManager _instance;
    public static ControllerManager instance
    {
        get
        {
            return _instance;
        }
    }

    // Use this for initialization
    void Start () {

    }
    void Awake()
    {
        if (_instance != null)
            throw new Exception("Tentative de création d'une autre instance de MonoBehaviorSingleton1 alors que c'est un singleton.");
        _instance = this;
        
        onAj1 = new UnityEvent();
        onBj1 = new UnityEvent();
        onCj1 = new UnityEvent();
        onDj1 = new UnityEvent(); 
        onAxis1 = new JoystickEvent();

        onAj2 = new UnityEvent();
        onBj2 = new UnityEvent();
        onCj2 = new UnityEvent();
        onDj2 = new UnityEvent();
        onAxis2 = new JoystickEvent();
    }

    public void Dispose()
    {
        _instance = null;
    }

    protected void OnDestroy()
    {
        _instance = null;
    }
    
    void Update () {
        //testDebugController();
        checkButton();
    }
    #endregion


    #region Methods

    private void checkButton()
    {
        if (Input.GetButtonDown("P1_A")) onAj1.Invoke();
        if (Input.GetButtonDown("P2_A")) onAj2.Invoke();
        if (Input.GetButtonDown("P1_B")) onBj1.Invoke();
        if (Input.GetButtonDown("P2_B")) onBj2.Invoke();
        if (Input.GetButtonDown("P1_X")) onCj1.Invoke();
        if (Input.GetButtonDown("P2_X")) onCj2.Invoke();
        if (Input.GetButtonDown("P1_Y")) onDj1.Invoke();
        if (Input.GetButtonDown("P2_Y")) onDj2.Invoke();
        if (Input.GetAxis("P1_Horizontal") != 0 || Input.GetAxis("P1_Vertical") != 0) onAxis1.Invoke(Input.GetAxis("P1_Horizontal"), Input.GetAxis("P1_Vertical"));
        if (Input.GetAxis("P2_Horizontal") != 0 || Input.GetAxis("P2_Vertical") != 0) onAxis2.Invoke(Input.GetAxis("P2_Horizontal"), Input.GetAxis("P2_Vertical"));

    }
    private void testDebugController()
    {
        if (Input.GetButtonDown("P1_A")) Debug.Log("P1_A");
        if (Input.GetButtonDown("P2_A")) Debug.Log("P2_A");
        if (Input.GetButtonDown("P1_B")) Debug.Log("P1_B");
        if (Input.GetButtonDown("P2_B")) Debug.Log("P2_B");
        if (Input.GetButtonDown("P1_X")) Debug.Log("P1_X");
        if (Input.GetButtonDown("P2_X")) Debug.Log("P2_X");
        if (Input.GetButtonDown("P1_Y")) Debug.Log("P1_Y");
        if (Input.GetButtonDown("P2_Y")) Debug.Log("P2_Y");
        if (Input.GetAxis("P1_Horizontal") != 0) Debug.Log("P1_Horizontal : " + Input.GetAxis("P1_Horizontal"));
        if (Input.GetAxis("P2_Horizontal") != 0) Debug.Log("P2_Horizontal : " + Input.GetAxis("P2_Horizontal"));
        if (Input.GetAxis("P1_Vertical") != 0) Debug.Log("P1_Vertical : " + Input.GetAxis("P1_Vertical"));
        if (Input.GetAxis("P2_Vertical") != 0) Debug.Log("P2_Vertical : " + Input.GetAxis("P2_Vertical"));
    }
    #endregion
}
