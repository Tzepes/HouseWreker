using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HWPlayer : NetworkBehaviour
{
    [SerializeField]
    private GameObject interactableArea;

    public override void OnStartAuthority()
    {
        interactableArea.SetActive(true);
    }
}