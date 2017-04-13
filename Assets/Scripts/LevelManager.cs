using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static LevelManager m_Manager;
    public static LevelManager manager { get { return m_Manager; } }

    private static int countArrived = 0;
    public static int numberOfDancer = 30;

    public GameObject prefab;
    public GameObject playerPrefab;
    
    private List<GameObject> neutralPerson = new List<GameObject>();
    private List<Vector2> neutralPos = new List<Vector2>();
    private List<GameObject> dancingPerson = new List<GameObject>();

    public Vector3 stageDimensions;

    private bool isContaminated = false;
    
    public GameObject spawnP1;
    public GameObject spawnP2;

    public UnityEvent onGameOver;

    public string winner = "";
    private string loserTeam;

    private Vector2 startPos = new Vector2(8, -8);
    
    void Awake()
    {
        m_Manager = this;
        onGameOver = new UnityEvent();
        UIManager.manager.onGameOver.AddListener(DestroyAllThisShit);
        UIManager.manager.onTimerEnd.AddListener(Winner);
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i < numberOfDancer; i++)
        {
            //Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            stageDimensions = new Vector3( LevelManager.manager.GetComponent<RectTransform>().rect.width/2, LevelManager.manager.GetComponent<RectTransform>().rect.height/2, 0);
            float randomX = Random.Range(-stageDimensions.x, stageDimensions.x);
            float randomY = Random.Range(-stageDimensions.y, stageDimensions.y);

            Vector2 pos = new Vector2(randomX, randomY);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            go.transform.position = new Vector3(pos.x, pos.y, getZSort(go.transform.position));
            go.GetComponent<NeutralCharacter>().onArrived.AddListener(NeutralArrived);

            neutralPerson.Add(go);
            neutralPos.Add(pos);
            go.transform.position = new Vector3(8, -8, getZSort(go.transform.position));
        }
        StartCoroutine("NeutralCome");
        spawnPlayer();
        //MusicLoopsManager.manager.PlayMusic(MusicType.menuMusic);

    }

    private void NeutralArrived()
    {
        countArrived++;
        if (countArrived == numberOfDancer) UIManager.manager.canStart = true;
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

    public void DancingToNeutral(GameObject dancingChar)
    {
        neutralPerson.Add(dancingChar);
        dancingPerson.Remove(dancingChar);
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
            neutralPerson[i].GetComponent<NeutralCharacter>().onArrived.RemoveListener(NeutralArrived);
            Destroy(neutralPerson[i]);
        }

        for (int i = 0; i < dancingPerson.Count; i++)
        {
            dancingPerson[i].GetComponent<NeutralCharacter>().onArrived.RemoveListener(NeutralArrived);
            Destroy(dancingPerson[i]);
        }
    }

    private void Winner()
    {
        int count1 = 0;
        int count2 = 0;
        foreach (var dancing in dancingPerson)
        {
            if (dancing.GetComponent<NeutralCharacter>().team == "1") count1++;
            else count2++;
        }
        if (count1 > count2)
        {
            winner = "Player 1 Win !";
            loserTeam = "2";
        }
        else if (count2 > count1)
        {
            winner = "Player 2 Win !";
            loserTeam = "1";
        }
        else winner = "Draw !";

        if (winner == "Draw !") StartCoroutine("DrawOut");
        else StartCoroutine("LoserOut");
        StartCoroutine("ShowWinner");
        Vector3 newScale = new Vector3(1,1,1);
        UIManager.manager.timerEnd.transform.localScale = Vector3.MoveTowards(UIManager.manager.timerEnd.transform.localScale, newScale, Time.deltaTime);
    }

    IEnumerator ShowWinner()
    {
        yield return new WaitForSeconds(2.0f);
        UIManager.manager.time.text = winner;
    }

    IEnumerator NeutralCome()
    {
        for (int i = 0; i < neutralPerson.Count; i++)
        {
            neutralPerson[i].GetComponent<NeutralCharacter>().GoTo(neutralPos[i]);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator DrawOut()
    {
        Debug.Log("yo");
        for (int i = 0; i < neutralPerson.Count; i++)
        {
            neutralPerson[i].GetComponent<NeutralCharacter>().GoTo(startPos);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < dancingPerson.Count; i++)
        {
            dancingPerson[i].GetComponent<NeutralCharacter>().GoTo(startPos);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator LoserOut()
    {
        for (int i = 0; i < dancingPerson.Count; i++)
        {
            if(dancingPerson[i].GetComponent<NeutralCharacter>().team == loserTeam) dancingPerson[i].GetComponent<NeutralCharacter>().GoTo(startPos);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < neutralPerson.Count; i++)
        {
            neutralPerson[i].GetComponent<NeutralCharacter>().GoTo(startPos);
            yield return new WaitForSeconds(0.1f);
        }
    }

}
