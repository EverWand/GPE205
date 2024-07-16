using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_DmgUp : Pickup
{
    public Powerup_DmgBoost powerup;    //set the powerup Effect
    private void OnTriggerEnter(Collider other)
    {
        //Store variable to object Powerup manager
        Powerup_Manager manager = other.GetComponent<Powerup_Manager>();
        PickedUp.Invoke();

        //Powerup Manager Obtained:
        if (manager)
        {
            manager.Add(powerup);   //Add Power Up to te Manager
            Destroy(gameObject);    //Destroy the Powerup Object
        }
    }
}
