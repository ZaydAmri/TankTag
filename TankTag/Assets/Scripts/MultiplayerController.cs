using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class MultiplayerController : RealTimeMultiplayerListener
{
    public MPUpdateListener updateListener;
    public MPLobbyListener lobbyListener;
    private uint minimumOpponents = 1;
    private uint maximumOpponents = 1;
    private uint gameVariation = 0;

    private static MultiplayerController _instance = null;

    private byte _protocolVersion = 1;
    // Byte + Byte + 2 floats for position + 2 floats for velcocity + 1 float for rotZ
    private int _updateMessageLength = 26;
    private List<byte> _updateMessage;

    private MultiplayerController()
    {
        _updateMessage = new List<byte>(_updateMessageLength);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public static MultiplayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MultiplayerController();
            }
            return _instance;
        }
    }

    public void SignInAndStartMPGame()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.localUser.Authenticate((bool success) => {
                if (success)
                {
                    Debug.Log("We're signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
                    StartMatchMaking();
                }
                else
                {
                    Debug.Log("Oh... we're not signed in.");
                }
            });
        }
        else
        {
            Debug.Log("You're already signed in.");
            // We could also start our game now
        }
    }

    public void TrySilentSignIn()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate((bool success) => {
                if (success)
                {
                    Debug.Log("Silently signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
                }
                else
                {
                    Debug.Log("Oh... we're not signed in.");
                }
            }, true);
        }
        else
        {
            Debug.Log("We're already signed in");
        }
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public bool IsAuthenticated()
    {
        return PlayGamesPlatform.Instance.localUser.authenticated;
    }

    private void StartMatchMaking()
    {
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(minimumOpponents, maximumOpponents, gameVariation, this);
    }
    private void ShowMPStatus(string message)
    {
        Debug.Log(message);
        if (lobbyListener != null)
        {
            lobbyListener.SetLobbyStatusMessage(message);
        }
    }

    public List<Participant> GetAllPlayers()
    {
        return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
    }
    public string GetMyParticipantId()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
    }

    public void SendMyUpdate(float posX, float posZ, Vector3 velocity, float rotY,float health)
    {
        float x = velocity.x;
        float z = velocity.z;
        _updateMessage.Clear();
        _updateMessage.Add(_protocolVersion);
        _updateMessage.Add((byte)'U');
        _updateMessage.AddRange(System.BitConverter.GetBytes(posX));
        _updateMessage.AddRange(System.BitConverter.GetBytes(posZ));
        _updateMessage.AddRange(System.BitConverter.GetBytes(x));
        
        _updateMessage.AddRange(System.BitConverter.GetBytes(z));
        _updateMessage.AddRange(System.BitConverter.GetBytes(rotY));
        _updateMessage.AddRange(System.BitConverter.GetBytes(health));
        byte[] messageToSend = _updateMessage.ToArray();
        //Debug.Log("Sending my update message  " + messageToSend + " to all players in the room");
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, messageToSend);
    }

    public void SendMyUpdateWithHealth(float posX, float posZ, Vector3 velocity, float rotY, float tankNumber)
    {
        float x = velocity.x;
        float z = velocity.z;
        _updateMessage.Clear();
        _updateMessage.Add(_protocolVersion);
        _updateMessage.Add((byte)'A');
        _updateMessage.AddRange(System.BitConverter.GetBytes(posX));
        _updateMessage.AddRange(System.BitConverter.GetBytes(posZ));
        _updateMessage.AddRange(System.BitConverter.GetBytes(x));

        _updateMessage.AddRange(System.BitConverter.GetBytes(z));
        _updateMessage.AddRange(System.BitConverter.GetBytes(rotY));
        _updateMessage.AddRange(System.BitConverter.GetBytes(tankNumber));
        byte[] messageToSend = _updateMessage.ToArray();
        //Debug.Log("Sending my update message  " + messageToSend + " to all players in the room");
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, messageToSend);
    }
    //--------------------------------------------------------------------------------------------------
    public void OnRoomSetupProgress(float percent)
    {
        
        ShowMPStatus("Connecting Players...");
    }

    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            lobbyListener.HideLobby();
            lobbyListener = null;
            UnityEngine.SceneManagement.SceneManager.LoadScene("test2");
            //Application.LoadLevel("MainGame");
        }
        else
        {
            ShowMPStatus("Connection Failed. Please Try Again.");
        }
    }

    public void OnLeftRoom()
    {
        ShowMPStatus("We have left the room. We should probably perform some clean-up tasks.");
    }

    public void OnParticipantLeft(Participant participant)
    {
        throw new NotImplementedException();
    }

    public void OnPeersConnected(string[] participantIds)
    {
        foreach (string participantID in participantIds)
        {
            ShowMPStatus("Player " + participantID + " has joined.");
        }
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        foreach (string participantID in participantIds)
        {
            ShowMPStatus("Player " + participantID + " has left.");
        }
    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        // We'll be doing more with this later...
        byte messageVersion = (byte)data[0];
        // Let's figure out what type of message this is.
        char messageType = (char)data[1];
        if (messageType == 'U' && data.Length == _updateMessageLength )
        {
            float posX = System.BitConverter.ToSingle(data, 2);
            float posZ = System.BitConverter.ToSingle(data, 6);
            float velX = System.BitConverter.ToSingle(data, 10);
            
            float velZ = System.BitConverter.ToSingle(data, 14);
            float rotY = System.BitConverter.ToSingle(data, 18);
            float health = System.BitConverter.ToSingle(data, 22);
            //Debug.Log("Player " + senderId + " is at (" + posX + ", " + posY + ") traveling (" + velX + ", " + velY + ") rotation " + rotZ);
            if (updateListener != null)
            {
                updateListener.UpdateReceived(senderId, posX, posZ, velX,velZ, rotY, health);
            }
        }else if (messageType == 'A' && data.Length == 26)
        {
            float posX = System.BitConverter.ToSingle(data, 2);
            float posZ = System.BitConverter.ToSingle(data, 6);
            float velX = System.BitConverter.ToSingle(data, 10);

            float velZ = System.BitConverter.ToSingle(data, 14);
            float rotY = System.BitConverter.ToSingle(data, 18);
            float tankNumber = System.BitConverter.ToSingle(data, 22);
            //Debug.Log("Player " + senderId + " is at (" + posX + ", " + posY + ") traveling (" + velX + ", " + velY + ") rotation " + rotZ);
            if (updateListener != null)
            {
                updateListener.UpdateReceivedWithHealth(senderId, posX, posZ, velX, velZ, rotY, tankNumber);
            }
        }
    }





    //---------------------------------
}
