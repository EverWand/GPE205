using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    //
    GameManager gameManager;
    public TextMeshProUGUI HsDisplay;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        //---from Game Manager
        gameManager.On_HighScore_Change += UpdateDisplay; //Link Highscore Display
    }

    //Updates the value display
    public void UpdateDisplay()
    {
        //get the current Highscore
        int hs = gameManager.highScore;

        //set the text to the highscore
        HsDisplay.text = hs.ToString();
    }
}
