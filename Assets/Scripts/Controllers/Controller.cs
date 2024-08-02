using UnityEngine;
using UnityEngine.Events;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;

    public int score = 0;
    public UnityEvent Score_Updated = new UnityEvent();
    public UnityEvent Score_Added = new UnityEvent();
    public UnityEvent Score_Removed = new UnityEvent();

    public int lives = 3;
    public UnityEvent Lives_Updated = new UnityEvent();
    public UnityEvent Life_Lost = new UnityEvent();
    public UnityEvent Life_Gained = new UnityEvent();

    //===|SCHEDULES|===
    private void Awake()
    {
        if (!pawn )
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
    public void SyncWithPawnDestroy() 
    {
       if (!pawn)
        {
            Destroy(gameObject);
        }
  
    }
    public void AddToScore(int addedScore) 
    { 
        score += addedScore;

        Score_Added.Invoke();
        Score_Updated.Invoke();
    }
    public void RemoveFromScore(int removedscore)
    {
        score -= removedscore;

        Score_Removed.Invoke();
        Score_Updated.Invoke();
    }
    public void AddLives(int amountAdded) 
    {
        lives += amountAdded;

        Life_Gained.Invoke();
        Lives_Updated.Invoke();
    }
    public void RemoveLives(int amountRemoved) 
    { 
        lives -= amountRemoved;

        Life_Lost.Invoke();
        Lives_Updated.Invoke();
    }

    public void handleLivesUpdate() 
    {
        GameManager.instance.ActivateGameOverScreen();
    }
}