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
    public GameObject hud;
    public GameObject decor;
    public Camera mainCamera;

    public Image number3;
    public Image number2;
    public Image number1;
    public Image numberGO;
    private bool isCounterOver = false;
    

    public GameObject msgP1;
    public GameObject msgP2;

    public GameObject qteTxtMiss;
    public GameObject qteTxtPerfect;
    public GameObject qteTxtGood;

    private bool isGameOver = false;

    public UnityEvent onGameOver;
    public UnityEvent onTimerEnd;
    public UnityEvent onCounterOver;

    public bool canStart = false;

    private static UIManager m_Manager;
    public static UIManager manager { get { return m_Manager; } }

    void Awake()
    {
        m_Manager = this;
        onGameOver = new UnityEvent();
        onCounterOver = new UnityEvent();
        onTimerEnd = new UnityEvent();
    }
    // Use this for initialization
    void Start()
    {
        waitingInput = new List<GameObject>();
        timeLeft = timeStart;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver && isCounterOver)
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

        }

        if(canStart)DecreaseCounterTimer();
        if (isCounterOver) DecreaseTimer();

    }

    private void DecreaseCounterTimer()
    {
        if (timerCounter == 4.0f) number3.gameObject.SetActive(true);
        else if (timerCounter <= 3.0f && timerCounter >= 2.0f)
        {
            number3.gameObject.SetActive(false);
            number2.gameObject.SetActive(true);
        }
        else if (timerCounter <= 2.0f && timerCounter >= 1.0f)
        {
            number2.gameObject.SetActive(false);
            number1.gameObject.SetActive(true);
        }
        else if (timerCounter <= 1.0f && timerCounter >= 0.0f)
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
            ActiveGameOverText();
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

    private void ActiveGameOverText()
    {
        timerEnd.gameObject.SetActive(true);
        ControllerManager.instance.onAj1.AddListener(DestroyAllThisUIShit);
        onTimerEnd.Invoke();
    }

    public void onGameFinish()
    {
        
        
    }

    private void DestroyAllThisUIShit()
    {
        onGameOver.Invoke();
        Destroy(timerEnd);
        Destroy(time);
        Destroy(hud);
        decor.gameObject.SetActive(false);
        LevelManager.manager.EndGame();
        mainCamera.GetComponent<AudioListener>().gameObject.SetActive(false);
        canStart = false;
        for (int i = 0; i < waitingInput.Count; i++)
        {
            Destroy(waitingInput[i]);
        }


    }

    public void SetQteMsg(int pMsg, bool pP1)
    {
        GameObject msg;



        if      (pMsg == 0) msg = Instantiate(qteTxtPerfect);
        else if (pMsg == 1) msg = Instantiate(qteTxtGood);
        else                msg = Instantiate(qteTxtMiss);

        if (pP1)
        {
            msg.transform.position = msgP1.transform.position;
        }
        else msg.transform.position = msgP2.transform.position;

        msg.transform.SetParent(Qtues[0].transform.parent.transform);
    }

}

 
