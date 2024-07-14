using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup
{
    public float duration;      //Duration of the powerup
    public bool isPermanent;   //Boolean for if the powerup is Permanent

    public abstract void ApplyEffect(Powerup_Manager target);
    public abstract void RemoveEffect(Powerup_Manager target);
}
