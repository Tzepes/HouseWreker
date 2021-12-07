using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject lobbyUI = null;
    [SerializeField]
    private Button startGameButton = null;

    private void Start()
    {
        HWNetworkManager.ClientOnConnected += HandleClientConnected;
        HWPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
    }

    private void OnDestroy()
    {
        HWNetworkManager.ClientOnConnected -= HandleClientConnected;
        HWPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
    }

    private void HandleClientConnected()
    {
        lobbyUI.SetActive(true);
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }

    public void CatButton()
    {
        NetworkClient.connection.identity.GetComponent<HWPlayer>().SetPlayerType("Cat");
    }

    public void HumanButton()
    {
        NetworkClient.connection.identity.GetComponent<HWPlayer>().SetPlayerType("Human");
    }

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<HWPlayer>().CmdStartGame();
    }

    public void LeaveLobby()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();

            SceneManager.LoadScene(0);
        }
    }
}
