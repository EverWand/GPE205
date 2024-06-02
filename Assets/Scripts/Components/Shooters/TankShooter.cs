using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    // Update is called once per frame
    void Update()
    {
        //Shoot a projectile if the shootkey ispressed and the tank isn't on cooldown
        if (Input.GetKeyDown(ShootKey) && canShoot()) {
            Shoot(); //Spawn and launched a projectile
        }
    }
}
