using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;



/// <summary>
/// 
/// </summary>
public class MenuManager : MonoBehaviour
{

    private static MenuManager _instance;

    public Button danceButton;
    public GameObject isartPanel;
    public GameObject titleCardPanel;
    public GameObject creditsPanel;
    public GameObject characterPanel;

    public Image P1;
    public Image P2;

    public Button playButton;
    public Button creditsButton;
    public Button backButton;
    public Button characterButton;
    private Button currentButton;

    public Image player1;
    public Image player2;
    private int numberplayer = 2;
    private int currentNumberPlayer1 = 0;
    private int currentNumberPlayer2 = 0;
    private bool isChanging = false;

    public Sprite[] persoImage1;
    public Sprite[] persoImage2;

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
        ControllerManager.instance.onAxis1.AddListener(ChangeSelectedButton);
    }

    protected void Update()
    {
        isartTimer -= Time.deltaTime;
        if(isartTimer <= 0 && !isActive)
        {
            isartPanel.gameObject.SetActive(false);
            titleCardPanel.gameObject.SetActive(true);
            isActive = true;
            if(!MusicLoopsManager.manager.IsPlaying()) MusicLoopsManager.manager.PlayMusic(MusicLoopsManager.manager.GetMusicIndex("TitleCard"));
        }

        CheckIfPlayerConnected();
    }

    private void ChangeSelectedButton(float axis1,float axis2)
    {
        //ControllerManager.instance.onAxis1.RemoveListener(ChangeSelectedButton);
        if (currentButton == playButton && axis1 > 0.5)
        {
            currentButton = creditsButton;
            creditsButton.Select();
        }
        else if (currentButton == creditsButton && axis1 < -0.5)
        {
            currentButton = playButton;
            playButton.Select();
        }
    }

    private void DoActionSelectedButton()
    {
        ControllerManager.instance.onAj1.RemoveListener(DoActionSelectedButton);
        if (currentButton == playButton)
        {
            ControllerManager.instance.onAj1.AddListener(LoadScene);
            onTitleCardButton();
        }
        else if (currentButton == creditsButton)
        {
            titleCardPanel.gameObject.SetActive(false);
            creditsPanel.gameObject.SetActive(true);
            ControllerManager.instance.onAj1.AddListener(BackToMenu);
        }
    }

    private void BackToMenu()
    {
        ControllerManager.instance.onAj1.RemoveListener(BackToMenu);
        ControllerManager.instance.onAj1.AddListener(DoActionSelectedButton);
        creditsPanel.gameObject.SetActive(false);
        titleCardPanel.gameObject.SetActive(true);
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
        characterButton.Select();
        ControllerManager.instance.onAxis1.AddListener(ChangePerso1);
        ControllerManager.instance.onAxis2.AddListener(ChangePerso2);
        if (Input.GetJoystickNames().Length > 0) ActiveP1();
    }

    private void ChangePerso1(float ax1, float ax2)
    {
        if(ax1 >= 0.8f && !isChanging)
        {
            isChanging = true;
            currentNumberPlayer1++;
            if (currentNumberPlayer1 > 2) currentNumberPlayer1 = 0;
            StartCoroutine(TimerPerso());
        }
        else if(ax1 <= -0.8f && !isChanging)
        {
            isChanging = true;
            currentNumberPlayer1--;
            if (currentNumberPlayer1 < 0) currentNumberPlayer1 = 2;
            StartCoroutine(TimerPerso());
        }
        GameManager.instance.player1 = currentNumberPlayer1;
        player1.GetComponent<Image>().sprite = persoImage1[currentNumberPlayer1];
    }

    IEnumerator TimerPerso()
    {
        yield return new WaitForSeconds(0.25f);
        isChanging = false;
    }

    private void ChangePerso2(float ax1, float ax2)
    {
        if(ax1 > 0.8 && !isChanging)
        {
            isChanging = true;
            currentNumberPlayer2++;
            if (currentNumberPlayer2 > 2) currentNumberPlayer2 = 0;
            StartCoroutine(TimerPerso());
            GameManager.instance.player1 = currentNumberPlayer2;

        }
        else if (ax1 < -0.8 && !isChanging)
        {
            isChanging = true;
            currentNumberPlayer2--;
            if (currentNumberPlayer1 < 0) currentNumberPlayer1 = 2;
            StartCoroutine(TimerPerso());
            GameManager.instance.player1 = currentNumberPlayer2;
        }

        player2.GetComponent<Image>().sprite = persoImage2[currentNumberPlayer2];

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
        GameManager.instance.player1 = 0;
        GameManager.instance.player2 = 1;
        DontDestroyOnLoad(GameManager.instance);
        SceneManager.LoadScene("DanceCrewBattle", LoadSceneMode.Single);
    }
}
