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
    public List<PlayerController> playerList = new List<PlayerController>();   //Controllers
    public List<PawnSpawner> pawnSpawns = new List<PawnSpawner>();  //List of all the PlayerSpawners

    //---AI Controllers
    public List<AIController> AIControllerList = new List<AIController>();
    //---WayPoints
    public List<WaypointScript> wayPoints = new List<WaypointScript>();

    //====| SCHEDULES |====
    public void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        //Generate the Map
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap();

        for (int id = 0; id <= numberOfPlayers; id++)
        {
            SpawnPlayer(); //Spawn the Player into the Scene
        }

        try //catch any errors when setting default target to all AI's
        {
            SetDefaultAITarget(playerCharacter); //set default AI Target to all AIs
        }
        catch { };
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

    public PawnSpawner getRandPawnSpawn()
    {
        Debug.Log("Spawners in World: " + pawnSpawns.Count);
        int ranSpawnIndex = Random.Range(0, pawnSpawns.Count);

        Debug.Log("Pawn Spawn Found: " + ranSpawnIndex);
        return pawnSpawns[ranSpawnIndex];
    }

    //Set the DeafaultAITargets to a specific target
    private void SetDefaultAITarget(GameObject target)
    {
        //For every AI Controller Found...
        foreach (AIController AI in AIControllerList)
        {
            //and if that AIController doesn't have a target Set...
            if (!AI.target)
            {
                //Set it to the specific target
                AI.target = target;
            }
        }
    }
}