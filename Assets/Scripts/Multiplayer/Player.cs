using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id {get; private set;}
    public string Username {get; private set;}

    public PlayerMovement Movement => movement;
    [SerializeField] private PlayerMovement movement;

    private void onDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username)
    {
        foreach(Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(-5.0f,0f,10.0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ( {(string.IsNullOrEmpty(username) ? "Guest" : username)} )";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username ;

        player.SendSpawned();
        list.Add(id,player);
    }

    #region Messages

    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData( Message.Create(MessageSendMode.Reliable, ServerToClientId.playerSpawned) ), toClientId);
    }
    
    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        return message;

    }

    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
        Debug.Log("Entro en spawn ");
    }
    
    [MessageHandler((ushort)ClientToServerId.input)]
    private static void Input(ushort fromClientId, Message message)
    {
        if(list.TryGetValue(fromClientId, out Player player))
            player.movement.SetInput(message.GetBools(5), message.GetVector3()); 
    }
    #endregion
}
