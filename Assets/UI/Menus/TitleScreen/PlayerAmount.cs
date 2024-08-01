using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAmount : MonoBehaviour
{
    int players = 1;

    GameManager manager; //The Game Manager singleton
    public GameObject amountTxtGameObject;
    TextMeshProUGUI amountTxt;

    private void Start()
    {
        manager = GameManager.instance;
        amountTxt = amountTxtGameObject.GetComponent<TextMeshProUGUI>();
    }

    public void RemovePlayer() {
        players = Math.Clamp(--players, 1, manager.PLAYER_MAX);
        UpdateDisplay();
    }
    public void AddPlayer() {
        players = Math.Clamp(++players, 1, manager.PLAYER_MAX);
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
