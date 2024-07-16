using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //====| VARIABLES |====
    public static GameManager instance;
    public static MapGenerator mapGenerator;

    //Player References
    public int numberOfPlayers = 1; //decides how many players there should be
    public GameObject playerControllerPrefab;
    public GameObject playerPrefab;
    public Transform playertransform;
    private GameObject playerCharacter;

    //====| GameObject Lists |====
    //---Players
    public List<PlayerController> playerList = new();   //Controllers
    public List<PawnSpawner> pawnSpawns = new();             //List of all the Possible spawn locations for the 
    //---Pickups
    public List<PickupSpawner> pickupSpawns = new();  //List of all the Pick up Spawns
    public List<Pickup> Pickups = new();
    //---AI
    //------Controllers
    public List<AIController> AIControllerList = new();
    //------AI Types
    public GameObject AI_BasePrefab;
    public GameObject AI_PatrollerPrefab;
    public GameObject AI_AfraidPrefab;
    public GameObject AI_TurretPrefab;

    //---WayPoints
    public List<WaypointScript> wayPoints = new();

    //====| SCHEDULES |====
    public void Awake()
    {
        //Game Manager Persistent Instance Set Up:
        //If there's no instance made yet
        if (!instance)
        {
            instance = this; //set this as the instance
            DontDestroyOnLoad(gameObject);  //make sure this game Manager does not get destroyed in between scene loads
        }
        else //Theres a game instance already:
        {
            Destroy(gameObject); //Delete the new game instance
        }
    }
    public void Start()
    {
        //Generate the Level
        //---Creating the Map
        mapGenerator = GetComponent<MapGenerator>();    //set the map generator
        mapGenerator.GenerateMap();                     //Make the Map
        //---Creating the Enemies
        GenerateEnemies();
        //---Enable the Spawners
        EnablePickUpSpawners();

        //for every players there are meant to be:
        for (int id = 0; id < numberOfPlayers; id++)
        {
            SpawnPlayer(); //Spawn the Player into the Scene
        }
        SetDefaultAITarget(playerCharacter.gameObject); //set default AI Target to all AIs
    }

    //====| FUNCTIONS |====
    //instantiates the Player in the transform of the playerspawner
    private void SpawnPlayer()
    {
        //Player set-up References
        GameObject Controller = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; //Make a Player controller into the scene
        playerCharacter = Instantiate(playerPrefab, getRandPawnSpawn().transform) as GameObject; //Make a Player Pawn into the scene

        Controller.GetComponent<Controller>().pawn = playerCharacter.GetComponent<Pawn>(); // Attach the spawned player pawn to the spawned controller
    }

    //Function that procedurally generates enemies
    private void GenerateEnemies()
    {
        int AISpawnAmount = (int)(pawnSpawns.Count * mapGenerator.enemyDensity); //get's the amount of spawns should have enemy from the desity percentages

        //Spawn the needed amount of enemies
        for(int i = 0; i < AISpawnAmount; i++)
        {
            //Make sure we fill in the required enemies first
            if (i < mapGenerator.RequiredAI.Count)
            {
                SpawnEnemyOfType(mapGenerator.RequiredAI[i]); //Spawn the required enemy from the list.
            }
            else {
                int AI_ID = Random.Range(0, mapGenerator.listOfAIBehaviors.Count); //Get a random ID for AI controller the Map can generate
                SpawnEnemyOfType(mapGenerator.listOfAIBehaviors[AI_ID]);           //Spawn that AI
            }
        }
        
        int missingAISpawns = AISpawnAmount - mapGenerator.RequiredAI.Count;    //The amount of Required AIs missing after we spawn AIs

        //SPAWNING THE REST OF THE REQUIRED AIs:
        //Are there still required AIs needing spawned and is there's still space for new spawns?
        if (missingAISpawns > 0 && missingAISpawns <= pawnSpawns.Count - AISpawnAmount) { 
            int listJump = mapGenerator.RequiredAI.Count - missingAISpawns; //The jump we're making for compensation of the required AI's already spawned

            Debug.Log("THE JUMP WE ARE TAKING!: " + listJump);  //DEBUG: Tracking the index we are jumping to in the required AI list.

            //spawning the required enemies starting from where the initial round of spawns left off.
            for(int i = listJump; i > mapGenerator.RequiredAI.Count; i++) {
                SpawnEnemyOfType(mapGenerator.RequiredAI[i]); //Spawn the Required Enemy in the list from after the list Jump.
            }
        }
    }
    //Enables certains spawns 
    private void EnablePickUpSpawners() 
    {
        //get the amount of pick up spawners to activate
        int spawnsToEnable = (int)(pickupSpawns.Count * mapGenerator.pickupDensity);    
        
        for(int i = 0; i < spawnsToEnable; i++) 
        {
            //Get a random spawn Index
            int ranSpawn = Random.Range(0, pickupSpawns.Count);
            
            //while the random spawn is already active
            while (pickupSpawns[ranSpawn].isActive) {
                // find a new random spawner
                ranSpawn = Random.Range(0, pickupSpawns.Count); 
            }
            //set the spawner as active
            pickupSpawns[ranSpawn].SetActive(true);
        }
    }
    //--- SPAWNING AIs ---
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
    
    //Returns a random Pawn Spawner point in the world
    public PawnSpawner getRandPawnSpawn()
    {
        int ranSpawnIndex = Random.Range(0, pawnSpawns.Count);  //Gives a random intenger based on the amount of pawnSpawners are loaded

        return pawnSpawns[ranSpawnIndex];                       //Return the random spawner
    }

    //Set the DeafaultAITargets to a specific target
    private void SetDefaultAITarget(GameObject target)
    {
        //For every AI Controller Found...
        foreach (AIController AI in AIControllerList)
        {
            Debug.Log("Testing target of AI: " + AI.gameObject);
            //and if that AIController doesn't have a target Set...
            if (!AI.target)
            {
                //Set it to the specific target
                AI.target = target;
            }
        }
    }
}