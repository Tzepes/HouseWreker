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
    private GameObject currentPlayerType;

    public enum Type
    {
        Cat,
        Human
    }

    public override void OnStartAuthority()
    {
        interactableArea.SetActive(true);
        if ((int)Type.Cat == 1)
        {
            catPlayer.SetActive(true);
        }

        if ((int)Type.Human == 0)
        {
            humanPlayer.SetActive(true);
        }

        player.GetComponent<PlayerCameraController>().GetPlayerTypeTransform(currentPlayerType.transform); // make a function in camera controller to take the transform 
                                                                                                     // and call it here
        

    }
}