using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int connectionID;
    [SyncVar] public int playerIdNumber;
    [SyncVar] public ulong playerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;

    private CustomNetworkManager _manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (_manager != null)
            {
                return _manager;
            }

            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.gamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }
    
    public override void OnStopClient()
    {
        Manager.gamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }


    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        this.PlayerNameUpdate(this.PlayerName, playerName);
    }
    
    
    public void PlayerNameUpdate(string oldValue, string newValue)
    {
        if (isServer) //Host
        {
            this.PlayerName = newValue;
        }

        if (isClient) //Client
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }
    
    
    
}
