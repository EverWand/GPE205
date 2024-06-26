using System.Collections.Generic;
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
    public float viewDistance;          //The distance of which the AI can see

    //---ACTION VARIABLES---
    //---Attackig
    public float attackRange;           //Range of when the AI can use their attack

    public float timeLastSwitched;      //tracks the time it takes to transition into another state
    public GameObject target;           //target the AI is attempting to sense

    public List<Transform> postList;    //List of the AI's gaurd Posts


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
        //Is there a target to interact with?
        if (!target)
        {
            return;
        }
        
        //React to Target
        switch (currState)
        {
            //In Guard State
            case AIState.Guard:
                DoGuardState(); //Gaurd current Post

                //Can the AI hear the player?
                if (CanHear(target))
                {
                    ChangeState(AIState.Scan); //Switch to Chase State
                }
                //Can directly See the target
                if (CanSee(target))
                {
                    ChangeState(AIState.Chase); //chase the target
                }
                break;
            //In Chase State
            case AIState.Chase:
                DoChase(); //Chase the Target
                if (target == null) 
                { 
                    ChangeState(AIState.Guard); 
                }
                //Is the Target too far away?
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Guard); //go back to guard state
                }
                //is the target lined up and within attacking range?
                if (CanSee(target) && IsDistanceLessThan(target, attackRange))
                {
                    ChangeState(AIState.Attack); //Attack the target
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
                DoAttackState(); //Attack the target
                
                //lost sight of the Target
                if (!CanSee(target)) 
                {
                    ChangeState(AIState.Scan); //Scan for Target
                }
                break;
            //In Scan State
            case AIState.Scan:
                DoScan(); //Scan the Environment
                //Target is spotted?
                if (CanSee(target))
                {
                    //Chase the Target
                    ChangeState(AIState.Chase);
                }
                break;
            //In Back to Post State
            case AIState.BackToPost:
                break;
            //In Unknown State
            default:
                break;
        }
    }


    //---STATE FUNCTIONS---
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
        if (target == null) { return; }
        Seek(target);
    }

    public void DoAttackState()
    {
        Seek(target);
        pawn.Primary();
    }
    //Scans the Environment by turning clockwise
    public void DoScan()
    {
        pawn.TurnClockwise();
    }


    //---ACTION FUNCTIONS---
    //---Seeking out target
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

    //---CONDITION FUNCTIONS---
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
    
    //---Check Senses
    //Return wether the AI can Hear the target
    public bool CanHear(GameObject target)
    {
        //Get Target's Noisemaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        //===| HEARING PREREQUISITES |===
        //Make sure the target makes noise
        if (noiseMaker == null) { return false; }
        //Make sure the target's noise has a volume
        if (noiseMaker.volumeDistance <= 0) { return false;}

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
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

        if (angleToTarget < fieldOfView)
        {
            RaycastHit hit; //create a raycast
            //does the ray cast hit an Object?
            if (Physics.Raycast(pawn.transform.position, agentToTargetVector, out hit, viewDistance))
            {
                GameObject seenGameObject = hit.transform.gameObject; //get the seen game object

                //is the object seen the Target?
                if (seenGameObject == target) { return true; }
            }
            return false;
        }
        return false;
    }
}
