using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AIController : Controller
{
    public static List<AIController> AllAIControllers;
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
    //---Scanning
    public float ScanSpan;              //Time spent scanning {Leave Negative if you want infinite Scan time}
    public float AttentionSpan;         //Time Spent trying to refind target if it's lost
    public float PostSpan;              //Time Spent at a Post before moving on

    //---ACTION VARIABLES---
    //---Attackig
    public float attackRange;           //Range of when the AI can use their attack

    //---Steering
    public float minSteerDistance;     //Distance for minimum steer strength
    public float maxSteerDistance;     //Maximum distance needed to steer

    private float timeLastSwitched;      //tracks the time it takes to transition into another state
    public GameObject target;           //target the AI is attempting to sense

    public List<Transform> wayPoints;    //List of the AI's gaurd Posts


    // Start is called before the first frame update
    void Start()
    {
        ChangeState(AIState.Guard);
        GameManager.instance.AIControllerList.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.instance.AIControllerList.Remove(this);
    }

    public override void ProcessInputs()
    {
        //Is there a target to interact with?
        if (!target || !pawn)
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
                if (ShouldAvoidObstacle() || !CanSee(target)) { 
                    AvoidObstacles();
                    
                    if (hasTimePassed(AttentionSpan)) 
                    {
                        
                    }
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
                //DoPostStuff
                currState = AIState.Guard;
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
        if (ShouldAvoidObstacle()) {
            AvoidObstacles();
        }
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
        pawn.TurnClockwise(pawn.turnSpeed);
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
        //Move Foward
        pawn.MoveForward();
    }

    //Function used to move around obstacles
    public void AvoidObstacles()
    {
        GameObject seenObject = ShootRaycast(pawn.transform.forward, maxSteerDistance);
        Collider obstacle = seenObject?.GetComponent<Collider>();

        if (obstacle != null && seenObject != target)
        {
            float currDistance = Vector3.Distance(pawn.transform.position, obstacle.transform.position); // The Current Distance of the agent to the collider
            float steeringAdjustPercent = 0; //varibale to set an adjustment to the turn speed

            steeringAdjustPercent = Math.Clamp(currDistance / maxSteerDistance, 0, 1); // Set Steering distance 0% - 100% adjustment

            float steerSpeed = pawn.turnSpeed * steeringAdjustPercent;

            pawn.TurnClockwise(steerSpeed); //Rotate Clockwise with the AdjustedSpeed
        }
    }

    //---CONDITION FUNCTIONS---
    //Checks if there's an Obstacle in front of the agent
    public bool ShouldAvoidObstacle() 
    {
        //Get GameObject Seen and it's collider
        GameObject seenObject = ShootRaycast(pawn.transform.forward, maxSteerDistance);
        Collider obstacle = seenObject?.GetComponent<Collider>();

        //are we seeing an obstacle [not including the target]?
        if ( obstacle != null && seenObject != target) 
        {
            return true;
        }

        return false;
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
    //Return if the the time in one state is over a given time limit
    public bool hasTimePassed(TimeSpan timer, float timeLimit)
    {
        //if a negative number is set, Then there's no time limit | Never reaches the time limit
        if (timeLimit < 0) {
            return false;
        }

        return timer.TotalSeconds >= timeLimit; //return if the the time passed is greater then the time limit
                                                            
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

        //Is Target within FOV
        if (angleToTarget < fieldOfView)
        {
            //Do we get the target within our veiw Raycasting?
            if(ShootRaycast(agentToTargetVector, viewDistance) == target) { return true; }
            
            return false; //Dont See the Target
        }

        return false; // Target is not within FOV
    }

    //Get an Game Object from a Raycast
    private GameObject ShootRaycast(Vector3 direction, float length) 
    {
        RaycastHit hit; //create a raycast

        //Is there something in front of the pawn?
        if (Physics.Raycast(pawn.transform.position, direction, out hit, length))
        {
            GameObject seenObject = hit.transform.gameObject;   //The Gameobject that's Seen by the raycast
            
            return seenObject;  //return the seen game object
        }

        return null; //No Game Object seen
    }
}
