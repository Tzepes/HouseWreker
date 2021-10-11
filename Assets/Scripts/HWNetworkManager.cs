using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine;

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

    public void StartGame()
    {
        //if (Players.Count < 2) { return; }

        isGameInProgress = true;

        ServerChangeScene("SampleScene");
    }

    void OnCreateCharacter(NetworkConnection conn)
    {
        GameObject gameobject = Instantiate(playerPrefab);

        HWPlayer player = gameobject.GetComponent<HWPlayer>();

        // UI player choice will be taken and passed in SetPlayerType();
        player.SetPlayerType("Cat");

        GameObject playerTypePrefab = player.GetPlayerTypePrefab();

        GameObject playerToSpawn = Instantiate(playerTypePrefab);

        NetworkServer.AddPlayerForConnection(conn, playerToSpawn);

        NetworkServer.Destroy(gameobject);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        HWPlayer player = conn.identity.GetComponent<HWPlayer>();

        Players.Add(player);

        player.SetPartyOwner(Players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(SceneManager.GetActiveScene().name == "SampleScene")
        {
            foreach(HWPlayer player in Players)
            {
                OnCreateCharacter(player.connectionToClient);
            }
        }
    }

    #endregion

    #region Client

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

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
