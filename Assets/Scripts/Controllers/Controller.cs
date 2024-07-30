using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;
    public int score = 0;

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
    }
    public void RemoveFromScore(int removedscore)
    {
        score -= removedscore;
    }
}

