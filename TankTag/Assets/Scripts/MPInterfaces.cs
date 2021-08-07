public interface MPLobbyListener
{
    void SetLobbyStatusMessage(string message);
    void HideLobby();

    
}
public interface MPUpdateListener
{
    void UpdateReceived(string participantId, float posX, float posZ, float velX, float velZ, float rotY, float health);
    void UpdateReceivedWithHealth(string participantId, float posX, float posZ, float velX, float velZ, float rotY,float tankNumber);
}