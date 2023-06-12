using System.Collections;
using System.Collections.Generic;
using Riptide;
using Riptide.Utils;
using TMPro;
using UnityEngine;
using LogType = UnityEngine.LogType;

public enum ServerToClientId : ushort
{
    playerSpawned = 1,
    playerMovement
}

public enum ClientToServerId : ushort
{
    name = 1,
    input
}

public class NetworkManager : MonoBehaviour
{
    public TextMeshProUGUI tmptext;
    private static NetworkManager _singleton;

    public static NetworkManager Singleton
    {
        get => _singleton;
        private set{
            if(_singleton == null)
                _singleton = value;
            else if(_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!...");
                Destroy(value);
            }
        }
    }

    public Server Server { get; private set;}

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        Application.targetFrameRate = 120; 
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError,false);
        Server = new Server();
        Server.Start(port,maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }

    void FixedUpdate() // update if problem
    {
        Server.Update();
    }

    void OnApplicationQuit(){
        Server.Stop();
    }

    private void PlayerLeft(object sender, ServerDisconnectedEventArgs e)
    {
        if(Player.list.TryGetValue(e.Client.Id, out Player player))
            Destroy(player.gameObject);
    }


    void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }

    void LogCallback(string logString, string stackTrace, LogType type)
    {
        tmptext.text = logString;
    }
}


