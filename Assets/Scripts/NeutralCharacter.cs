using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralCharacter : MonoBehaviour {

    delegate void LaunchAction();
    private LaunchAction DoAction;

    public GameObject m_target;
    public Vector3 targetPos;
    public bool isTargeted = false;
    private float m_speed = 2.0f;

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
        LevelManager.manager.RemoveFromTab(gameObject);
        m_target = LevelManager.manager.SearchForSomeoneNear(gameObject);
        m_target.GetComponent<NeutralCharacter>().isTargeted = true;
        targetPos = new Vector3(m_target.transform.position.x - 1.0f, m_target.transform.position.y - 1.0f, m_target.transform.position.z);
        DoAction = DoActionSearch;

    }

    private void DoActionSearch()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos, m_speed * Time.deltaTime);

        if (transform.position == targetPos)
        {
            SetModeDance();
            m_target.GetComponent<NeutralCharacter>().SetModeSearchForSomeone();
        }

    }

    private void DoActionDance()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        //LevelManager.manager.ContaminateAnotherGuy(gameObject);
    }

    private void DoActionVoid()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        SetModeSearchForSomeone();
    }


}
