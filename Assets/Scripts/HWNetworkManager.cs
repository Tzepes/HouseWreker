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
    Player
}

public class HWNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<PlayerTypeMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        PlayerTypeMessage playerMessage = new PlayerTypeMessage
        {
            playerType = PlayerType.Player
        };
        conn.Send(playerMessage);
    }

    void OnCreateCharacter(NetworkConnection conn, PlayerTypeMessage message)
    {
        GameObject gameobject = Instantiate(playerPrefab);

        HWPlayer player = gameobject.GetComponent<HWPlayer>();

        // UI player choice will be taken and passed in SetPlayerType();
        player.SetPlayerType("Human");

        GameObject playerTypePrefab = player.GetPlayerTypePrefab();

        GameObject playerToSpawn = Instantiate(playerTypePrefab);

        NetworkServer.AddPlayerForConnection(conn, playerToSpawn);
    }


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Debug.Log("PlayerAdded");
    }
}
