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

    public void SetPlayerType(string typeChoice)
    {
        playerType = typeChoice;
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
}