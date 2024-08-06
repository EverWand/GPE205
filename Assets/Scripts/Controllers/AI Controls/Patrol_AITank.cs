public class Patrol_AITank : AIController
{

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
                if (HasTimePassed(PostSpan))
                {
                    ChangeState(AIState.Patrol);
                }

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
            //In Patrol State
            case AIState.Patrol:
                DoPatrol();

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
                //Target is spotted?
                if (CanSee(target))
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

                if (CanSee(target)) { ChangeState(AIState.Chase); }
                if (CanHear(target)) { ChangeState(AIState.Scan); }
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
}
