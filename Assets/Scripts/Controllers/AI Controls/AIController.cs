using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AIController : Controller
{
    //Behavioral States
    public enum AIState { Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost }
    public AIState currState; // Tracks what state the AI is in

    public float chaseDistance = 10;

    public float timeLastSwitched;
    public GameObject target;

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
                if (IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            //In Chase State
            case AIState.Chase:
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Guard);
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
        Seek(target.transform.position);
    }

    public void Seek(GameObject target) //Seek Game Object
    {
        //Look at the Game Objects Position
        Seek(target.transform.position);
    }
    public void Seek(Vector3 targetPosition) //Seek Position
    {
        //Look at the position
        pawn.RotateTowards(targetPosition);

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
}
