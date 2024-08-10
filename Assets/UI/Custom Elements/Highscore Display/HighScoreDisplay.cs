using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    GameManager gameManager;
    public TextMeshProUGUI HsDisplay;

    private void Start()
    {
        // Start is called before the first frame update
        gameManager = GameManager.instance;
        //---from Game Manager
        gameManager.On_HighScore_Change += UpdateDisplay; //Link Highscore Display
        
        UpdateDisplay();
    }


    //Updates the value display
    private void UpdateDisplay()
    {
        //get the current Highscore
        int hs = gameManager.highScore;
        //set the text to the highscore
        HsDisplay.text = hs.ToString();
    }
}
