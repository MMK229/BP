using Mirror;
using TMPro;
using UnityEngine;

public class NetworkUIManager : MonoBehaviour
{
    public TMP_InputField address;
    
    public void JoinClient()
    {
        NetworkManager.singleton.networkAddress = address.text;
        NetworkManager.singleton.StartClient();
    }

    public void Disconnect()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else if(NetworkServer.active)
            NetworkManager.singleton.StopServer();
        else if (NetworkClient.isConnected)
            NetworkManager.singleton.StopClient();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
