using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Powerup_Manager : MonoBehaviour
{
    public List<Powerup> powerupList;
    public List<Powerup> removedPowerupList;


    private void Start()
    {
        powerupList = new();
        removedPowerupList = new();
    }

    private void Update()
    {
        DecrementPowerUpTimer();
    }

    private void LateUpdate()
    {
        ApplyRemovalPowerUpQueue();
    }
    public void Add(Powerup powerup)
    {
        //POWERUP ADD TO MANAGER
        powerup.ApplyEffect(this);
        powerupList.Add(powerup);
    }
    //Remove a powerup
    public void Remove(Powerup powerup) 
    {
        powerup.RemoveEffect(this); //Remove effect of the power up
        removedPowerupList.Add(powerup);    //puts in the removal queue
    }

    public void DecrementPowerUpTimer()
    {
        if (powerupList.Count > 0) 
        {
            //decrease each power up timer
            foreach (Powerup powerup in powerupList)
            {
                powerup.duration -= Time.deltaTime;

                if (powerup.duration <= 0 && !powerup.isPermanent)
                {
                    Remove(powerup);
                }
            }
        }
    }

    private void ApplyRemovalPowerUpQueue()
    {
        foreach(Powerup powerup in removedPowerupList)
        {
            powerupList.Remove(powerup);
        }

        removedPowerupList.Clear();
    }
}

