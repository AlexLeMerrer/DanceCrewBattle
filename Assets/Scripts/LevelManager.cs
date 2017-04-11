using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private static LevelManager m_Manager;
    public static LevelManager manager { get { return m_Manager; } }

    public GameObject prefab;
    private List<GameObject> neutralPerson = new List<GameObject>();
    private List<GameObject> dancingPerson = new List<GameObject>();

    private float timer = 3.0f;
    private bool isContaminated = false;

    void Awake()
    {
        m_Manager = this;
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 10; i++)
        {
            Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            float randomX = Random.Range(-stageDimensions.x, stageDimensions.x);
            float randomY = Random.Range(-stageDimensions.y, stageDimensions.y);

            Vector2 pos = new Vector2(randomX, randomY);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);

            neutralPerson.Add(go);
            
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(!isContaminated)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                ContaminationTimer();
                isContaminated = true;
            }
        }
        

        
	}

    private void ContaminationTimer()
    {
        int randomChar = Random.Range(0, neutralPerson.Count);
        int randomTarget = Random.Range(0, neutralPerson.Count);
        neutralPerson[randomChar].GetComponent<NeutralCharacter>().SetModeSearchForSomeone();
        neutralPerson[randomChar].GetComponent<NeutralCharacter>().targetPos = neutralPerson[randomTarget].transform.position;
        neutralPerson.Remove(neutralPerson[randomChar]);
    }

    public void ContamineAnotherGuy(Vector3 targetPos)
    {
        for (int i = 0; i < neutralPerson.Count; i++)
        {
            if(neutralPerson[i].transform.position == targetPos)
            {
                int randomTarget = Random.Range(0, neutralPerson.Count);
                neutralPerson[i].GetComponent<NeutralCharacter>().SetModeSearchForSomeone();
                neutralPerson[i].GetComponent<NeutralCharacter>().targetPos = neutralPerson[randomTarget].transform.position;
                neutralPerson.Remove(neutralPerson[i]);
            }
        }
    }
}
