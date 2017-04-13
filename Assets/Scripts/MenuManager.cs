using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;



/// <summary>
/// 
/// </summary>
public class MenuManager : MonoBehaviour
{

    private static MenuManager _instance;

    public Button danceButton;
    public GameObject isartPanel;
    public GameObject titleCardPanel;
    public GameObject characterPanel;

    public Image P1;
    public Image P2;

    public Button playButton;
    public Button creditsButton;
    private Button currentButton;

    private bool isPlayer1Connected = false;
    private bool isPlayer2Connected = false;
    private bool isActive = false;

    private float isartTimer = 2.0f;

    /// <summary>
    /// instance unique de la classe     
    /// </summary>
    public static MenuManager instance
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
            throw new Exception("Tentative de création d'une autre instance de MenuManager alors que c'est un singleton.");
        }
        _instance = this;
        
        isartPanel.gameObject.SetActive(true);
    }

    protected void Start()
    {
        currentButton = playButton;
        playButton.Select();
        ControllerManager.instance.onAj1.AddListener(DoActionSelectedButton);
    }

    protected void Update()
    {
        isartTimer -= Time.deltaTime;
        if(isartTimer <= 0 && !isActive)
        {
            isartPanel.gameObject.SetActive(false);
            titleCardPanel.gameObject.SetActive(true);
            isActive = true;
        }

        CheckIfPlayerConnected();
    }

    private void DoActionSelectedButton()
    {
        if (currentButton == playButton) onTitleCardButton();
    }

    private void CheckIfPlayerConnected()
    {
        danceButton.gameObject.SetActive(true);
    }

    public void onTitleCardButton()
    {
        titleCardPanel.gameObject.SetActive(false);
        characterPanel.gameObject.SetActive(true);
        danceButton.gameObject.SetActive(false);
        P1.gameObject.SetActive(false);
        P2.gameObject.SetActive(false);
        if (Input.GetJoystickNames().Length > 0) ActiveP1();
    }

    private void ActiveP1()
    {
        isPlayer1Connected = true;
        P1.gameObject.SetActive(true);
    }

    private void ActiveP2()
    {
        isPlayer2Connected = true;
        P2.gameObject.SetActive(true);
    }

    protected void OnDestroy()
    {
        _instance = null;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("DanceCrewBattle", LoadSceneMode.Single);
    }
}
