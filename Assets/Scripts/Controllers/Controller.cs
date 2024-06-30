using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;

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

    public abstract void ProcessInputs();
    public void SyncWithPawnDestroy() 
    {
       if (!pawn)
        {
            Destroy(gameObject);
        }
  
    }
}

