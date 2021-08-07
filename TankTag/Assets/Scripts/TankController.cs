using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour {

    public GameObject bullet;
    public Transform bulletOrigin;
    public float speed,rotationSpeed;
    public bool mainPlayer;
    public Slider myHealth;
    public float healthValue = 100;
    public string myId;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (mainPlayer)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    float angle = Input.GetAxis("Horizontal");
                    this.transform.eulerAngles += new Vector3(0, rotationSpeed, 0);
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    float angle = Input.GetAxis("Horizontal");
                    this.transform.eulerAngles -= new Vector3(0, rotationSpeed, 0);
                }
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    this.transform.position += this.transform.forward * speed;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    this.transform.position += this.transform.forward * speed * -1f;
                }
            }
            if (Input.GetButtonDown("Jump"))
            {
                Fire();
            }
        }
        

    }


    public void Up()
    {
        if (mainPlayer)
        {
            this.transform.position += this.transform.forward * speed;

        }
        
    }
    public void Down()
    {
        if (mainPlayer)
        {
            this.transform.position += this.transform.forward * speed * -1f;
        }
        
    }
    public void Right()
    {
        if (mainPlayer)
        {
            float angle = Input.GetAxis("Horizontal");
            this.transform.eulerAngles += new Vector3(0, rotationSpeed, 0);
        }
        
    }
    public void Left()
    {
        if (mainPlayer)
        {
            float angle = Input.GetAxis("Horizontal");
            this.transform.eulerAngles -= new Vector3(0, rotationSpeed, 0);
        }
        
    }

    public void Fire()
    {
        if (mainPlayer)
        {
            var tempBullet = (GameObject)Instantiate(bullet, bulletOrigin.position, Quaternion.identity);
            tempBullet.GetComponent<Rigidbody>().velocity = this.transform.forward * 30;
            Destroy(tempBullet, 2);
        }
        
    }

    public void DecreaseHealth()
    {
        healthValue -= 10;
        myHealth.value -= 10; 
    }


    //--------------------------------
}
