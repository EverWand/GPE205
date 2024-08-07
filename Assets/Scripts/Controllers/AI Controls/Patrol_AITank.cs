public class Patrol_AITank : AIController
{

    //Overridding function to process the different inputs of the contoller (AKA: The FSM)
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
                if (HasTimePassed(PostSpan))
                {
                    ChangeState(AIState.Patrol);
                }

                break;
            //In Chase State
            case AIState.Chase:
                DoChase(); //Chase the Target

                //Is the Target too far away?
                if (!IsDistanceLessThan(chaseDistance, null, targets))
                {
                    ChangeState(AIState.BackToPost); //go back to guard state
                }
                //is the target lined up and within attacking range?
                if (CanSee(null, targets) && IsDistanceLessThan(attackRange, null, targets))
                {
                    ChangeState(AIState.Attack); //Attack the target
                }
                //Did the AI lose its target for a given amount of time?
                if (!CanSee(null,targets) && HasTimePassed(AttentionSpan))
                {
                    ChangeState(AIState.Scan);
                }
                break;
            //In Patrol State
            case AIState.Patrol:
                DoPatrol();

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
                if (!CanSee(null, targets) && HasTimePassed(AttentionSpan))
                {
                    ChangeState(AIState.Scan); //Scan for Target
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

}
