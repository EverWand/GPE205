using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float movementSpeed;

    public float turnSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public abstract void MoveForward();
    public abstract void MoveBackwards();
    public abstract void TurnClockwise();
    public abstract void TurnCounterClockwise();
}
