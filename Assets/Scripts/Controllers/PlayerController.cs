using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    public KeyCode MoveForward;
    public KeyCode MoveBackward;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;

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
        if (Input.GetKeyDown(MoveForward)) {
            pawn.MoveForward();
        }
        if (Input.GetKeyDown(MoveBackward))
        {
            pawn.MoveBackwards();
        }
        if (Input.GetKeyDown(TurnLeft))
        {
            pawn.TurnCounterClockwise();
        }
        if (Input.GetKeyDown(TurnRight))
        {
            pawn.TurnClockwise();
        }

    }
}
