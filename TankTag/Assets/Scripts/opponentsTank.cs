using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class opponentsTank : MonoBehaviour {
    //trick1
    private Vector3 _startPos;
    private Vector3 _destinationPos;
    private Quaternion _startRot;
    private Quaternion _destinationRot;
    private float _lastUpdateTime;
    private float _timePerUpdate = 0.16f;

    //trick2
    private Vector3 _lastKnownVel;
    public Slider oppHealth;
    public float healthValue = 100;
    public string oppId;
    // Use this for initialization
    void Start () {
        _startPos = this.transform.position;
        _startRot = this.transform.rotation;
        
    }
	
	// Update is called once per frame
	void Update () {

        // 1
    float pctDone = (Time.time - _lastUpdateTime) / _timePerUpdate;

        if (pctDone <= 1.0)
        {
            // 2
            this.transform.position = Vector3.Lerp(_startPos, _destinationPos, pctDone);
            this.transform.rotation = Quaternion.Slerp(_startRot, _destinationRot, pctDone);
        }
        else
        {
            // Guess where we might be
            this.transform.position = transform.position + (_lastKnownVel * Time.deltaTime);
        }
    }

    public void SetTankInformation(float posX, float posZ, float velX, float velZ, float rotY)
    {
        // 1
        _startPos = this.transform.position;
        _startRot = this.transform.rotation;
        // 2
        _destinationPos = new Vector3(posX, this.transform.position.y, posZ);
        _destinationRot = Quaternion.Euler(0, rotY, 0);
        //3
        _lastKnownVel = new Vector3(velX, 0, velZ);
        _lastUpdateTime = Time.time;




        //this.gameObject.GetComponent<Transform>().position = new Vector3(posX, this.gameObject.GetComponent<Transform>().position.y, posZ);
        //this.gameObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, rotY, 0);
        // We're going to do nothing with velocity.... for now
    }
    public void DecreaseHealth(float health)
    {
        healthValue = health;
        oppHealth.value = health;
    }
}
