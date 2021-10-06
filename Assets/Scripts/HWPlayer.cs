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
    private GameObject humanPlayer;
    [SerializeField]
    private GameObject catPlayer;

    private string playerType = "Cat";
    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;

    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    public bool GetIsPartyOwner()
    {
        return isPartyOwner;
    }

    public void SetPlayerType(string typeChoice)
    {
        playerType = typeChoice;
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

        ((HWNetworkManager)NetworkManager.singleton).StartGame();
    }

    public override void OnStartClient()
    {
        if(NetworkServer.active) { return; }

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