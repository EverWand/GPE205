using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Speed : Powerup
{
    public float speedToAdd;

    public override void ApplyEffect(Powerup_Manager target)
    {
        //Apply Health Changed
        Pawn targetMove = target.GetComponent<Pawn>();
        if (targetMove)
        {
            targetMove.movementSpeed += speedToAdd;
        }
    }
    public override void RemoveEffect(Powerup_Manager target)
    {
        //Remove Health Changes
        Pawn targetMove = target.GetComponent<Pawn>();

        if (targetMove)
        {
            targetMove.movementSpeed -= speedToAdd;
        }
    }
}
