using UnityEngine;

public class AIController : Controller
{
    //Behavioral States
    public enum AIState { Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost }
    public AIState currState; // Tracks what state the AI is in

    //---SENSORY VARIABLES---
    //---Hearing
    public float chaseDistance = 10;    //Distance the AI can Chase
    public float hearingDistance;       //Distance the AI can Hear
    //---Seeing
    public float fieldOfView;           //The max angle that the AI can see
    public float ViewDistance;          //The distance of which the AI can see

    public float timeLastSwitched;  //tracks the time it takes to transition into another state
    public GameObject target; //target the AI is attempting to sense


    // Start is called before the first frame update
    void Start()
    {
        ChangeState(AIState.Guard);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    public override void ProcessInputs()
    {
        switch (currState)
        {
            //In Guard State
            case AIState.Guard:
                DoGuardState();
                if (!target) { return; } // Do not proceed if there's no target

                if (CanHear(target))//Can the AI hear the player?
                {
                    ChangeState(AIState.Chase); //Switch to Chase State
                }
                break;
            //In Chase State
            case AIState.Chase:
                DoChase(); //Chase the Target
                if(target == null) { return; }
                //Is the Target too far away?
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Guard); //go back to guard state
                }
                break;
            //In Flee State
            case AIState.Flee:
                break;
            //In Patrol State
            case AIState.Patrol:
                break;
            //In Attack State
            case AIState.Attack:
                break;
            //In Scan State
            case AIState.Scan:
                break;
            //In Back to Post State
            case AIState.BackToPost:
                break;
            //In Unknown State
            default:
                break;
        }
    }

    //Used to Change to a different State
    public virtual void ChangeState(AIState newState)
    {
        //switch to the new state
        currState = newState;
        //set this as the new time that this has  been last switched
        timeLastSwitched = Time.time;
    }

    public void DoGuardState()
    {
        //Do Guard Stuff
    }

    public void DoChase()
    {
        //Find the Target
        if (target == null) { return;}
        Seek(target);
    }

    public void Seek(GameObject target) //Seek GameObject
    {
        //Look at the Game Objects Position
        Seek(target.transform.position);
    }
    public void Seek(Vector3 targetPosition) //Seek Position
    {
        //Look at the position
        pawn.RotateTowards(targetPosition);
        pawn.MoveForward();
    }
    //Returns wether the given target is within a certain distance
    public bool IsDistanceLessThan(GameObject target, float distance)
    {
        //Checks if the target is within the distance given from the this pawn's postion.
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            //Within Range
            return true;
        }
        else
        {
            //Out of Range
            return false;
        }
    }

    //Return wether the AI can Hear the target
    public bool CanHear(GameObject target)
    {
        //Get Target's Noisemaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        //===| HEARING PREREQUISITES |===
        //Make sure the target makes noise
        if (noiseMaker == null) { return true; }
        //Make sure the target's noise has a volume
        if (noiseMaker.volumeDistance <= 0) { return true; }

        //===| HEARING TEST |===
        //Farthest distance the AI would be able to hear the targetted noise
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;

        //Checks if the AI distance from the target is within the total hearing distance calculated
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            return true;
        }

        return false;
    }

    //Return wether the AI can see the Target
    public bool CanSee(GameObject target)
    {
        //Get vector pointing to the target
        Vector3 agentToTargetVector = target.transform.position - pawn.transform.position;
        //Get the angle to the targeted vector from where the AI is looking forward
        float angleToVector = Vector3.Angle(agentToTargetVector, target.transform.forward);

        if (angleToVector < fieldOfView && IsDistanceLessThan(target, ViewDistance)) { return true; }   // 
        else { return false; }
    }
}
