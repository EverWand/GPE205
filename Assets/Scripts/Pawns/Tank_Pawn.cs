using UnityEngine;

public class Tank_Pawn : Pawn
{
    Shooter shooter;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //===| PAWN KINEMATICS |===
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
    public override void RotateTowards(Vector3 targetPos) { 
        Vector3 vectorToTarget = targetPos - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    //===| PAWN ACTIONS |===
    public override void Primary() 
    {
        shooter.Shoot();
    }

}

