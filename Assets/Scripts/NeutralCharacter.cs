using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralCharacter : MonoBehaviour {

    delegate void LaunchAction();
    private LaunchAction DoAction;

    public GameObject m_target;
    public Vector3 targetPos;
    public bool isTargeted = false;
    private float m_speed = 1.0f;
    public string team;
    public bool isContamined = false;
    public string state = "neutral";
    private int contamineCounter = 2;

    void Awake()
    {
        SetModeVoid();
    }

    void Update()
    {
        DoAction();
    }

    public void SetModeVoid()
    {
        DoAction = DoActionVoid;
    }

    public void SetModeDance()
    {
        DoAction = DoActionDance;
    }

    public void SetModeSearchForSomeone()
    {
        isContamined = true;
        state = "contamined";
        LevelManager.manager.NeutralToDancing(gameObject);
        if (LevelManager.manager.getNeutralLength() == 0) SetModeVoid();
        else
        {
            if (!LevelManager.manager.SomeoneTargetable(gameObject)) SetModeDance();
            else
            {
                m_target = LevelManager.manager.SearchForSomeoneNear(gameObject);
                m_target.GetComponent<NeutralCharacter>().isTargeted = true;
                targetPos = new Vector3(m_target.transform.position.x, m_target.transform.position.y, m_target.transform.position.z);
                DoAction = DoActionSearch;
            }
        }
    }

    private void DoActionSearch()
    {
        if(m_target.GetComponent<NeutralCharacter>().state != "neutral")
        {
            SetModeSearchForSomeone();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, m_speed * Time.deltaTime);
            float deltaDist = Vector3.Distance(transform.position, targetPos);
            if (deltaDist<1.0f)
            {
                contamineCounter--;
                RecruitNeutral();
                if (contamineCounter == 0) SetModeDance();
                else SetModeSearchForSomeone();
            }
        }
    }

    public void RecruitNeutral()
    {
        m_target.GetComponent<NeutralCharacter>().setTeam(team);
        m_target.GetComponent<NeutralCharacter>().SetModeSearchForSomeone();
    }

    private void DoActionDance()
    {
        //LevelManager.manager.ContaminateAnotherGuy(gameObject);
    }

    private void DoActionVoid()
    {

    }

    private void Comportement()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isContamined)
        {
            if (coll.gameObject.transform.parent.name.Contains("1")) setTeam("1");
            else if (coll.gameObject.transform.parent.name.Contains("2")) setTeam("2");
            SetModeSearchForSomeone();
        }
    }

    public void setTeam(string pTeam)
    {
        TeamManager.AddToTeam(pTeam, gameObject);
        team = pTeam;
        Debug.Log(pTeam);
        gameObject.GetComponent<Renderer>().material.color = (pTeam =="2")?new Color(1,0.75f, 0.79f,0.5f):Color.blue;
    }
}
