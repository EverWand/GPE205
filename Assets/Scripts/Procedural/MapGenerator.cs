using System;
using System.Collections.Generic;
using UnityEngine;
public class MapGenerator : MonoBehaviour
{
    private GameManager gameManager;
    //MAP VARIABLES
    //---Map Size
    public int rows;
    public int columns;
    //---Room Size
    public float roomWidth = 50f;
    public float roomHeight = 50f;

    //SEED
    public enum SeedTypes
    {
        Preset,
        Random,
        Daily,
        Time
    }
    public SeedTypes seedMode;
    public int seed;

    //MAP OBJECTS
    //---Map Grid
    public GameObject[] roomTiles;  //The types of Map Tiles the Map can generate
    private Room[,] grid;           //Matrix of the rooms

    //---Map Content
    //------Spawners
    //---------Pawns
    public List<PawnSpawner> pawnSpawns = new();             //List of all the Possible spawn locations
    //---------Pickups
    public List<PickupSpawner> pickupSpawns = new();  //List of all the Pick up Spawns
    public List<Pickup> Pickups = new();              //List of all Pickups
    //---WayPoints
    public List<WaypointScript> wayPoints = new();
    //------AI
    public float enemyDensity = .2f;    //The Percentage of how many spawners produce AI
    public List<AIController> listOfAIBehaviors = new();  //The Types of Behavior the Map can spawn
    public List<AIController> RequiredAI = new();        //The type of Behaviors the Map Has to spawn

    //------AI Types
    public GameObject AI_BasePrefab;
    public GameObject AI_PatrollerPrefab;
    public GameObject AI_AfraidPrefab;
    public GameObject AI_TurretPrefab;

    //------Pickups
    public float pickupDensity = .2f;

    //SCHEDULES
    private void Awake()
    {
        enemyDensity = Mathf.Clamp01(enemyDensity);     //keep the AI density between 0% - 100%
        pickupDensity = Mathf.Clamp01(pickupDensity);   //keep the Pickup density between 0% - 100%
    }

    private void Start()
    {
        gameManager = GameManager.instance;
    }
    //Returns a random room tile
    public GameObject RandomRoomPrefab()
    {
        return roomTiles[UnityEngine.Random.Range(0, roomTiles.Length)];
    }

    //Get the day and convert it into an integer
    public int DateToInt(DateTime date)
    {
        return date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
    }

    //Generates the Map
    public void GenerateMap()
    {
        //Set the Seed based on given Seed Type:
        switch (seedMode)
        {
            //PRESET SEED
            case SeedTypes.Preset:
                SetSeed(seed);
                break;
            //RANDOM SEED
            case SeedTypes.Random:
                SetSeed(UnityEngine.Random.Range(0, 9999));
                break;
            //THE DAILY SEED
            case SeedTypes.Daily:
                SetSeed(DateToInt(DateTime.Now.Date));
                break;
            //SEED BASED ON THE TIME
            case SeedTypes.Time:
                SetSeed(DateToInt(DateTime.Now));
                break;
        }

        grid = new Room[columns, rows];

        //ROW
        for (int currRow = 0; currRow < rows; currRow++)
        {
            //COLUMN
            for (int currCol = 0; currCol < columns; currCol++)
            {
                //Sets the offsets of the rooms for placement
                float xPos = roomWidth * currCol;
                float zPos = roomHeight * currRow;

                //Gets the position based on the offset
                Vector3 newPos = new(xPos, 0.0f, zPos);

                //Spawn a Random Room Prefab
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPos, Quaternion.identity) as GameObject;

                //
                tempRoomObj.transform.parent = this.transform;
                tempRoomObj.name = "Room" + currCol + ", " + currRow;

                //Get the Room Component
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                //OPENING DOORS
                //--Vertical Facing
                if (currRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currRow == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                //--Horizontal Facing
                if (currCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currCol == columns - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorWest.SetActive(false);
                    tempRoom.doorEast.SetActive(false);
                }


                grid[currCol, currRow] = tempRoom; // add the Room Prefab to the map grid
            }
        }

        //---Creating the Enemies
        GenerateEnemies();
        //---Enable the Spawners
        EnablePickUpSpawners();
    }
    //Destroys the Map if one has been created
    public void DestroyMap()
    {

        //====| CLEAN UP LOOSE GAMEOBJECT FROM MAP GENERATION |====

        //Destroy Room Objects if they exist
        if (grid != null && grid.Length > 0)
        {
            //go through every room in the map
            foreach (var room in grid)
            {
                Destroy(room?.gameObject); //DESTROY THE ROOM!
            }
        }

        //Destroy waypoints Objects if they exist
        if (wayPoints != null && wayPoints.Count > 0)
        {
            //go through every waypoint in the map
            foreach (var waypoint in wayPoints)
            {
                Destroy(waypoint?.gameObject); //DESTROY THE WAYPOINT!
            }
        }
        //Destroy Pick ups Objects if they exist
        if (Pickups != null && Pickups.Count > 0)
        {
            //go through every Pickup in the map
            foreach (var pickup in Pickups)
            {
                Destroy(pickup?.gameObject);  //DESTROY THE PICKUP!
            }
        }

        //Clean out the lists
        ClearAllLists();
    }

    //Clean out the lists
    private void ClearAllLists()
    {
        //Clear Map grid
        grid = null;
        //Clear Pickups
        Pickups.Clear();
        //Clear Pickup spawners
        pickupSpawns.Clear();
        //Clear Pawn spawners
        pawnSpawns.Clear();
        //Clear Waypoints
        wayPoints.Clear();
    }

