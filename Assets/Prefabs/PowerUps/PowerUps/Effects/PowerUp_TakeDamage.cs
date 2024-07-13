using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUp_TakeDamage : Powerup
{
    public float DamageDealt;

    public override void ApplyEffect(Powerup_Manager target)
    {
        //Apply Health Changed
        HealthSystem targetHP = target.GetComponent<HealthSystem>();
        if (targetHP)
        {
            targetHP.TakeDamage(DamageDealt);
        }
    }
    public override void RemoveEffect(Powerup_Manager target)
    {
        
    }
}
