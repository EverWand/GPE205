using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawner : MonoBehaviour
{
   public PawnSpawner nextWaypoint;
    
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.instance.pawnSpawns.Add(this);
    }
    private void OnDestroy()
    {
        GameManager.instance.pawnSpawns.Remove(this);
    }
}
