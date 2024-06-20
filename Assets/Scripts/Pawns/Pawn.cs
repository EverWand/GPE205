using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float movementSpeed;
    public float turnSpeed;

    public Mover mover;

    // Start is called before the first frame update
    public virtual void Start()
    {
      mover = GetComponent<Mover>();        //Set mover reference by attatched component
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //===| PAWN KINEMATICS |===
    public abstract void MoveForward();
    public abstract void MoveBackwards();
    public abstract void TurnClockwise();
    public abstract void TurnCounterClockwise();
    public abstract void RotateTowards(Vector3 targetPos);

    //===| PAWN ACTIONS|===
    public abstract void Primary();

}