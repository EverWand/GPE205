using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Health : Powerup
{
    public float healthToAdd;

    public override void ApplyEffect(Powerup_Manager target) { 
        //Apply Health Changed
        HealthSystem targetHP = target.GetComponent<HealthSystem>();
        if (targetHP) {
            targetHP.Heal(healthToAdd);
        }
    }
    public override void RemoveEffect(Powerup_Manager target) { 
        //Remove Health Changes
        HealthSystem targetHP = target.GetComponent<HealthSystem>();

        if (targetHP) {
            targetHP.TakeDamage(healthToAdd);
        }
    }
}
