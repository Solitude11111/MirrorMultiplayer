using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Steamworks;
using TMPro;
using Unity.VisualScripting;

public class CharacterCosmetics : MonoBehaviour
{
    public int currentColorIndex = 0;
    public Material[] playerColors;
    public Image currentColorImage;
    public TMP_Text currentColorText;
    
    public GameObject localPlayerObject;
    public PlayerObjectController localplayerController;
    
    IEnumerator Start()
    {
        currentColorIndex = PlayerPrefs.GetInt("currentColorIndex", 0);
        currentColorImage.color = playerColors[currentColorIndex].color;
        currentColorText.text = playerColors[currentColorIndex].name;
        yield return new WaitForSeconds(0.001f);
        FindLocalPlayer();
    }

    public void NextColor()
    {
        if (currentColorIndex < playerColors.Length - 1)
        {
            currentColorIndex++;
            PlayerPrefs.SetInt("currentColorIndex", currentColorIndex);
            currentColorImage.color = playerColors[currentColorIndex].color;
            currentColorText.text = playerColors[currentColorIndex].name;
            localplayerController.CmdUpdatePlayerColor(currentColorIndex);
        }
    }
    public void PreviousColor()
    {
        if (currentColorIndex > 0)
        {
            currentColorIndex--;
            PlayerPrefs.SetInt("currentColorIndex", currentColorIndex);
            currentColorImage.color = playerColors[currentColorIndex].color;
            currentColorText.text = playerColors[currentColorIndex].name;
            localplayerController.CmdUpdatePlayerColor(currentColorIndex);
        }
    }
    
    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localplayerController = localPlayerObject.GetComponent<PlayerObjectController>();
        localplayerController.CmdUpdatePlayerColor(currentColorIndex);  //The command was added here so it runs at the start, that way the last color used is automatically applied.
        Debug.Log("FindLocalPlayer called");
    }
    
}
