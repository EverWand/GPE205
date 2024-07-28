using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup_DmgBoost : Powerup
{
    public float damageToAdd;

    public override void ApplyEffect(Powerup_Manager target)
    {
        //Remove Health Changes
        Pawn targetPawn = target.GetComponent<Pawn>();

        if (targetPawn)
        {
            TankShooter targetShooter = targetPawn.GetComponent<TankShooter>();
            if (targetShooter)
            {
                targetShooter.damage += damageToAdd;
            }
        }
    }

    public override void RemoveEffect(Powerup_Manager target)
    {
        //Remove Health Changes
        Pawn targetPawn = target.GetComponent<Pawn>();

        if (targetPawn)
        {
            TankShooter targetShooter = targetPawn.GetComponent<TankShooter>();
            if (targetShooter) {
                targetShooter.damage -= damageToAdd;
            }
        }
    }
}
