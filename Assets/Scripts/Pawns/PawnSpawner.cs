using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawner : MonoBehaviour
{    
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.instance.mapGenerator.pawnSpawns.Add(this);
    }
    private void OnDestroy()
    {
        GameManager.instance.mapGenerator.pawnSpawns.Remove(this);
    }
}
