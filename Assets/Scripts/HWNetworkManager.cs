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
    public List<String> ChosenPlayerTypes { get; } = new List<String>();

    [SerializeField]
    private int gameScore;

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
        ChosenPlayerTypes.Clear();

        isGameInProgress = false;
    }

    public void StartGame()
    {
        //if (Players.Count < 2) { return; }

        isGameInProgress = true;

        ServerChangeScene("SampleScene");
    }

    public void SetPlayerTypes()
    {
        foreach (HWPlayer player in Players)
        {
            if (player.GetPlayerType() == "None" || player.GetPlayerType() == null)
            {
                return;
            }

            ChosenPlayerTypes.Add(player.GetPlayerType());
        }
        DebugPlayerTypeList();
    }

    void DebugPlayerTypeList()
    {
        foreach(String playerType in ChosenPlayerTypes)
        {
            Debug.Log(playerType);
        }
    }

    void OnCreateCharacter(NetworkConnection conn, String playerType)
    {
        GameObject gameobject = Instantiate(playerPrefab);

        HWPlayer player = gameobject.GetComponent<HWPlayer>();

        player.HWSetPlayerType(playerType);

        GameObject playerTypePrefab = player.GetPlayerTypePrefab();
        
        GameObject playerToSpawn = Instantiate(playerTypePrefab);

        NetworkServer.ReplacePlayerForConnection(conn, playerToSpawn);

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
            int pTypeIndex = 0;
            foreach(HWPlayer player in Players)
            {
                OnCreateCharacter(player.connectionToClient, ChosenPlayerTypes[pTypeIndex]);
                pTypeIndex++;
            }
        }
    }

    [ContextMenu("Send Score")]
    public void SendScore()
    {
        NetworkServer.SendToAll(new ScoreDisplay.ScoreMessage { score = gameScore });
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
