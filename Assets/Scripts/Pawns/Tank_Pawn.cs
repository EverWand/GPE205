using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Pawn : Pawn
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void MoveForward() //Move the tank Forwards using rigid body
    {
        Debug.Log("Moving Foward");
    }
    public override void MoveBackwards() // move the tank back using rigid body
    {

        Debug.Log("Moving Backwards");
    }

    public override void TurnClockwise() // turn the tank Right
    {

        Debug.Log("Turning Clockwise");
    }
    public override void TurnCounterClockwise() //Turn the tank Left
    {

        Debug.Log("Turning Counter Clockwise");
    }
}
