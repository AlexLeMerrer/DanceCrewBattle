using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GAF.Core;
using UnityEngine.Events;

public class NeutralCharacter : MonoBehaviour {

    delegate void LaunchAction();
    private LaunchAction DoAction;

    public GameObject m_target;
    public Vector3 targetPos;
    public bool isTargeted = false;
    public string targetedBy = "";
    private float m_speed = 1.0f;
    public string team = "";
    public bool isContamined = false;
    public string state = "neutral";
    private int contamineCounter = 2;

    public int speedIntro = 25;

    public GameObject[] assetList;
    private GameObject animationAsset;
    private GAFMovieClip animation;
    public bool isConverting = false;
    private const int MAX_CONVERT = 200;
    private float counterConvert1 = 0;
    private float counterConvert2 = 0;
    private float chanceConvert = 0.8f;
    private bool isCollidePlayer = false;
    private string wichPlayerCollide = "";
    public UnityEvent onArrived;

    private Vector2 initPos;

    void Awake()
    {
        animationAsset = Instantiate(assetList[Random.Range(0, assetList.Length)]);
        animationAsset.transform.position = transform.position;
        animationAsset.transform.SetParent(gameObject.transform);
        animation = transform.GetChild(transform.childCount - 1).GetComponent<GAFMovieClip>();

        SetModeVoid();
    }

    void Update()
    {
        DoAction();
        if (isCollidePlayer) ConvertFromPlayer();
    }

    public void SetModeVoid()
    {
        DoAction = DoActionVoid;
        animation.setSequence("idle", true);
        animation.gotoAndPlay((uint)Random.Range(animation.currentSequence.startFrame, animation.currentSequence.endFrame));
        team = "";
        counterConvert1 = 0;
        counterConvert2 = 0;
        isConverting = false;
        isContamined = false;
        isTargeted = false;
        targetedBy = "";
        state = "neutral";
        contamineCounter = 2;
    }

    public void SetModeDance()
    {
        DoAction = DoActionDance;
        animation.setSequence("dance"+team, true);
        animation.gotoAndPlay((uint)Random.Range(animation.currentSequence.startFrame, animation.currentSequence.endFrame));
        if(contamineCounter == 0) StartCoroutine("ReturnVoid");
    }

    public void SetModeSearchForSomeone()
    {
        if (contamineCounter == 0) SetModeDance();
        else
        {
            isContamined = true;
            state = "contamined";
            LevelManager.manager.NeutralToDancing(gameObject);
            if (LevelManager.manager.getNeutralLength() == 0) SetModeDance();
            else
            {
                if (!LevelManager.manager.SomeoneTargetable(gameObject)) SetModeDance();
                else
                {
                    m_target = LevelManager.manager.SearchForSomeoneNear(gameObject);
                    m_target.GetComponent<NeutralCharacter>().isTargeted = true;
                    m_target.GetComponent<NeutralCharacter>().targetedBy = team;
                    targetPos = new Vector3(m_target.transform.position.x, m_target.transform.position.y, m_target.transform.position.z);
                    DoAction = DoActionSearch;
                }
            }
        }
    }

    private void DoActionSearch()
    {
        animation.setSequence("move" + team, true);
        if (m_target.GetComponent<NeutralCharacter>().isContamined) SetModeSearchForSomeone();
        transform.position = Vector3.MoveTowards(transform.position, targetPos, m_speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, LevelManager.manager.getZSort(transform.position));
        float deltaDist = Vector3.Distance(transform.position, targetPos);
        if (deltaDist<1.0f)
        {
            contamineCounter--;
            StartConversion();
        }
    }

    private void StartConversion()
    {
        SetModeDance();
        isConverting = true;
        m_target.GetComponent<NeutralCharacter>().isConverting = true;
    }

    public void RecruitNeutral()
    {
        m_target.GetComponent<NeutralCharacter>().setTeam(team);
        m_target.GetComponent<NeutralCharacter>().SetModeSearchForSomeone();
    }

    private void DoActionDance()
    {
        //LevelManager.manager.ContaminateAnotherGuy(gameObject);
        if (isConverting)
        {
            if (m_target != null)
            {
                if (!m_target.GetComponent<NeutralCharacter>().isConverting)
                {
                    isConverting = false;
                    SetModeSearchForSomeone();
                }
            }
            else return;

            if (m_target.GetComponent<NeutralCharacter>().counterConvert1 > MAX_CONVERT || m_target.GetComponent<NeutralCharacter>().counterConvert2 > MAX_CONVERT)
            {
                isConverting = false;
                m_target.GetComponent<NeutralCharacter>().isConverting = false;
                m_target.GetComponent<NeutralCharacter>().counterConvert1 = 0;
                m_target.GetComponent<NeutralCharacter>().counterConvert2 = 0;
                if (isContamined)
                {
                    if (Random.Range(0.0f, 1.0f) <= chanceConvert)
                    {
                        if (m_target != null) RecruitNeutral();
                    }
                    SetModeSearchForSomeone();
                }
            }

            if (team == "1") m_target.GetComponent<NeutralCharacter>().counterConvert1 += 0.5f;
            else m_target.GetComponent<NeutralCharacter>().counterConvert2 += 0.5f;
        }
    }
    

    private void DoActionVoid()
    {

    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        
    }

    private void ConvertFromPlayer()
    {
        if (!isContamined)
        {
            if (wichPlayerCollide == "1")
            {
                Debug.Log(counterConvert1);
                counterConvert1 += 5.0f;
                if (counterConvert1 >= MAX_CONVERT)
                {
                    setTeam("1");
                    SetModeSearchForSomeone();
                }
            }
            else if (wichPlayerCollide == "2")
            {
                counterConvert2 += 5.0f;
                if (counterConvert2 >= MAX_CONVERT)
                {
                    setTeam("2");
                    SetModeSearchForSomeone();
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.transform.parent.name.Contains("1") || coll.gameObject.transform.parent.name.Contains("2"))
        {
            isCollidePlayer = true;
            if (coll.gameObject.transform.parent.name.Contains("1")) wichPlayerCollide = "1";
            else wichPlayerCollide = "2";
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        Debug.Log(coll);
        if (coll == null) return;
        if (coll.gameObject.transform.parent.name.Contains("1") || coll.gameObject.transform.parent.name.Contains("2"))
        {
            isCollidePlayer = false;
            wichPlayerCollide = "";
        }
    }

    public void setTeam(string pTeam)
    {
        TeamManager.AddToTeam(pTeam, gameObject);
        team = pTeam;
        //gameObject.GetComponent<Renderer>().material.color = (pTeam =="2")?new Color(1,0.75f, 0.79f,0.5f):Color.blue;
    }

    public void SetModeEnter()
    {
        animation.setSequence("move", true);
        DoAction = DoActionEnter;
    }

    private void DoActionEnter()
    {
        transform.position = Vector3.MoveTowards(transform.position, initPos, Time.deltaTime* speedIntro);
        transform.position = new Vector3(transform.position.x, transform.position.y, LevelManager.manager.getZSort(transform.position));
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        if (Vector2.Distance(pos,initPos)<0.1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y);
            transform.position = new Vector3(transform.position.x, transform.position.y, LevelManager.manager.getZSort(transform.position));
            onArrived.Invoke();
            SetModeVoid();
        }
    }

    public void GoTo(Vector2 pPos)
    {
        initPos = pPos;
        SetModeEnter();
    }

    IEnumerator ReturnVoid()
    {
        yield return new WaitForSeconds(8.0f);
        LevelManager.manager.DancingToNeutral(gameObject);
        SetModeVoid();
    }
}
