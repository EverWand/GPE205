using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scared_AITank : AIController
{
    public float followDistance;

    public override void ProcessInputs()
    {
        //Is there a target to interact with?
        if (targets == null || !pawn)
        {
            return;
        }

        //FSM
        switch (currState)
        {
            //In Guard State
            case AIState.Guard:
                DoGuardState(); //Gaurd current Post

                //Can the AI hear the player?
                if (CanHear(null, targets))
                {
                    ChangeState(AIState.Scan); //Switch to Chase State
                }
                //Can directly See the target
                if (CanSee(null, targets))
                {
                    ChangeState(AIState.Chase); //chase the target
                }

                break;
            //In Chase State
            case AIState.Chase:
                DoChase(); //Chase the Target
                if (targets == null)
                {
                    ChangeState(AIState.Guard);
                }
                //Is the Target too far away?
                if (!IsDistanceLessThan(chaseDistance,null, targets))
                {
                    ChangeState(AIState.Guard); //go back to guard state
                }
                //is the target lined up and within attacking range?
                if (CanSee(null, targets) && IsDistanceLessThan(attackRange, null, targets))
                {
                    ChangeState(AIState.Attack); //Attack the target
                }
                //Did the AI lose its target for a given amount of time?
                if (!CanSee(null, targets) && HasTimePassed(AttentionSpan))
                {
                    ChangeState(AIState.Scan);
                }
                break;
            //In Flee State
            case AIState.Flee:
                DoFlee();
                if(IsDistanceLessThan(followDistance, FindHealthiestAI().gameObject) && FindHealthiestAI() != this)
                {
                    ChangeState(AIState.Scan);
                } //If it's close enough to the ally
                break;
            //In Patrol State
            case AIState.Patrol:
                //Can the AI hear the player?
                if (CanHear(null, targets))
                {
                    ChangeState(AIState.Scan); //Switch to Chase State
                }
                //Can directly See the target
                if (CanSee(null, targets))
                {
                    ChangeState(AIState.Chase); //chase the target
                }

                break;
            //In Attack State
            case AIState.Attack:
                DoAttackState(); //Attack the target

                //lost sight of the Target
                if (!CanSee(null, targets))
                {
                    ChangeState(AIState.Scan); //Scan for Target
                }
                //Is Health Below half?
                if (IsHealthBelow(.50))
                {
                    ChangeState(AIState.Flee); //Run to a healthier tank
                }
                break;
            //In Scan State
            case AIState.Scan:
                DoScan(); //Scan the Environment
                //Target is spotted?
                if (CanSee(null, targets))
                {
                    //Chase the Target
                    ChangeState(AIState.Chase);
                }
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

                if (CanSee(null, targets)) { ChangeState(AIState.Chase); }
                if (CanHear(null, targets)) { ChangeState(AIState.Scan); }

                break;
            //In Unknown State
            default:
                break;
        }
    }
}