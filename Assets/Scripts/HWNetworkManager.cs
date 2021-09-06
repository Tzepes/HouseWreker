using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public struct PlayerTypeMessage : NetworkMessage
{
    public PlayerType playerType;
}

public enum PlayerType
{
    Human,
    Cat
}

public class HWNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<PlayerTypeMessage>(OnCreateCharacter);
    }

    void OnCreateCharacter(NetworkConnection conn, PlayerTypeMessage message)
    {
        GameObject gameobject = Instantiate(playerPrefab);

        HWPlayer player = gameobject.GetComponent<HWPlayer>();
        //player.type = message.type;

        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("connected to a server");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Debug.Log("PlayerAdded");
    }
}
