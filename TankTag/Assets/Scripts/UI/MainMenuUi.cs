using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuUi : MonoBehaviour, MPLobbyListener
{

    public GameObject connectionButton,multiplayerPanel;
    public Text multiplayerText;
    
    // Use this for initialization
    void Start () {
        MultiplayerController.Instance.TrySilentSignIn();
    }
	
	// Update is called once per frame
	void Update () {
        if (MultiplayerController.Instance.IsAuthenticated())
        {
            connectionButton.SetActive(false);
        }else
        {
            connectionButton.SetActive(true);
        
        }
	}
    public void Connection()
    {
        MultiplayerController.Instance.SignInAndStartMPGame();
    }
    public void Disonnection()
    {
        MultiplayerController.Instance.SignOut();
    }
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("test1");
    }
    public void MultiplayerPlay()
    {
        multiplayerPanel.SetActive(true);
        RetainedUserPicksScript.Instance.multiplayerGame = true;
        multiplayerText.text = "Searching For Player...";
        //_showLobbyDialog = true;
        MultiplayerController.Instance.lobbyListener = this;
        MultiplayerController.Instance.SignInAndStartMPGame();
    }
   
    public void SetLobbyStatusMessage(string message)
    {
        multiplayerText.text = message;
    }
    public void HideLobby()
    {
        multiplayerText.text = "";
        multiplayerPanel.SetActive(false);
    }







    //----------------------------------
}
