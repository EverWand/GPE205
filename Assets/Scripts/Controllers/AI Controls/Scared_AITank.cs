using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scared_AITank : AIController
{
    public Transform[] waypoints;
    public override void ProcessInputs()
    { 
        switch (currState) 
        {
            case AIState.Guard:
                break;
        }
    }
}
