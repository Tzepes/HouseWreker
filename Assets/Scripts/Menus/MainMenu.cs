using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject landingPagePannel = null;

    public void HostLobby()
    {
        landingPagePannel.SetActive(false);

        NetworkManager.singleton.StartHost();
    }
}
 