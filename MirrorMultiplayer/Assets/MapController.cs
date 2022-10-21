using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MapController : NetworkBehaviour
{
    //Variables
    public string[] mapNames;
    public int currentMapIndex;
    public TMP_Text currentMapText;

    [SyncVar(hook = nameof(UpdateMapName))]
    public string mapNameSynced = "Game";

    private void Start()
    {
        currentMapIndex = 0;
        UpdateMapVariables();
    }

    public void NextMap()
    {
        if (currentMapIndex < mapNames.Length - 1)
        {
            currentMapIndex++;
            UpdateMapVariables();
            CmdSendMessageToPlayers(mapNames[currentMapIndex]);
        }
        
    }
    
    public void PreviousMap()
    {
        if (currentMapIndex > 0)
        {
            currentMapIndex--;
            UpdateMapVariables();
            CmdSendMessageToPlayers(mapNames[currentMapIndex]);
        }
    }
    public void UpdateMapVariables()
    {
        currentMapText.text = mapNames[currentMapIndex];
        LobbyController.instance.mapName = mapNames[currentMapIndex];
    }

    public void UpdateMapName(string oldValue, string newValue)
    {
        if (isServer)
        {
            mapNameSynced = newValue;
        }

        if (isClient && (oldValue != newValue))
        {
            currentMapText.text = newValue;
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSendMessageToPlayers(string newMessage)
    {
        UpdateMapName(mapNameSynced, newMessage);
    }
    
}
