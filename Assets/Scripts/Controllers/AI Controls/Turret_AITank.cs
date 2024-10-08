using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AIController;
using static UnityEngine.GraphicsBuffer;

public class Turret_AITank : AIController
{
    public override void ProcessInputs()
    {
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

                //Can the AI hear the player?
                if (CanHear(null, targetList))
                {
                    ChangeState(AIState.Scan); //Switch to Chase State
                }
                //Can directly See the target
                if (CanSee(null, targetList))
                {
                    ChangeState(AIState.Attack); //chase the target
                }

                break;     
            //In Attack State
            case AIState.Attack:
                DoAttackState(); //Attack the target

                //lost sight of the Target
                if (!CanSee(null, targetList))
                {
                    ChangeState(AIState.Scan); //Scan for Target
                }
                break;
            //In Scan State
            case AIState.Scan:
                DoScan(); //Scan the Environment
                //Target is spotted?
                if (CanSee(null, targetList))
                {
                    //Chase the Target
                    ChangeState(AIState.Attack);
                }
                //Has it been enough time scanning?
                if (HasTimePassed(ScanSpan))
                {
                    //Nobody there, go back to post | Must have been the wind
                    ChangeState(AIState.Guard);
                }
                break;
            //In Back to Post State

            default:
                break;
        }
    }
}
