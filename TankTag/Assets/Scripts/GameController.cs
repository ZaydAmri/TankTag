using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames.BasicApi.Multiplayer;

public class GameController : MonoBehaviour, MPUpdateListener
{

    public GameObject opponentPrefab;
    public Transform[] positions;
    public GameObject myTank, opponentTank;
    private bool _multiplayerReady;
    public string _myParticipantId;
    public List<Participant> allPlayers = new List<Participant>();

    //private Vector2 _startingPoint = new Vector2(0.09675431f, -1.752321f);
    //private float _startingPointYOffset = 0.2f;
    private Dictionary<string, opponentsTank> _opponentScripts;
    private bool _multiplayerGame;
    private float _nextBroadcastTime = 0;
    //------------------------------------
    public Text myText, oppText;
    public Slider oppHealth,myHealth;
    public bool bulletShot;

    //--------à initialiser apres l'envoit du message
    public string tankKilled;
    public bool updateHealth;
    // Use this for initialization
    void Awake()
    {
        RetainedUserPicksScript userPicksScript = RetainedUserPicksScript.Instance;
        _multiplayerGame = userPicksScript.multiplayerGame;
        
        SetupMultiplayerGame();
        
    }
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {

        DoMultiplayerUpdate();
    }

    void SetupMultiplayerGame()
    {
        MultiplayerController.Instance.updateListener = this;
        // 1
        _myParticipantId = MultiplayerController.Instance.GetMyParticipantId();
        // 2
        allPlayers = MultiplayerController.Instance.GetAllPlayers();
        _opponentScripts = new Dictionary<string, opponentsTank>(allPlayers.Count - 1);
        for (int i = 0; i < allPlayers.Count; i++)
        {
            string nextParticipantId = allPlayers[i].ParticipantId;
            //Debug.Log("Setting up car for " + nextParticipantId);
            // 3
            


                Vector3 tankStartPoint = positions[i].position;

                if (nextParticipantId == _myParticipantId)
                {
                // 4
                myText.text = "Me : " + allPlayers[i].DisplayName;
                myTank.GetComponent<TankController>().mainPlayer = true;
                myTank.GetComponent<TankController>().myId = _myParticipantId;
                myTank.transform.position = tankStartPoint;
                }
                else
                {
                    // 5
                    opponentTank = (Instantiate(opponentPrefab, tankStartPoint, Quaternion.identity) as GameObject);
                    oppText.text = "Opp : "+ allPlayers[i].DisplayName;
                    opponentsTank opponentScript = opponentTank.GetComponent<opponentsTank>();
                    opponentScript.oppHealth = oppHealth;
                    opponentScript.oppId = nextParticipantId;
                    //opponentScript.SetCarNumber(i + 1);
                    // 6
                    _opponentScripts[nextParticipantId] = opponentScript;
                }
            
        }
        
        _multiplayerReady = true;
    }

    void DoMultiplayerUpdate()
    {
        if (Time.time > _nextBroadcastTime)
        {
            // We will be doing more here
            
            
            if (updateHealth)
            {
                //--------------------------get number of opponent in allplayer[] and send this number
                float tankNumber = 9;
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    if(allPlayers[i].ParticipantId == tankKilled)
                    {
                        tankNumber = i * 1.0f;
                        
                    }
                }

                //_opponentScripts[tankKilled].DecreaseHealth();
                //-------------------------------------------------------------------------------------
                MultiplayerController.Instance.SendMyUpdateWithHealth(myTank.GetComponent<Transform>().position.x,
                                                                           myTank.GetComponent<Transform>().position.z,
                                                                           myTank.GetComponent<Rigidbody>().velocity,
                                                                           myTank.GetComponent<Transform>().rotation.eulerAngles.y,
                                                                           tankNumber);

                updateHealth = false;
                tankKilled = "";
            }
            else
            {
                MultiplayerController.Instance.SendMyUpdate(myTank.GetComponent<Transform>().position.x,
                                                                        myTank.GetComponent<Transform>().position.z,
                                                                        myTank.GetComponent<Rigidbody>().velocity,
                                                                        myTank.GetComponent<Transform>().rotation.eulerAngles.y,
                                                                        myTank.GetComponent<TankController>().healthValue);
            }
            _nextBroadcastTime = Time.time + .10f;
        }

        
    }

    public void UpdateReceived(string senderId, float posX, float posZ, float velX, float velZ, float rotY, float health)
    {
        if (_multiplayerReady)
        {
            opponentsTank opponent = _opponentScripts[senderId];
            if (opponent != null)
            {
                _opponentScripts[senderId].SetTankInformation(posX, posZ, velX, velZ, rotY);
                if(_opponentScripts[senderId].healthValue > health)
                {
                    _opponentScripts[senderId].DecreaseHealth(health);
                }
            }
        }
    }
    public void UpdateReceivedWithHealth(string senderId, float posX, float posZ, float velX, float velZ, float rotY,float tankNumber)
    {
        if (_multiplayerReady)
        {
            opponentsTank opponent = _opponentScripts[senderId];
            if (opponent != null)
            {
                opponent.SetTankInformation(posX, posZ, velX, velZ, rotY);

            }
            //int _tanknumber =  Convert.ToInt32(Math.Ceiling(FloatValue));
            int _tanknumber = Mathf.RoundToInt(tankNumber);
            string _tankKilled = allPlayers[_tanknumber].ParticipantId;
            
            if (_tankKilled == _myParticipantId)
            {
                    myTank.GetComponent<TankController>().DecreaseHealth();
            }
            
        }
    }




    //------------------------------------
}
