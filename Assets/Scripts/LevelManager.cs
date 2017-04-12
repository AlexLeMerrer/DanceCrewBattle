using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private static LevelManager m_Manager;
    public static LevelManager manager { get { return m_Manager; } }

    public GameObject prefab;
    public GameObject playerPrefab;
    
    private List<GameObject> neutralPerson = new List<GameObject>();
    private List<GameObject> dancingPerson = new List<GameObject>();

    private bool isContaminated = false;
    
    public GameObject spawnP1;
    public GameObject spawnP2;
    
    void Awake()
    {
        m_Manager = this;
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 30; i++)
        {
            Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            float randomX = Random.Range(-stageDimensions.x, stageDimensions.x);
            float randomY = Random.Range(-stageDimensions.y, stageDimensions.y);

            Vector2 pos = new Vector2(randomX, randomY);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);

            neutralPerson.Add(go);
            
        }
        MusicLoopsManager.manager.PlayMusic(MusicType.solveMusic);
    }

        spawnPlayer();
        MusicLoopsManager.manager.PlayMusic(MusicType.menuMusic);

    }

    public GameObject SearchForSomeoneNear(GameObject neutralChar)
    {
        float distance = 100000000000.0f;
        GameObject nearestChar = neutralChar;
        foreach (var lChar in neutralPerson)
        {
            if (lChar.GetComponent<NeutralCharacter>().isTargeted) continue;
            float newDistance = Vector3.Distance(neutralChar.transform.position, lChar.transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                nearestChar = lChar;
            }
        }
        return nearestChar;
    }

    public bool SomeoneTargetable()
    {
        foreach (var lChar in neutralPerson)
        {
            if (!lChar.GetComponent<NeutralCharacter>().isTargeted) return true;
        }
        return false;
    }

    public void RemoveFromTab(GameObject neutralChar)
    {
        neutralPerson.Remove(neutralChar);
    }

    public int getNeutralLength()
    {
        return neutralPerson.Count;
    }
	
	// Update is called once per frame
	void Update () {
        
        
	}

    private void ContaminationTimer()
    {
        int randomChar = Random.Range(0, neutralPerson.Count);
        int randomTarget = Random.Range(0, neutralPerson.Count);
        neutralPerson[randomChar].GetComponent<NeutralCharacter>().SetModeSearchForSomeone();
        neutralPerson[randomChar].GetComponent<NeutralCharacter>().targetPos = neutralPerson[randomTarget].transform.position;
        neutralPerson.Remove(neutralPerson[randomChar]);
    }

    public void ContaminateAnotherGuy(GameObject danseur)
    {
        Vector3 nearDanseur = SearchForSomeoneNear(danseur).transform.position;
        for (int i = 0; i < neutralPerson.Count; i++)
        {
            if (neutralPerson[i].transform.position == nearDanseur) neutralPerson[i].GetComponent<NeutralCharacter>().SetModeSearchForSomeone();
        }
    }
    
    private void spawnPlayer()
    {
        GameObject Player1 = Instantiate(playerPrefab, spawnP1.transform.position, Quaternion.identity);
        GameObject Player2 = Instantiate(playerPrefab, spawnP2.transform.position, Quaternion.identity);
    }
}
