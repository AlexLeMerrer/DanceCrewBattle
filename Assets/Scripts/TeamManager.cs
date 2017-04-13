using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    public static List<GameObject> blue = new List<GameObject>();
    public static List<GameObject> rose = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}

    public static void AddToTeam(string pTeam,GameObject pGameObject)
    {
        if (pTeam == "2") rose.Add(pGameObject);
        else blue.Add(pGameObject);
    }

    public static int getBlueTeamCount()
    {
        return blue.Count;
    }

    public static int getRoseTeamCount()
    {
        return rose.Count;
    }

    public static void reset()
    {
        rose.Clear();
        blue.Clear();
    }


    // Update is called once per frame
    void Update () {
		
	}
}
