using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;   //controller's Pawn

    //SCORE VARIABLE
    public int score = 0;                   //value for score
    //---Score Events
    public event Action On_Score_Change;    //Triggers When the Score Changed
    public UnityEvent Score_Added;          //Triggers when score is added
    public UnityEvent Score_Removed;        //Triggers when score is lost

    //LIVES VARIABLE
    public int lives = 3;
    //---Score Events
    public event Action On_NoLives;         //Triggers when there are 0 lives left
    public event Action On_Lives_Change;    //Triggers when lives value changes
    public UnityEvent Life_Lost;            //Triggers when lives are lost
    public UnityEvent Life_Gained;          //Triggers when lives are added


    //===|SCHEDULES|===
    private void Awake()
    {
        if (!pawn)
        {
            Debug.LogWarning("No pawn added to " + gameObject.name + " Controller Component.");
        }

        On_NoLives += Handle_NoLives;   //Subscribe to No Lives event
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    //===|ABSTRACTED FUNCTIONS|===
    public abstract void ProcessInputs(); //For input reading
    public abstract void addToManager(); // to add the controller to the game manager

    //===|FUNCTIONS|===

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
        lives += amountAdded;   //increase lives value

        Life_Gained?.Invoke();      //signal that lives has increased
        On_Lives_Change?.Invoke();  //signal that lives has changed
    }
    //Removes lives by value
    public void RemoveLives(int amountRemoved)
    {
        lives -= amountRemoved;
        //Debug.Log("Losing a life for " + pawn.gameObject.name);


        //is lives below or is 0
        if (lives <= 0)
        {
            lives = 0;              //Make sure lives is 0
            On_NoLives?.Invoke();   //Signal if there's no lives left
        }

        Life_Lost?.Invoke();        //Signal that a life has been lost
        On_Lives_Change?.Invoke();  //signal that lives value changed
    }

    private void Handle_NoLives()
    {
        //Debug.Log("Handling No Lives for " + pawn.gameObject.name);

        //Destroy both the Pawn and the controller
        Destroy(pawn.gameObject);
        Destroy(gameObject);
    }
}