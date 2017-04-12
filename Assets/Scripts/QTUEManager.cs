using UnityEngine;
using UnityEngine.Events;
using System;


public class ScaleEventP1 : UnityEvent<float>
{

}

public class ScaleEventP2 : UnityEvent<float>
{

}
/// <summary>
/// 
/// </summary>
public class QTUEManager : MonoBehaviour
{

    private static QTUEManager _instance;
    public ScaleEventP1 scalefactorP1;
    public ScaleEventP2 scalefactorP2;
    /// <summary>
    /// instance unique de la classe     
    /// </summary>
    public static QTUEManager instance
    {
        get
        {
            return _instance;
        }
    }

    protected void Awake()
    {
        if (_instance != null)
        {
            throw new Exception("Tentative de création d'une autre instance de QTUEManager alors que c'est un singleton.");
        }
        _instance = this;
        scalefactorP1 = new ScaleEventP1();
        scalefactorP2 = new ScaleEventP2();
    }

    protected void Start()
    {

    }

    protected void Update()
    {

    }

    protected void OnDestroy()
    {
        _instance = null;
    }

    public void InvokeScale(float scaleNumber,GameObject qteparent)
    {
        if(qteparent.name == "QTE 1") scalefactorP1.Invoke(scaleNumber);
        else if(qteparent.name == "QTE 2") scalefactorP2.Invoke(scaleNumber);

    }
}
