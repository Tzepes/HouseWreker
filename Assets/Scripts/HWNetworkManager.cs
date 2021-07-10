using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HWNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("connected to a server");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Debug.Log("PlayerAdded");
    }
}
