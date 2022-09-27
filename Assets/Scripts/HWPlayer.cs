using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HWPlayer : NetworkBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject interactableArea;
    [SerializeField]
    public GameObject mainCamera;

    [SerializeField]
    private GameObject humanPlayer;
    [SerializeField]
    private GameObject catPlayer;

    [SerializeField]
    private string playerType;

    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;

    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    public bool GetIsPartyOwner()
    {
        return isPartyOwner;
    }

    [Command]
    public void SetPlayerType(string newTypeChoice)
    {
        playerType = newTypeChoice;
    }

    public void HWSetPlayerType(string newTypeChoice)
    {
        playerType = newTypeChoice;
    }

    public String GetPlayerType()
    {
        return playerType;
    }

    [Server]
    public void SetPartyOwner(bool state)
    {
        isPartyOwner = state;
    }

    [Command]
    public void CmdStartGame()
    {
        if(!isPartyOwner) { return; }

        ((HWNetworkManager)NetworkManager.singleton).SetPlayerTypes();

        ((HWNetworkManager)NetworkManager.singleton).StartGame();
    }

    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartClient()
    {
        if(NetworkServer.active) { return; }
        
        DontDestroyOnLoad(gameObject);

        ((HWNetworkManager)NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient()
    {
        if(!isClientOnly) { return; }

        ((HWNetworkManager)NetworkManager.singleton).Players.Remove(this);
    }

    public GameObject GetPlayerTypePrefab()
    {
        if(playerType == "Cat")
        {
            return catPlayer;
        }
        if(playerType == "Human")
        {
            return humanPlayer;
        }

        return null;
    }

    public override void OnStartAuthority()
    {
        if(!interactableArea) { return; }

        interactableArea.SetActive(true);

       // player.GetComponent<PlayerCameraController>().GetPlayerTypeTransform(currentPlayerType.transform); // make a function in camera controller to take the transform 
                                                                                                     // and call it here
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) { return; }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }
}