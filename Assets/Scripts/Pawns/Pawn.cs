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
    private void OnDestroy()
    {
        mover = null;
    }
    //===| PAWN KINEMATICS |===
    public abstract void MoveForward(float speed);
    public abstract void MoveBackwards(float speed);
    public abstract void TurnClockwise(float turnSpeed);
    public abstract void TurnCounterClockwise(float turnSpeed);
    public abstract void RotateTowards(Vector3 targetPos);

    //===| PAWN ACTIONS|===
    public abstract void Primary();

    //Gives another controller this pawn's score reward
    public void DealScoreReward(Controller controllerDealt)
    {
        controllerDealt.AddToScore(scoreReward);   //Add to the score of the source
    }
}