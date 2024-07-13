using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup
{
    public float duration;
    public bool isPermantent;

    public abstract void ApplyEffect(Powerup_Manager target);
    public abstract void RemoveEffect(Powerup_Manager target);
}
