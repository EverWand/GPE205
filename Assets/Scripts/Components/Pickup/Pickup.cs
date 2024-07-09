using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Powerup powerup;

    public void OnTriggerEnter(Collider other)
    {
        //Store variable to object Powerup manager
        Powerup_Manager manager = other.GetComponent<Powerup_Manager>();
        
        //Power Up Manager Obtained:
        if (manager) 
        {
            manager.Add(powerup);   //Add Power Up to te Manager
            Destroy(gameObject);    //Destroy the Powerup Object
        } 
    }
}
