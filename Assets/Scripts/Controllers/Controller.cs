using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;
    
    //SCORE VARIABLE
    public int score = 0;
    //---Score Events
    public event Action On_Score_Change;
    public UnityEvent Score_Added;
    public UnityEvent Score_Removed;

    //LIVES VARIABLE
    public int lives = 3;
    //---Score Events
    public event Action On_Lives_Change;
    public UnityEvent Life_Lost;
    public UnityEvent Life_Gained;

    //===|SCHEDULES|===
    private void Awake()
    {
        if (!pawn)
        {
            Debug.LogWarning("No pawn added to " + gameObject.name + " Controller Component.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        SyncWithPawnDestroy();
        ProcessInputs();
    }

    //===|ABSTRACTED FUNCTIONS|===
    public abstract void ProcessInputs();

    //===|FUNCTIONS|===
    //make sure the controller is destroyed if it its pawn is destroyed
    public void SyncWithPawnDestroy()
    {
        if (!pawn)
        {
            Destroy(gameObject);
        }

    }

    //Invokes variables needed for UI
    public void InvokeUiVariables() 
    {
        On_Lives_Change?.Invoke();
        On_Score_Change?.Invoke();
    }

    //---SCORE
    //Adds value to score
    public void AddToScore(int addedScore)
    {
        score += addedScore;

        Score_Added?.Invoke();
        On_Score_Change?.Invoke();
    }
    //Removes score by value
    public void RemoveFromScore(int removedscore)
    {
        score -= removedscore;

        Score_Removed.Invoke();
        On_Score_Change.Invoke();
    }
    //---LIVES
    //Add Lives by value
    public void AddLives(int amountAdded)
    {
        lives += amountAdded;

        Life_Gained?.Invoke();
        On_Lives_Change?.Invoke();
    }
    //Removes lives by value
    public void RemoveLives(int amountRemoved)
    {
        lives -= amountRemoved;

        Life_Lost?.Invoke();
        On_Lives_Change?.Invoke();
    }

    //Tries to activate the Gameover state
    public void CheckGameOver()
    {
        GameManager.instance.ActivateGameOverScreen();
    }
}