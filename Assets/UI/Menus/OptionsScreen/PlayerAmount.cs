using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAmount : MonoBehaviour
{
    int players;    //Tracks how many players will be playing

    GameManager manager; //The Game Manager singleton
    public GameObject amountTxtGameObject;  //Game Object of the display showing how many players are playing
    TextMeshProUGUI amountTxt;              //Text display for the amount of players

    private void Awake()
    {
        players = 1;    //Default to 1-Player
        manager = GameManager.instance; //Set the Game Manager Reference
        amountTxt = amountTxtGameObject.GetComponent<TextMeshProUGUI>();    //set Reference to the TMP for the amount of players
    }

    //Removes players
    public void RemovePlayer() {
        players = Math.Clamp(--players, 1, manager.PLAYER_MAX); //Decrease the amount of players and keep it between 1-the mximum amount of players set
        UpdateDisplay();
    }
    public void AddPlayer() {
        players = Math.Clamp(++players, 1, manager.PLAYER_MAX); //Increases the amount of players and keep it between 1-the mximum amount of players set
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        amountTxt.text = players.ToString();
    }

    public void SubmitPlayerAmount() 
    {
        manager.numberOfPlayers = players;
    }
}
