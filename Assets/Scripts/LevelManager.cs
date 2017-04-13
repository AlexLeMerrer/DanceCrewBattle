using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static LevelManager m_Manager;
    public static LevelManager manager { get { return m_Manager; } }

    public GameObject prefab;
    public GameObject playerPrefab;
    
    private List<GameObject> neutralPerson = new List<GameObject>();
    private List<GameObject> dancingPerson = new List<GameObject>();

    public Vector3 stageDimensions;

    private bool isContaminated = false;
    
    public GameObject spawnP1;
    public GameObject spawnP2;

    public UnityEvent onGameOver;
    
    void Awake()
    {
        m_Manager = this;
        onGameOver = new UnityEvent();
        UIManager.manager.onGameOver.AddListener(DestroyAllThisShit);
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 30; i++)
        {
            //Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            stageDimensions = new Vector3( LevelManager.manager.GetComponent<RectTransform>().rect.width/2, LevelManager.manager.GetComponent<RectTransform>().rect.height/2, 0);
            float randomX = Random.Range(-stageDimensions.x, stageDimensions.x);
            float randomY = Random.Range(-stageDimensions.y, stageDimensions.y);

            Vector2 pos = new Vector2(randomX, randomY);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            go.transform.position = new Vector3(pos.x, pos.y, getZSort(go.transform.position));

            neutralPerson.Add(go);
            
        }
        
        spawnPlayer();
        //MusicLoopsManager.manager.PlayMusic(MusicType.menuMusic);

    }

    public float getZSort(Vector3 pPos)
    {
        float posY = -stageDimensions.y / 2 - ((-stageDimensions.y / 2) - pPos.y);
        float zSort = (posY / stageDimensions.y)*2;
        return 20 + zSort;
    }

    public GameObject SearchForSomeoneNear(GameObject neutralChar)
    {
        float distance = 100000000000.0f;
        GameObject nearestChar = neutralChar;
        foreach (var lChar in neutralPerson)
        {
            if (lChar.GetComponent<NeutralCharacter>().isTargeted && neutralChar.GetComponent<NeutralCharacter>().team == lChar.GetComponent<NeutralCharacter>().targetedBy) continue;
            float newDistance = Vector3.Distance(neutralChar.transform.position, lChar.transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                nearestChar = lChar;
            }
        }
        return nearestChar;
    }

    public bool SomeoneTargetable(GameObject neutralChar)
    {
        foreach (var lChar in neutralPerson)
        {
            if (!lChar.GetComponent<NeutralCharacter>().isTargeted) return true;
        }
        return false;
    }

    public void NeutralToDancing(GameObject neutralChar)
    {
        neutralPerson.Remove(neutralChar);
        dancingPerson.Add(neutralChar);
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
        Player1.name = "1";
        Player2.name = "2";
    }

    public void EndGame()
    {
        onGameOver.Invoke();
        SceneManager.LoadSceneAsync("TitleCard", LoadSceneMode.Additive);
    }
   
    private void DestroyAllThisShit()
    {
        for (int i = 0; i < neutralPerson.Count; i++)
        {
            Destroy(neutralPerson[i]);
        }

        for (int i = 0; i < dancingPerson.Count; i++)
        {
            Destroy(dancingPerson[i]);
        }
    }
}
