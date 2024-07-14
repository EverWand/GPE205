using UnityEngine;

public class Tank_Pawn : Pawn
{
    Shooter shooter;
    private NoiseMaker noiseMaker;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        shooter = gameObject.GetComponent<Shooter>();
        noiseMaker = gameObject.GetComponent<NoiseMaker>();
    }

    //===| PAWN KINEMATICS |===
    public override void MoveForward(float speed) //Move the tank Forwards using rigid body
    {
        if (mover!=null) { 
            mover.Move(transform.forward,speed);
        }
        else{
            Debug.LogWarning("No Movement Component Attatched to Pawn.");
        }

    }
    public override void MoveBackwards(float speed) // move the tank back using rigid body
    {
        if (mover != null)
        {
            mover.Move(-transform.forward, speed);
        }
        else { 
            Debug.LogWarning("No Movement Component Attatched to Pawn.");
        }
    }

    public override void TurnClockwise(float turnSpeed) // turn the tank Right
    {
        if (mover!=null){
        mover.Rotate(turnSpeed);
            }
        else{
            Debug.LogWarning("No Movement Component Attatched to Pawn.");
        }
    }
    public override void TurnCounterClockwise(float turnSpeed) //Turn the tank Left
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

