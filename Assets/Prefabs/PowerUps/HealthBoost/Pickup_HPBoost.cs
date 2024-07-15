using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_HPBoost : Pickup
{
    public Powerup_HPBoost powerup;    //set the HPBoost Effect
    private void OnTriggerEnter(Collider other)
    {
        //Store variable to object Powerup manager
        Powerup_Manager manager = other.GetComponent<Powerup_Manager>();

        //Powerup Manager Obtained:
        if (manager)
        {
            manager.Add(powerup);   //Add Power Up to te Manager
            Destroy(gameObject);    //Destroy the Powerup Object
        }
    }
}