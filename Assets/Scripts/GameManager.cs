using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    //Player References
    public GameObject playerControllerPrefab;
    public GameObject playerPrefab;
    public Transform playertransform;
    private GameObject playerCharacter;

    //AIControllers
    private List<AIController> AIControllerList;

    public void Awake()
    {
        if (instance == null)
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
        SpawnPlayer(); //Spawn the Player into the Scene
        try //catch any errors when setting default target to all AI's
        {
            SetDefaultAITarget(playerCharacter); //set default AI Target to all AIs
        }
        catch { };
    }
    //instantiates the Player in the transform of the playerspawner
    private void SpawnPlayer()
    {
        //Player set-up References
        GameObject Controller = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; //Make a Player controller into the scene
        playerCharacter = Instantiate(playerPrefab, playertransform) as GameObject; //Make a Player Pawn into the scene
        playerCharacter.AddComponent<NoiseMaker>();

        Controller.GetComponent<Controller>().pawn = playerCharacter.GetComponent<Pawn>(); // Attatch the spawned player pawn to the spawned controller
    }

    //Set the DeafaultAITargets to a specific target
    private void SetDefaultAITarget(GameObject target)
    {
        AIControllerList = UpdateAIControllerList(); //Get List of AIControllers
        
        //For every AI Controller Found...
        foreach (AIController AI in AIControllerList)
        {
            //and if that AIController doesn't have a target Set...
            if (AI.target == null)
            {
                //Set it to the specific target
                AI.target = target;
            }
        }
    }

    //Updates the List of AIControllers
    public List<AIController> UpdateAIControllerList() 
    {
        AIController[] foundControllers = FindObjectsByType<AIController>(FindObjectsSortMode.None); //find all AIController instances and save them into an array
        return new List<AIController>(foundControllers);    //Return the Controller array as a list 
    }
}
