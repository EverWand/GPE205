using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject PickupPrefab; //The Pickup prefab that is being spawned
    public float spawnDelay;        //The delay of the spawn time
    private float nextSpawnTime;    //Tracks the next spawn time once the delay is done
    private Transform tf;           //for setting the pickup transform

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time * spawnDelay; //sets the time for the delay

        tf = transform; //The transform of the Pickup
    }

    // Update is called once per frame
    void Update()
    {
        //is time to spawn pickup:
        if (Time.time > nextSpawnTime)
        {
            Instantiate(PickupPrefab, tf.position, Quaternion.identity); //spawn the pick up

            nextSpawnTime = Time.time + spawnDelay; //Add new delay time                                     
        }
        else
        {
            nextSpawnTime = Time.time + spawnDelay; //Object Exists, postpone spawn time
        }
    }
}
