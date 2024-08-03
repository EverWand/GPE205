using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    Controller controller;
    TextMeshPro scoreDisplay;

    private void Start()
    {
        scoreDisplay = GetComponent<TextMeshPro>();
    }

    public void UpdateScoreDisplay() 
    {
        scoreDisplay.text = controller.score.ToString();
    }
}
