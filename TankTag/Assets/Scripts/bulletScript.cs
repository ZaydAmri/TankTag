using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

    public GameController gameController;
    // Use this for initialization
    void Awake()
    {
        if(GameObject.Find("GameController") != null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.gameObject.name);

        if (col.gameObject.CompareTag("opponent"))
        {
            //Debug.Log(col.gameObject.name);
            gameController.updateHealth = true;
            gameController.tankKilled = col.gameObject.GetComponent<opponentsTank>().oppId;
            //Debug.Log(col.gameObject.GetComponent<opponentsTank>().oppId);
            //Debug.Log(gameController.tankKilled + " 2");
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}
