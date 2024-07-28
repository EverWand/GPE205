using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PowerUp_AddScore : Powerup
{
    public int scoreToAdd;
    public override void ApplyEffect(Powerup_Manager target)
    {
        Controller targetController = target.GetComponent<Controller>();

        if (targetController)
        {
            targetController.score += scoreToAdd;
        }
    }


    public override void RemoveEffect(Powerup_Manager target)
    {
        Controller targetController = target.GetComponent<Controller>();

        if (targetController)
        {
            targetController.score -= scoreToAdd;
        }
    }
}