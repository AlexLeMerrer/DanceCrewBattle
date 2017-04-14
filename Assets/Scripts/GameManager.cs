using UnityEngine;
using System;

/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public int player1;
    public int player2;

    private static GameManager _instance;

    /// <summary>
    /// instance unique de la classe     
    /// </summary>
    public static GameManager instance
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
            throw new Exception("Tentative de création d'une autre instance de GameManager alors que c'est un singleton.");
        }
        _instance = this;
    }

    protected void Start()
    {

    }

    protected void Update()
    {

    }

    public void OnDestroy()
    {
        _instance = null;
    }
}
