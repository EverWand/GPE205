using System.Collections.Generic;
using UnityEngine;

public class Powerup_Manager : MonoBehaviour
{
    public AudioSource pickupSFX;
    public List<Powerup> powerupList;
    public List<Powerup> removedPowerupList;

    private void Start()
    {
        powerupList = new();
        removedPowerupList = new();
    }

    private void Update()
    {
        //Count down powerup timers
        DecrementPowerUpTimer();
    }

    private void LateUpdate()
    {
        //Apply powerup effects
        ApplyRemovalPowerUpQueue();
    }
    //Add a new Powerup
    public void Add(Powerup powerup)
    {
        //Pick up has been picked up, Play Pickup Sound!
        GameManager.instance.PlaySFX(pickupSFX);

        //POWERUP ADD TO MANAGER
        powerup.ApplyEffect(this);
        powerupList.Add(powerup);
    }
    //Remove a powerup
    public void Remove(Powerup powerup)
    {
        if (powerup != null)
        {
            powerup.RemoveEffect(this); //Remove effect of the power up
            removedPowerupList.Add(powerup);    //puts in the removal queue
        }
    }
    //Counts down the timer of each powerup collected
    public void DecrementPowerUpTimer()
    {
        //are there power ups?
        if (powerupList != null && powerupList.Count > 0)
        {
            //decrease each power up timer
            foreach (Powerup powerup in powerupList)
            {
                //found a powerup reference?
                if (powerup != null)
                {
                    powerup.duration -= Time.deltaTime; //Lose time on the powerup's timer

                    //Is the time depleted and it's not a permanent effect?
                    if (powerup.duration <= 0 && !powerup.isPermanent)
                    {
                        Remove(powerup);    //remove the power up
                    }
                }
            }
        }
    }

    private void ApplyRemovalPowerUpQueue()
    {
        foreach (Powerup powerup in removedPowerupList)
        {
            //found a powerup reference?
            if (powerup != null)
            {
                powerupList.Remove(powerup);
            }
        }
        //Is there powerups in the removal queue?
        if (removedPowerupList != null && removedPowerupList.Count > 0)
        {
            removedPowerupList.Clear(); //Clear the Powerups
        }
    }
}

