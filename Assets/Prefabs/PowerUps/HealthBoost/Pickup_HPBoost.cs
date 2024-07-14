using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_HPBoost : Pickup
{
    public Powerup_HPBoost HP_Boost;    //set the HPBoost Effect

    public void OnTriggerEnter(Collider other)
    {
        //Does the gameObject entered have a powerup manager?
        Powerup_Manager powerupManager = other.GetComponent<Powerup_Manager>();
        
        //does the powerup manager exist?
        if (powerupManager) {
            powerupManager.Add(powerup); //Add the powerup to the object's pwerup manager
            Destroy(gameObject);         //Destroy itself
        }
    }
}
