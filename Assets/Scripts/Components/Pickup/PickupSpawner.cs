using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public bool isActive = false;   //Checks if this Spawner is active
    public GameObject PickupPrefab; //The Pickup prefab that is being spawned
    public float spawnDelay;        //The delay of the spawn time
    private float nextSpawnTime;    //Tracks the next spawn time once the delay is done
    private Transform tf;           //for setting the pickup transform
    private GameObject spawnedPickUp;


    //====SCHEDULES
    //ADD|REMOVE from Game manager
    private void Awake()
    {
        GameManager.instance.pickupSpawns.Add(this);
    }
    private void OnDestroy()
    {
        GameManager.instance.pickupSpawns.Remove(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        tf = transform; //The transform of the Pickup
    }

    // Update is called once per frame
    void Update()
    {
        //Is there Spawner Ready to produce a new Pickup? [Spawntime reached & is active]
        if (isActive && Time.time > nextSpawnTime)
        {
            //spawn the pick up if there's not one out yet
            if (spawnedPickUp == null)
            {
                spawnedPickUp = Instantiate(PickupPrefab, tf.position, Quaternion.identity) as GameObject;
            }
            nextSpawnTime = Time.time + spawnDelay; //Add new delay time
        }
    }

    public void SetActive(bool enabled)
    {
        isActive = enabled;

        nextSpawnTime = Time.time + spawnDelay; //sets the time for the delay

        tf = transform; //The transform of the Pickup
    }
}
