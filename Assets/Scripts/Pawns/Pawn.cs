using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float movementSpeed;
    private float movementSpeed_Attack;
    public float turnSpeed;
    public int scoreReward;

    public Mover mover;
    public Controller controller;

    // Start is called before the first frame update
    public virtual void Start()
    {
        movementSpeed_Attack = movementSpeed / 2;
        mover = GetComponent<Mover>();        //Set mover reference by attatched component
    }

    //===| PAWN KINEMATICS |===
    public abstract void MoveForward(float speed);
    public abstract void MoveBackwards(float speed);
    public abstract void TurnClockwise(float turnSpeed);
    public abstract void TurnCounterClockwise(float turnSpeed);
    public abstract void RotateTowards(Vector3 targetPos);

    //===| PAWN ACTIONS|===
    public abstract void Primary();

}