using UnityEngine;

public class Tank_Pawn : Pawn
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void MoveForward() //Move the tank Forwards using rigid body
    {
        if (mover!=null) { 
        mover.Move(transform.forward, movementSpeed);
            }
        else{
            Debug.LogWarning("No Movement Component Attatched to Pawn.");
        }
    }
    public override void MoveBackwards() // move the tank back using rigid body
    {
        if (mover != null)
        {
            mover.Move(-transform.forward, movementSpeed);
        }
        else { 
            Debug.LogWarning("No Movement Component Attatched to Pawn.");
        }
    }

    public override void TurnClockwise() // turn the tank Right
    {
        if (mover!=null){
        mover.Rotate(turnSpeed);
            }
        else{
            Debug.LogWarning("No Movement Component Attatched to Pawn.");
        }
    }
    public override void TurnCounterClockwise() //Turn the tank Left
    {
        if (mover!=null){
        mover.Rotate(-turnSpeed);
            }
        else{
            Debug.LogWarning("No Movement Component Attatched to Pawn.");
        }
    }
}
