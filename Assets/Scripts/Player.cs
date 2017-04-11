using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static List<Player> list = new List<Player>();
    public float velocity = 3;
    public float gainInfluence = 1f;
    public InfluenceCircle influenceAsset;


    // TO MOVE
    private float topLimit;
    private float bottomLimit;
    private float rightLimit;
    private float leftLimit;

    // Use this for initialization
    void Start () {
        list.Add(this);

        topLimit = Camera.main.orthographicSize                         - GetComponent<Renderer>().bounds.size.y;
        bottomLimit = -Camera.main.orthographicSize                     + GetComponent<Renderer>().bounds.size.y;
        rightLimit = Camera.main.orthographicSize * Camera.main.aspect  - GetComponent<Renderer>().bounds.size.x;
        leftLimit = -Camera.main.orthographicSize * Camera.main.aspect  + GetComponent<Renderer>().bounds.size.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
        GrowInfluence();
        Debug.Log(Input.GetAxis("Horizontal"));
    }

    private void Move()
    {
        Vector3 newPos = transform.position + Time.fixedDeltaTime * velocity * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position = new Vector3(Mathf.Clamp(newPos.x, leftLimit, rightLimit),
                                         Mathf.Clamp(newPos.y, bottomLimit, topLimit),
                                         newPos.z);
    }

    private void GrowInfluence ()
    {
        if(Input.GetButtonDown("Fire1"))
            influenceAsset.SetModeGrow(gainInfluence);
    }

}
