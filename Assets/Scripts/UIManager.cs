using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{

    public GameObject[] Qtues;
    public GameObject[] controls;
    public static Text Text1;
    public static Text Text2;
    public float speed;
    private List<GameObject> waitingInput;
    public RectTransform[] textName;
    private int counter = 0;
    private GameObject next;

    public Text timerEnd;

    public float QteRate;
    public float timeStart;
    private float timeLeft;

    private float timerCounter = 4.0f;

    public Text time;
    public Button timerEndButton;
    public GameObject hud;
    public GameObject decor;
    public Camera mainCamera;

    public GameObject number3;
    public GameObject number2;
    public GameObject number1;
    public GameObject numberGO;
    private bool isCounterOver = false;

    private bool isGameOver = false;

    public UnityEvent onGameOver;
    public UnityEvent onCounterOver;

    private static UIManager m_Manager;
    public static UIManager manager { get { return m_Manager; } }

    void Awake()
    {
        m_Manager = this;
        onGameOver = new UnityEvent();
        onCounterOver = new UnityEvent();
    }
    // Use this for initialization
    void Start()
    {
        waitingInput = new List<GameObject>();
        timeLeft = timeStart;
        isCounterOver = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            if (counter > QteRate)
            {
                foreach (var lQtue in Qtues)
                {

                    next = GameObject.Instantiate(controls[Random.Range(0, controls.Length)]);
                    next.AddComponent<Qtue>();
                    next.transform.SetParent(lQtue.transform);
                    next.transform.position = lQtue.transform.FindChild("Start").gameObject.transform.position;
                    if (next.gameObject.transform.parent.name == "QTE 1") next.GetComponent<Qtue>().currentPlayer = "P1_";
                    else if (next.gameObject.transform.parent.name == "QTE 2") next.GetComponent<Qtue>().currentPlayer = "P2_";
                    waitingInput.Add(next);
                }

                counter = 0;
            }
            counter++;

            //DecreaseCounterTimer();
            if(isCounterOver) DecreaseTimer();
        }
        
    }

    private void DecreaseCounterTimer()
    {
        if (timerCounter == 4.0f) number3.gameObject.SetActive(true);
        else if (timerCounter == 3.0f)
        {
            number3.gameObject.SetActive(false);
            number2.gameObject.SetActive(true);
        }
        else if (timerCounter == 2.0f)
        {
            number2.gameObject.SetActive(false);
            number1.gameObject.SetActive(true);
        }
        else if (timerCounter == 1.0f)
        {
            number1.gameObject.SetActive(false);
            numberGO.gameObject.SetActive(true);
        }
        else if (timerCounter <= 0)
        {
            numberGO.gameObject.SetActive(false);
            onCounterOver.Invoke();
            isCounterOver = true;

        }
        timerCounter -= Time.deltaTime;
        string seconds = (timeLeft % 60).ToString("00");
        

    }

    private void DecreaseTimer()
    {
        timeLeft -= Time.deltaTime;
        string minutes = Mathf.Floor(timeLeft / 60).ToString("0");
        string seconds = (timeLeft % 60).ToString("00");
        if (timeLeft < 0)
        {
            timeLeft = 0;
            LevelManager.manager.EndGame();
            DestroyAllThisUIShit();
            isGameOver = true;
        }
        time.text = minutes + " : " + seconds;
    }

    public static void InputReaction(GameObject pParent, string pReaction)
    {
        Text lText = UIManager.Text1;
        if (pParent.name == "QTE 2")
        {
            lText = UIManager.Text2;
        }
        lText.gameObject.SetActive(true);
        lText.text = pReaction;
    }

    public void ActiveText(string TextName)
    {
        for (int i = 0; i < textName.Length; i++)
        {
            if (textName[i].name == TextName) textName[i].gameObject.SetActive(true);
        }
    }

    public void onGameFinish()
    {
        
        
    }

    private void DestroyAllThisUIShit()
    {
        onGameOver.Invoke();
        Destroy(timerEnd);
        Destroy(timerEndButton);
        Destroy(time);
        Destroy(hud);
        decor.gameObject.SetActive(false);
        mainCamera.GetComponent<AudioListener>().gameObject.SetActive(false);
        for (int i = 0; i < waitingInput.Count; i++)
        {
            Destroy(waitingInput[i]);
        }


    }

}

 