    //sets the seed for the Map generator
    private void SetSeed(int newSeed)
    {
        UnityEngine.Random.InitState(newSeed);  //Form the seed based on the ID given
        seed = newSeed;                         //set that as the seed
    }

    //Returns a random Pawn Spawner point in the world
    public PawnSpawner getRandPawnSpawn()
    {
        int ranSpawnIndex = UnityEngine.Random.Range(0, pawnSpawns.Count);  //Gives a random intenger based on the amount of pawnSpawners are loaded

        return pawnSpawns[ranSpawnIndex];                       //Return the random spawner
    }
    //Enables certains spawns 
    private void EnablePickUpSpawners()
    {
        //get the amount of pick up spawners to activate
        int spawnsToEnable = (int)(pickupSpawns.Count * pickupDensity);

        for (int i = 0; i < spawnsToEnable; i++)
        {
            //Get a random spawn Index
            int ranSpawn = UnityEngine.Random.Range(0, pickupSpawns.Count);

            //while the random spawn is already active
            while (pickupSpawns[ranSpawn].isActive)
            {
                // find a new random spawner
                ranSpawn = UnityEngine.Random.Range(0, pickupSpawns.Count);
            }
            //set the spawner as active
            pickupSpawns[ranSpawn].SetActive(true);
        }
    }
    //--- SPAWNING AIs ---
    //Function that procedurally generates enemies
    private void GenerateEnemies()
    {
        int AISpawnAmount = (int)(pawnSpawns.Count * enemyDensity); //get's the amount of spawns should have enemy from the desity percentages

        //Spawn the needed amount of enemies
        for (int i = 0; i < AISpawnAmount; i++)
        {
            //Make sure we fill in the required enemies first
            if (i < RequiredAI.Count)
            {
                SpawnEnemyOfType(RequiredAI[i]); //Spawn the required enemy from the list.
            }
            else
            {
                int AI_ID = UnityEngine.Random.Range(0, listOfAIBehaviors.Count); //Get a random ID for AI controller the Map can generate
                SpawnEnemyOfType(listOfAIBehaviors[AI_ID]);           //Spawn that AI
            }
        }

        int missingAISpawns = AISpawnAmount - RequiredAI.Count;    //The amount of Required AIs missing after we spawn AIs

        //SPAWNING THE REST OF THE REQUIRED AIs:
        //Are there still required AIs needing spawned and is there's still space for new spawns?
        if (missingAISpawns > 0 && missingAISpawns <= pawnSpawns.Count - AISpawnAmount)
        {
            int listJump = RequiredAI.Count - missingAISpawns; //The jump we're making for compensation of the required AI's already spawned

            //Debug.Log("THE JUMP WE ARE TAKING!: " + listJump);  //DEBUG: Tracking the index we are jumping to in the required AI list.

            //spawning the required enemies starting from where the initial round of spawns left off.
            for (int i = listJump; i > RequiredAI.Count; i++)
            {
                SpawnEnemyOfType(RequiredAI[i]); //Spawn the Required Enemy in the list from after the list Jump.
            }
        }
    }
    //Specific AI Spawn
    private void SpawnEnemyOfType(AIController behaviorType)
    {
        PawnSpawner spawn = getRandPawnSpawn();
        switch (behaviorType)
        {

            //PATROL
            case Patrol_AITank:
                SpawnPatrolAI(spawn);
                break;
            //SCARED
            case Scared_AITank:
                SpawnScaredAI(spawn);
                break;
            //TURRET
            case Turret_AITank:
                SpawnTurretAI(spawn);
                break;
            default:    //unfamiliar or Base AI Controller : Make a Default AI
                SpawnBaseAI(spawn);
                break;
        }

        behaviorType.currWayPoint = spawn.gameObject; //set the Spawnpoint as the Ai's current waypoint
    }

    //==== AI SPAWNING ==== 
    //Basic AI Spawn
    public void SpawnBaseAI(PawnSpawner spawn)
    {
        GameObject pawn = Instantiate(AI_BasePrefab, spawn.transform) as GameObject; //Instantiates the Basic AI
    }
    //Patrolling AI Spawn
    public void SpawnPatrolAI(PawnSpawner spawn)
    {
        GameObject pawn = Instantiate(AI_PatrollerPrefab, spawn.transform) as GameObject; //Instantiates the Patroller AI
        AIController AI_Controller = pawn.GetComponent<AIController>();                   //Get AI Controller
        WaypointScript waypoint = spawn.GetComponent<WaypointScript>();                   //Get the waypoint of the Spawn

        //Set up AI_Controller's Waypoints:
        AI_Controller.wayPoints.Add(waypoint);
        AI_Controller.wayPoints.Add(waypoint.nextWaypoint);
        AI_Controller.wayPoints.Add(waypoint.nextWaypoint.nextWaypoint);
        AI_Controller.wayPoints.Add(waypoint.nextWaypoint.nextWaypoint.nextWaypoint);
    }
    //Afraid AI Spawn
    public void SpawnScaredAI(PawnSpawner spawn)
    {
        GameObject pawn = Instantiate(AI_AfraidPrefab, spawn.transform) as GameObject; //Instantiates the afraid AI
    }
    //Turret AI Spawn
    public void SpawnTurretAI(PawnSpawner spawn)
    {
        GameObject pawn = Instantiate(AI_TurretPrefab, spawn.transform) as GameObject; //Instantiates the Basic AI
    }
}
