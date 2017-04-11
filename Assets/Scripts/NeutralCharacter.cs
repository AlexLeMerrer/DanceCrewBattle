using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralCharacter : MonoBehaviour {

    delegate void LaunchAction();
    private LaunchAction DoAction;

    public Vector3 targetPos;

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
        DoAction = DoActionSearch;
    }

    private void DoActionSearch()
    {
        LevelManager.manager.RemoveFromTab(gameObject);
        Vector3 targetBeforePos = new Vector3(LevelManager.manager.SearchForSomeoneNear(gameObject).x - 1.0f, LevelManager.manager.SearchForSomeoneNear(gameObject).y - 1.0f,LevelManager.manager.SearchForSomeoneNear(gameObject).z);

        transform.position = Vector3.MoveTowards(transform.position, targetBeforePos, 10.0f * Time.deltaTime);

        if (transform.position == targetBeforePos) DoAction = DoActionDance;

    }

    private void DoActionDance()
    {
        GetComponent<Renderer>().material.color = Color.blue;
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
