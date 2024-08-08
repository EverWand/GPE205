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
    public float AttentionSpan;         //Time Spent trying to refind targets if it's lost
    public float PostSpan;              //Time Spent at a Post before moving on

    //---ACTION VARIABLES---
    //---Attackig
    public float attackRange;           //Range of when the AI can use their attack

    //---Steering
    public float minSteerDistance;     //Distance for minimum steer strength
    public float maxSteerDistance;     //Maximum distance needed to steer

    private float AttentionStartTime;   //Used to start when attention limits are needed
    public List<GameObject> targetList = new List<GameObject>();    //Targets the AI is attempting to sense
    public GameObject focusTarget = null;

    public List<WaypointScript> wayPoints = new(); //List of the AI's gaurd Posts


    [HideInInspector] public int currWaypointID = 0;              //current WaypointID
    [HideInInspector] public WaypointScript currWayPointScript;   //the script of the current waypoint
    [HideInInspector] public GameObject currWayPoint;             //the gameobject of the current waypoint
    public GameObject wayPointPrefab;           //the prefab of a waypoint

    private int directionSwitch = 1;            // 1: go through Waypoints forwards | -1: Go through waypoints Backwards

    //====| SCHEDULES |====
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("AI CONTROLLER STARTING!!!");

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


        Debug.Log(this.gameObject.name + "grabbing default targets");
        targetList = GameManager.instance.DefaultAITargets;

    }

    //When the controller is destroyed
    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AIControllerList.Remove(this); //Remove this controller to the Game Manager AI Controller List
        }
    }

    //====| OVERRIDE FUNCTIONS |====
    public override void addToManager()
    {
        Debug.Log("--- AI Controller Added to Manager");
        GameManager.instance.AIControllerList.Add(this);    // Add this controller to the Game Manager AI Controller List
    }

    //Used to Clean out the target list if there are any null values
    public void CleanupTargetList()
    {
        // Remove any null references from the targetList
        targetList.RemoveAll(target => target == null);
    }
    //Overridding function to process the different inputs of the contoller (AKA: The FSM)
    public override void ProcessInputs()
    {
        CleanupTargetList();    // make sure the target list is clean of any null items

        //Is there a targets to interact with?
        if (CollectTargets(null, targetList).Count <= 0 || targetList == null || !pawn)
        {
            return;
        }

        //FSM
        switch (currState)
        {
            //In Guard State
            case AIState.Guard:
                DoGuardState(); //Gaurd current Post

                SenseForTarget();

                break;
            //In Chase State
            case AIState.Chase:
                DoChase(); //Chase the Target

                //Is the Target too far away?
                if (!IsDistanceLessThan(chaseDistance, focusTarget))
                {
                    ChangeState(AIState.BackToPost); //go back to guard state
                }
                //is the targets lined up and within attacking range?
                if (CanSee(null, targetList) && IsDistanceLessThan(chaseDistance, focusTarget))
                {
                    ChangeState(AIState.Attack); //Attack the targets
                }
                //Did the AI lose its targets for a given amount of time?
                if (!CanSee(null, targetList) && HasTimePassed(AttentionSpan))
                {
                    //Lost track of Target
                    focusTarget = null;
                    ChangeState(AIState.Scan);
                }
                break;
            //In Flee State
            case AIState.Flee:
                //Lost track of Target
                break;
            //In Patrol State
            case AIState.Patrol:
                //Not Looking for a specific Target
                focusTarget = null;
                DoPatrol();
                SenseForTarget();

                break;
            //In Attack State
            case AIState.Attack:
                DoAttackState(); //Attack the targets

                //lost sight of the Target
                if (!CanSee(null, targetList) && HasTimePassed(AttentionSpan))
                {
                    ChangeState(AIState.Scan); //Scan for Target
                }
                break;
            //In Scan State
            case AIState.Scan:
                DoScan(); //Scan the Environment
                
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

                SenseForTarget();
                if (IsDistanceLessThan(currWayPointScript.posThreshold, currWayPoint))
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

    //tries to find a target
    public void SenseForTarget()
    {
        if (targetList != null || targetList.Count > 0)
        {
            // Transition to chase state if player is within chase distance
            if (CanSee(null, targetList))
            {
                ChangeState(AIState.Chase);
            }
            //Transition to scan state if player is heard
            else if (CanHear(null, targetList))
            {
                ChangeState(AIState.Scan);
            }
        }
    }

    //---Check Senses
    //Return wether the AI can Hear the targets || USED TO FIND FOCUSED TARGET
    public bool CanHear(GameObject target = null, List<GameObject> targets = null)
    {
        if (focusTarget != null || targets == null || targets.Count <= 0) { return false; }


        //Iterate through each Target object within the target list
        foreach (GameObject targetObject in CollectTargets(target, targets))
        {
            //Get Target's Noisemaker
            NoiseMaker noiseMaker = targetObject.GetComponent<NoiseMaker>();

            //===| HEARING PREREQUISITES |===
            //Make sure the targets makes noise and it's in range
            if (noiseMaker == null || noiseMaker.noiseDistance <= 0 || focusTarget) { return false; }


            //===| HEARING TEST |===
            //Farthest distance the AI would be able to hear the targetted noise
            float totalDistance = noiseMaker.noiseDistance + hearingDistance;

            //Checks if the AI distance from the targets is within the total hearing distance calculated
            if (Vector3.Distance(pawn.transform.position, targetObject.transform.position) <= totalDistance)
            {
                focusTarget = targetObject; //Focus on that target it hears
                Debug.Log(pawn.gameObject.name + " hears a new target and will begin it's search");
                return true;
            }
        }
        return false;
    }

    //Return wether the AI can see the Target
    public bool CanSee(GameObject target, List<GameObject> targets)
    {
        if (targets == null || targets.Count <= 0) { return false; }
        //Collect the specified targets
        List<GameObject> targetList = CollectTargets(target, targets);
        foreach (GameObject targetObject in targetList)
        {
            //Get vector pointing to the targets
            Vector3 agentToTargetVector = targetObject.transform.position - pawn.transform.position;

            //Get the angle to the targeted vector from where the AI is looking forward
            float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

            //Is Target within FOV
            if (angleToTarget < fieldOfView)
            {
                //Do we get the targets within our veiw Raycasting?
                if (ShootRaycast(agentToTargetVector, viewDistance) == targetObject)
                {
                    //See a new target
                    focusTarget = targetObject;
                    Debug.Log(pawn.gameObject.name + " Sees a new target and will begin it's search");
                    return true;
                }

                return false; //Dont See the Target
            }
        }
        return false; // Target is not within FOV
    }

    public List<GameObject> CollectTargets(GameObject target = null, List<GameObject> targets = null)
    {
        List<GameObject> targetList = new();

        if (target != null)
        {
            targetList.Add(target);
        }
        if (targets != null)
        {
            targetList.AddRange(targets);
        }
        return targetList;
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
        //Find the Targets
        if (focusTarget == null) { return; }

        Seek(focusTarget);
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
            if (IsDistanceLessThan(currWayPointScript.posThreshold, currWayPoint))
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
        Seek(focusTarget);
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
    //---Seeking out targets
    public void Seek(GameObject target) //Seek GameObject
    {
        if (target != null)
        {
            Seek(target.transform.position);
        }
    }
    public void Seek(Vector3 targetPosition) //Seek Position
    {
        Vector3 directionToPos = targetPosition - pawn.transform.position;

        //===OBSTACLE AVOIDANCE===
        if (ShouldAvoidObstacle())
        {
            AvoidObstacles();
        }

        //Go to targets position
        else
        {
            //Look at the position
            pawn.RotateTowards(targetPosition);
            //Move Foward
            pawn.MoveForward(pawn.BaseMovementSpeed);
        }

        //DEBUG::
        Debug.Log("Seeking at a speed of: " + pawn.BaseMovementSpeed);
    }

    //Function used to move around obstacles
    public void AvoidObstacles()
    {
        //Shoot out a raycast for a GameObject
        GameObject seenObject = ShootRaycast(pawn.transform.forward, maxSteerDistance);
        //Get the Collider of the Game Object if it has one
        Collider obstacle = seenObject?.GetComponent<Collider>();

        //Is there a collider that's not the targets?
        if (obstacle && seenObject != focusTarget)
        {
            float currDistance = Vector3.Distance(pawn.transform.position, obstacle.transform.position); // The Current Distance of the agent to the collider
            float steeringAdjustPercent; //varibale to set an adjustment to the turn speed

            steeringAdjustPercent = Math.Clamp(currDistance / maxSteerDistance, 0, 1); // Set Steering distance 0% - 100% adjustment

            float steerSpeed = pawn.turnSpeed * steeringAdjustPercent; //the turning speed with modifier

            // Implement friction or deceleration when turning
            float forwardSpeedAdjustment = Mathf.Lerp(pawn.BaseMovementSpeed, pawn.BaseMovementSpeed / 2, steeringAdjustPercent); // Reduce speed during turns

            pawn.MoveForward(forwardSpeedAdjustment); // Move forward with adjusted speed

            Debug.Log("Moving at a speed of: " + forwardSpeedAdjustment + " || ORIGINAL SPEED WAS " + pawn.BaseMovementSpeed);


            pawn.TurnClockwise(steerSpeed); //Rotate Clockwise with the AdjustedSpeed
        }
        else
        {
            pawn.MoveForward(pawn.BaseMovementSpeed);
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


        foreach (GameObject targetObject in CollectTargets(null, targetList))
        {
            //are we seeing an obstacle [not including the targets]?
            if (obstacle != null && seenObject != targetObject)
            {
                return true;
            }
        }
        return false;
    }
    //Returns wether the given targets is within a certain distance
    public bool IsDistanceLessThan(float distance, GameObject target = null)
    {
        //Checks if the targets is within the distance given from the this pawn's postion.
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            //Within Range
            return true;
        }

        return false;  //No targets in range
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