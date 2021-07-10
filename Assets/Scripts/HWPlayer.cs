using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HWPlayer : NetworkBehaviour
{
    [SerializeField]
    private GameObject interactableArea;

    private void OnStartAuthority()
    {
        interactableArea.SetActive(true);
    }
}