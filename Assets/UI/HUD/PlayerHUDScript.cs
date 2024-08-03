using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHUDScript : MonoBehaviour
{
    //Controller reference
    public Pawn Pawn;
    Controller controller;
    
    //Text Elements
    public GameObject scoreDisplayObj;
    public GameObject highScoreDisplayObj;
    public GameObject livesDisplayObj;
    
    //TMP
    [HideInInspector] public TextMeshProUGUI scoreDisplay;
    [HideInInspector] public TextMeshProUGUI highScoreDisplay;
    [HideInInspector] public TextMeshProUGUI livesDisplay;


    private void Start()
    {
        //Set Controller
        controller = Pawn.controller;

        //Set the TCM References
        scoreDisplay = scoreDisplayObj.GetComponent<TextMeshProUGUI>();
        highScoreDisplay = highScoreDisplayObj.GetComponent<TextMeshProUGUI>();
        livesDisplay = livesDisplayObj.GetComponent<TextMeshProUGUI>();

        //Subscribe to the events
        //---from Controller
        if (controller != null)
        {
            controller.On_Score_Change += UpdateScoreDisplay;   //Link Score Display
            controller.On_Lives_Change += UpdateLivesDisplay;   //Link Lives Display
        }
        //---from Game Manager
        GameManager.instance.On_HighScore_Change += UpdateHighScoreDisplay; //Link Highscore Display

        FullHUDUpdate();
    }

    public void FullHUDUpdate() 
    {
        UpdateHighScoreDisplay();
        UpdateLivesDisplay();
        UpdateScoreDisplay();
    }

    //LIVES DISPLAY
    public void UpdateLivesDisplay()
    {
        livesDisplay.text = controller.lives.ToString(); //Sets the value of score to the text display
    }
    //HIGHSCORE DISPLAY
    public void UpdateHighScoreDisplay()
    {
        highScoreDisplay.text = GameManager.instance.highScore.ToString(); //Sets the value of score to the text display
    }
    //SCORE DISPLAY
    public void UpdateScoreDisplay()
    {
        scoreDisplay.text = controller.score.ToString();
    }
}