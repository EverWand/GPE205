using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject PickupPrefab;
    public float spawnDelay;

    private float nextSpawnTime;
    private Transform tf;
    private GameObject spawnedPickup;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time * spawnDelay;

        tf = transform;
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
