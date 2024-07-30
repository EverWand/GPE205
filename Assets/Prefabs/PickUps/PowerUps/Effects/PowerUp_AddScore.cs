using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PowerUp_AddScore : Powerup
{
    public int scoreToAdd;
    public override void ApplyEffect(Powerup_Manager target)
    {
        Controller targetController = target.GetComponent<Pawn>().controller;
        Console.WriteLine("SCORE PICKUP OBTAINED BY: " + targetController.name + " FOR " + scoreToAdd);
        if (targetController)
        {
            targetController.AddToScore(scoreToAdd);
        }
    }


    public override void RemoveEffect(Powerup_Manager target)
    {
        Controller targetController = target.GetComponent<Pawn>().controller;

        if (targetController)
        {
            targetController.RemoveFromScore(scoreToAdd);
        }
    }
}