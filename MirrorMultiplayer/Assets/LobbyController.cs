using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Unity.VisualScripting;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    //UI Elements
    public TMP_Text lobbyNameText;

    //Player data
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    //Other Data
    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    private List<PlayerListItem> _playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerObjectController;

    //Manager
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
    
    private void Awake()
    {
        if (instance == null) {instance = this; }
    }

    public void UpdateLobbyName()
    {
        currentLobbyID = _manager.GetComponent<SteamLobby>().currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if (!playerItemCreated) { CreateHostPlayerItem(); } //Host
        if(_playerListItems.Count < Manager.gamePlayers.Count) {CreateClientPlayerItem();}
        if(_playerListItems.Count > Manager.gamePlayers.Count) {RemovePlayerItem();}
        if(_playerListItems.Count == Manager.gamePlayers.Count) {UpdatePlayerItem();}
    }


    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerObjectController = localPlayerObject.GetComponent<PlayerObjectController>();
    }
    
    public void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
            PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.playerName = player.PlayerName;
            newPlayerItemScript.connectionID = player.connectionID;
            newPlayerItemScript.playerSteamID = player.playerSteamID;
            newPlayerItemScript.SetPlayerValues();
            
            newPlayerItem.transform.SetParent(playerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;
            
            _playerListItems.Add(newPlayerItemScript);
        }
        playerItemCreated = true;
    }
    
    public void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            if (!_playerListItems.Any(b => b.connectionID == player.connectionID))
            {
                GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.playerName = player.PlayerName;
                newPlayerItemScript.connectionID = player.connectionID;
                newPlayerItemScript.playerSteamID = player.playerSteamID;
                newPlayerItemScript.SetPlayerValues();
            
                newPlayerItem.transform.SetParent(playerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;
            
                _playerListItems.Add(newPlayerItemScript);
            }
        }
    }
    
    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            foreach (PlayerListItem playerListItemScript in _playerListItems)
            {
                if (playerListItemScript.connectionID == player.connectionID)
                {
                    playerListItemScript.playerName = player.PlayerName;
                    playerListItemScript.SetPlayerValues();
                }
            }
        }
    }
    
    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemToRemove = new List<PlayerListItem>();

        foreach (PlayerListItem playerListItem in _playerListItems)
        {
            if (!Manager.gamePlayers.Any(b => b.connectionID == playerListItem.connectionID))
            {
                playerListItemToRemove.Add(playerListItem);
            }
        }

        if (playerListItemToRemove.Count > 0)
        {
            foreach (PlayerListItem playerlistItemToRemove in playerListItemToRemove)
            {
                GameObject ObjectToRemove = playerlistItemToRemove.gameObject;
                _playerListItems.Remove(playerlistItemToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
            }
        }
    }
    
    
    
}
