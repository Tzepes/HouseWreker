using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool isGameInProgress = false;

    public List<HWPlayer> Players { get; } = new List<HWPlayer>();

    #region Server

    public override void OnServerConnect(NetworkConnection conn)
    {
        if(!isGameInProgress) { return; }

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        HWPlayer player = conn.identity.GetComponent<HWPlayer>();

        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear();

        isGameInProgress = false;
    }
    
    // Start Game button will not appear if the auto create check box is False, and if it's on true, the chosen player type wont be spawned.

    public void StartGame()
    {
        //if (Players.Count < 2) { return; }

        isGameInProgress = true;

        ServerChangeScene("SampleScene");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
    }

    // In order to have the player be spawned based on chosen type after the Start Game button was pressed from the lobby,
    // must call "NetworkServer.RegisterHandler<PlayerTypeMessage>(OnCreateCharacter);"
    // in another function: public override void OnServerSceneChanged(string sceneName) ----> Look on https://gitlab.com/GameDevTV/UnityMultiplayer/RealTimeStrategy/-/commit/1842e67e8bcc2f43bae7d884de41ca53ec81fe39

    void OnCreateCharacter(NetworkConnection conn, PlayerTypeMessage message)
    {
        Debug.Log("OnCreateCharacter called");

        GameObject gameobject = Instantiate(playerPrefab);

        HWPlayer player = gameobject.GetComponent<HWPlayer>();

        // UI player choice will be taken and passed in SetPlayerType();
        player.SetPlayerType("Cat");

        GameObject playerTypePrefab = player.GetPlayerTypePrefab();

        GameObject playerToSpawn = Instantiate(playerTypePrefab);

        NetworkServer.AddPlayerForConnection(conn, playerToSpawn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        HWPlayer player = conn.identity.GetComponent<HWPlayer>();

        Players.Add(player);

        player.SetPartyOwner(Players.Count == 1);

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            NetworkServer.RegisterHandler<PlayerTypeMessage>(OnCreateCharacter);
        }
    }

    #endregion

    #region Client

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        PlayerTypeMessage playerMessage = new PlayerTypeMessage
        {
            playerType = PlayerType.Player
        };
        conn.Send(playerMessage);

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }

    #endregion
}
