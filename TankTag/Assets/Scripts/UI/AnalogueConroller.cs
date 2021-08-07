using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogueConroller : MonoBehaviour {

    public GameObject guideAnalogue;
    private Vector3 startPosition,comparePosition;
    private Vector2 mouseEndPosition, mousePosition, mouseStartPosition;
    public bool touched = false;
    public TankController tankController;
    public float analogueMinDistance;
    // Use this for initialization
    void Start () {

        tankController = GameObject.Find("player1").GetComponent<TankController>();
        //this.transform.position = guideAnalogue.position;
        startPosition = guideAnalogue.GetComponent<Image>().rectTransform.anchoredPosition3D;
        comparePosition = startPosition;
    }
	
	// Update is called once per frame
	void Update () {


        if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && touched)
        {
            FirstClick();
            //clickCount++;
            
        }
        if ((Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))&&touched)
        {

            
                MouseDragged();
            

        }
        if ((Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            
                
                ReleaseMouse();
            


            touched = false;
        }
    }


    void FirstClick()
    {
        if (Input.touchCount > 0)
        {
            mouseStartPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);


        }
        else
        {
            mouseStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        }
    }

    void MouseDragged()
    {
        if (Input.touchCount > 0)
        {

            int touchCount = Input.touchCount;
            
            this.transform.position = Input.GetTouch(touchCount - 1).position;
            if (this.GetComponent<Image>().rectTransform.position.y > guideAnalogue.GetComponent<Image>().rectTransform.position.y + analogueMinDistance)
            {
                tankController.Up();
            }
            if (this.GetComponent<Image>().rectTransform.position.y < guideAnalogue.GetComponent<Image>().rectTransform.position.y - analogueMinDistance)
            {
                tankController.Down();
            }
            if (this.GetComponent<Image>().rectTransform.position.x > guideAnalogue.GetComponent<Image>().rectTransform.position.x + analogueMinDistance)
            {
                tankController.Right();
            }
            if (this.GetComponent<Image>().rectTransform.position.x < guideAnalogue.GetComponent<Image>().rectTransform.position.x - analogueMinDistance)
            {
                tankController.Left();
            }




        }
        else
        {
            
            this.transform.position = Input.mousePosition;
            if (this.GetComponent<Image>().rectTransform.position.y > guideAnalogue.GetComponent<Image>().rectTransform.position.y+ analogueMinDistance)
            {
                tankController.Up();
            }
            if (this.GetComponent<Image>().rectTransform.position.y < guideAnalogue.GetComponent<Image>().rectTransform.position.y - analogueMinDistance)
            {
                tankController.Down();
            }
            if (this.GetComponent<Image>().rectTransform.position.x > guideAnalogue.GetComponent<Image>().rectTransform.position.x + analogueMinDistance)
            {
                tankController.Right();
            }
            if (this.GetComponent<Image>().rectTransform.position.x < guideAnalogue.GetComponent<Image>().rectTransform.position.x - analogueMinDistance)
            {
                tankController.Left();
            }



        }

    }

    void ReleaseMouse()
    {
        this.GetComponent<Image>().rectTransform.anchoredPosition3D = startPosition;
    }

    public void touchBegin()
    {
        touched = true;
    }
    public void touchEnd()
    {
        touched = false;
    }

    public void Fire()
    {
        tankController.Fire();
    }
    //---------------------------------------
}
