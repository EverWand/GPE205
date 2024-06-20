using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    public KeyCode MoveForward;
    public KeyCode MoveBackward;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    public KeyCode Primary;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    public override void ProcessInputs()
    {
        //============| MOVEMENT |============
        if (Input.GetKey(MoveForward)) {
            pawn.MoveForward();
        }
        if (Input.GetKey(MoveBackward))
        {
            pawn.MoveBackwards();
        }
        if (Input.GetKey(TurnLeft))
        {
            pawn.TurnCounterClockwise();
        }
        if (Input.GetKey(TurnRight))
        {
            pawn.TurnClockwise();
        }

        //============| ACTIONS |============
        if (Input.GetKeyDown(Primary)) {
            //tank Shooting goes here

        }
    }
}
