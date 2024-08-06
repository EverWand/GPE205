using System;
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

    private float AttentionStartTime; //Used to start when attention limits are needed
    public GameObject target;          //Target the AI is attempting to sense

    public List<WaypointScript> wayPoints; //List of the AI's gaurd Posts


    public int currWaypointID = 0;              //current WaypointID
    public GameObject wayPointPrefab;           //the prefab of a waypoint
    public GameObject currWayPoint;             //the gameobject of the current waypoint
    public WaypointScript currWayPointScript;   //the script of the current waypoint

    private int directionSwitch = 1;            // 1: go through Waypoints forwards | -1: Go through waypoints Backwards


    // Start is called before the first frame update
    void Start()
    {
        wayPoints ??= new List<WaypointScript>(); //Created a new list instance if it is originally null
        pawn.controller = this;

        // Does the AI have any waypoints?
        if (wayPoints.Count <= 0)
        {

            ChangeState(AIState.Guard);   // Default to the Guard State

            // Spawn a Waypoint at the current position of the AI
            currWayPoint = Instantiate(wayPointPrefab, pawn.transform.position, Quaternion.identity);

            // Add the waypoint to the needed lists
            GameManager.instance.mapGenerator.wayPoints.Add(currWayPoint.GetComponent<WaypointScript>());
            wayPoints.Add(currWayPoint.GetComponent<WaypointScript>());

            UpdatePost(currWaypointID);
        }
        // There are waypoints
        else
        {
            UpdatePost(currWaypointID);     // update to next waypoint+
            ChangeState(AIState.Patrol);    // Default to Patrol State
        }
    }

    //When the controller is destroyed
    private void OnDestroy()
    {
        GameManager.instance.AIControllerList.Remove(this); //Remove this controller to the Game Manager AI Controller List
    }

    //====| OVERRIDE FUNCTIONS |====
    public override void addToManager()
    {
        GameManager.instance.AIControllerList.Add(this);    // Add this controller to the Game Manager AI Controller List
    }

    //Overridding function to process the different inputs of the contoller (AKA: The FSM)
    public override void ProcessInputs()
    {
        //Is there a target to interact with?
        if (!target || !pawn)
        {
            return;
        }

        //FSM
        switch (currState)
        {
            //In Guard State
            case AIState.Guard:
                DoGuardState(); //Gaurd current Post

                CheckForPlayer();

                break;
            //In Chase State
            case AIState.Chase:
                DoChase(); //Chase the Target

                //Is the Target too far away?
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.BackToPost); //go back to guard state
                }
                //is the target lined up and within attacking range?
                if (CanSee(target) && IsDistanceLessThan(target, attackRange))
                {
                    ChangeState(AIState.Attack); //Attack the target
                }
                //Did the AI lose its target for a given amount of time?
                if (!CanSee(target) && HasTimePassed(AttentionSpan))
                {
                    ChangeState(AIState.Scan);
                }
                break;
            //In Flee State
            case AIState.Flee:
                break;
            //In Patrol State
            case AIState.Patrol:
                DoPatrol();
                CheckForPlayer();

                break;
            //In Attack State
            case AIState.Attack:
                DoAttackState(); //Attack the target

                //lost sight of the Target
                if (!CanSee(target) && HasTimePassed(AttentionSpan))
                {
                    ChangeState(AIState.Scan); //Scan for Target
                }
                break;
            //In Scan State
            case AIState.Scan:
                DoScan(); //Scan the Environment
                CheckForPlayer();
                //Has it been enough time scanning?
                if (HasTimePassed(ScanSpan))
                {
                    //Nobody there, go back to post | Must have been the wind
                    ChangeState(AIState.BackToPost);
                }
                break;
            //In Back to Post State
            case AIState.BackToPost:
                DoBackToPost();

                CheckForPlayer();
                if (IsDistanceLessThan(currWayPoint, currWayPointScript.posThreshold))
                {
                    ChangeState(AIState.Guard);
                }
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
        currState = newState;           //switch to the new state
        AttentionStartTime = 0;         //Reset AI's Attention
    }

    public void CheckForPlayer()
    {
        target = FindClosestPlayer();

        if (target != null)
        {
            // Transition to chase state if player is within chase distance
            if (Vector3.Distance(transform.position, target.transform.position) < chaseDistance)
            {
                ChangeState(AIState.Chase);
            }
            //Transition to scan state if player is heard
            else if (CanHear(target))
            {
                ChangeState(AIState.Scan);
            }
        }
    }

    public GameObject FindClosestPlayer()
    {
        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (PlayerController player in GameManager.instance.playerList)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestPlayer = player.gameObject;
                closestDistance = distance;
            }
        }

        return closestPlayer;
    }

    //---Guards Action
    public void DoGuardState()
    {
        //Do Guard Stuff
    }
    //---Chase Action
    public void DoChase()
    {
        //Find the Target
        if (!target) return;
        Seek(target);
    }
    //---Flee Action
    public void DoFlee()
    {
        AIController currHealthiestAI = FindHealthiestAI();

        //If no healthiest Enemy: Go back to seek out current Post
        if (currHealthiestAI == this)
        {
            Seek(GetPostPos()); //Go back to your waypoint
        }
        else
        {
            Seek(currHealthiestAI.gameObject);  //find healthiest enemy: Go to that enemy
        }
    }
    //---Patrol Action
    public void DoPatrol()
    {
        if (wayPoints == null || wayPoints.Count == 0)
        {
            Debug.LogWarning("No waypoints set for patrol.");
            return;
        }

        if (currWaypointID >= 0 && currWaypointID < wayPoints.Count)
        {
            Seek(currWayPoint);
            if (IsDistanceLessThan(currWayPoint, currWayPointScript.posThreshold))
            {
                currWaypointID += directionSwitch;
                if (currWaypointID >= wayPoints.Count || currWaypointID < 0)
                {
                    directionSwitch *= -1;
                    currWaypointID += directionSwitch;
                }
                UpdatePost(currWaypointID);
            }
        }
    }
    //---Attack Action
    public void DoAttackState()
    {
        Seek(target);
        pawn.Primary();
    }
    //---Scanning Action
    public void DoScan()
    {
        pawn.TurnClockwise(pawn.turnSpeed); //rotate
    }
    //---BackToPost Action
    public void DoBackToPost()
    {
        Seek(GetPostPos()); //go to the current waypoint
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
        Vector3 directionToPos = targetPosition - pawn.transform.position;

        //===OBSTACLE AVOIDANCE===
        if (ShouldAvoidObstacle())
        {
            AvoidObstacles();
        }

        //Go to target position
        else
        {
            //Look at the position
            pawn.RotateTowards(targetPosition);
            //Move Foward
            pawn.MoveForward(pawn.movementSpeed);
        }
    }

    //Function used to move around obstacles
    public void AvoidObstacles()
    {
        //Shoot out a raycast for a GameObject
        GameObject seenObject = ShootRaycast(pawn.transform.forward, maxSteerDistance);
        //Get the Collider of the Game Object if it has one
        Collider obstacle = seenObject?.GetComponent<Collider>();

        //Is there a collider that's not the target?
        if (obstacle && seenObject != target)
        {
            float currDistance = Vector3.Distance(pawn.transform.position, obstacle.transform.position); // The Current Distance of the agent to the collider
            float steeringAdjustPercent = 0; //varibale to set an adjustment to the turn speed

            steeringAdjustPercent = Math.Clamp(currDistance / maxSteerDistance, 0, 1); // Set Steering distance 0% - 100% adjustment

            float steerSpeed = pawn.turnSpeed * steeringAdjustPercent; //the turning speed with modifier

            // Implement friction or deceleration when turning
            float forwardSpeedAdjustment = Mathf.Lerp(pawn.movementSpeed, pawn.movementSpeed * 0.5f, steeringAdjustPercent); // Reduce speed during turns

            pawn.MoveForward(forwardSpeedAdjustment); // Move forward with adjusted speed

            pawn.TurnClockwise(steerSpeed); //Rotate Clockwise with the AdjustedSpeed
        }
        else
        {
            pawn.MoveForward(pawn.movementSpeed);
        }
    }
    //Helps find The Healthiest AI
    public AIController FindHealthiestAI()
    {
        AIController currHealthiest = this; //set this controller as the current healthiest AI

        //Go through all AIControllers in the game manager
        foreach (AIController controller in GameManager.instance.AIControllerList)
        {
            //Grab the Health Systems of both th e Current Healthiest AI and the AI this iteration
            HealthSystem currPawnHealth = currHealthiest.pawn?.GetComponent<HealthSystem>();
            HealthSystem checkPawnHealth = controller.pawn?.GetComponent<HealthSystem>();

            //Is the checked health more than the current healthiest?
            if (checkPawnHealth.currHealth > currPawnHealth.currHealth)
            {
                currHealthiest = controller; //Set the iterated controller as the new healthiest AI
            }
        }

        return currHealthiest; //return the found healthiest AI
    }
    public bool IsHealthBelow(double percentage)
    {
        HealthSystem HP = pawn.GetComponent<HealthSystem>();
        if (HP != null)
        {
            double currPercentage = HP.currHealth / HP.maxHealth;

            return currPercentage < percentage;
        }
        return false;
    }
    //---CONDITION FUNCTIONS---
    //Checks if there's an Obstacle in front of the agent
    public bool ShouldAvoidObstacle()
    {
        //Get GameObject Seen and it's collider
        GameObject seenObject = ShootRaycast(pawn.transform.forward, maxSteerDistance);
        Collider obstacle = seenObject?.GetComponent<Collider>();

        //are we seeing an obstacle [not including the target]?
        if (obstacle != null && seenObject != target)
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
    public bool HasTimePassed(float timeLimit)
    {
        //if a negative number is set, Then there's no time limit | Never reaches the time limit
        if (timeLimit < 0)
        {
            return false;
        }

        //Is there no current time started?
        if (AttentionStartTime <= 0)
        {
            AttentionStartTime = Time.time; //Pin the time when the timer starts
        }

        float timePassed = Time.time - AttentionStartTime;  //total time that has passed since setting the timer

        //Has the elapsed time gone over the time limit?
        if (timePassed >= timeLimit)
        {
            AttentionStartTime = 0; //Reset the Timer for next time
            return true;
        }

        return false; //Time is still going
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
        if (noiseMaker.noiseDistance <= 0) { return false; }

        //===| HEARING TEST |===
        //Farthest distance the AI would be able to hear the targetted noise
        float totalDistance = noiseMaker.noiseDistance + hearingDistance;

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
            if (ShootRaycast(agentToTargetVector, viewDistance) == target) { return true; }

            return false; //Dont See the Target
        }

        return false; // Target is not within FOV
    }

    //Get Position of current WayPoint
    public Vector3 GetPostPos()
    {
        // Ensure there are waypoints and the current waypoint ID is valid
        if (wayPoints == null || wayPoints.Count == 0 || currWaypointID < 0 || currWaypointID >= wayPoints.Count)
        {
            Debug.LogError("GetPostPos(): Invalid waypoints or currWaypointID out of range.");
            return Vector3.zero; // Return a default position or handle appropriately
        }

        return currWayPoint.transform.position;
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

    //Used to update the information for the current post
    private void UpdatePost(int postID)
    {
        currWaypointID = postID;    //Set the Current Way Point ID
        Mathf.Clamp(currWaypointID, 0, wayPoints.Count - 1); //Make sure WaypointID is within the range of the waypoint list
        currWayPointScript = wayPoints[currWaypointID];      //Set the Current Waypoint Script
        currWayPoint = currWayPointScript.gameObject;        //Set up the Current Waypoint Object
    }
}