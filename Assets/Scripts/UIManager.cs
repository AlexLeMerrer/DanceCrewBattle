using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private float timeStart = 60.0f;
    private float timeLeft;

    public Text time;

    private static UIManager m_Manager;
    public static UIManager manager { get { return m_Manager; } }
    // Use this for initialization
    void Start()
    {
        waitingInput = new List<GameObject>();
        m_Manager = this;
        timeLeft = timeStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 60 * 2)
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

        DecreaseTimer();
    }

    private void DecreaseTimer()
    {
        timeLeft -= Time.deltaTime;
        string minutes = Mathf.Floor(timeLeft / 60).ToString("0");
        string seconds = (timeLeft % 60).ToString("00");
        if (timeLeft < 0) timeLeft = 0;
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
}

 
