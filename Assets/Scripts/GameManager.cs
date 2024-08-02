using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //====| VARIABLES |====
    public static GameManager instance;
    public MapGenerator mapGenerator;

    //Player References
    public int PLAYER_MAX;  //The Maximum amount of players
    [HideInInspector] public int numberOfPlayers = 1; //Decides how many players should be spawned {hidden from editor view}

    public GameObject player1ControllerPrefab;
    public GameObject player2ControllerPrefab;
    public GameObject playerPrefab;
    public Transform playertransform;
    private GameObject playerCharacter;

    //Game States
    public GameObject TitleScreenObject;
    public GameObject OptionsObject;
    public GameObject CreditsObject;
    public GameObject GameplayObject;
    public GameObject GameOverObject;

    //====| GameObject Lists |====
    //---Players
    public List<PlayerController> playerList = new();   //Controllers

    //---AI
    //------Controllers
    public List<AIController> AIControllerList = new();


    //====| SCHEDULES |====
    private void Awake()
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
    private void Start()
    {
        ActivateTitleScreen();  //Start on the Title Screen
    }

    //Function for starting the Game
    private void StartGame()
    {
        if(mapGenerator != null) { 
            //Generate the Level
            mapGenerator.GenerateMap();

            //for every players there are meant to be:
            for (int id = 0; id < numberOfPlayers; id++)
            {
                SpawnPlayer(id); //Spawn the Player into the Scene
            }

            SetDefaultAITarget(playerCharacter.gameObject); //set default AI Target to all AIs
        }
    }
    //Quits the Game
    public void QuitGame()
    {
        //RUN THIS IF A UNITY APPLICATION IS BEING RAN
#if UNITY_STANDALONE
        Application.Quit(); //Quit the Application
#endif

        //RUN THIS IF BEING RAN IN THE EDITOR
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    //Exits Editor playtime
#endif
    }

    //==== GAME STATES ==== 
    //Deactivates all Game States
    private void DeactivateAllGameStates()
    {
        TitleScreenObject.SetActive(false);
       
        OptionsObject.SetActive(false);
        CreditsObject.SetActive(false);
        GameplayObject.SetActive(false);
        GameOverObject.SetActive(false);
    }

    //---TITLE
    public void ActivateTitleScreen()
    {
        DeactivateAllGameStates();          //reset all current game states
        TitleScreenObject.SetActive(true);  //Activate Title Screen
    }
    //---OPTIONS
    public void ActivateOptions()
    {
        DeactivateAllGameStates();          //reset all current game states
        OptionsObject.SetActive(true);  //Activate Title Screen
    }
    //---CREDITS
    public void ActivateCredits()
    {
        DeactivateAllGameStates();          //reset all current game states
        CreditsObject.SetActive(true);  //Activate Title Screen
    }
    //---GAMEPLAY
    public void ActivateGameplay()
    {
        DeactivateAllGameStates();  //reset all current game states
        StartGame();                //Start the Game        
    }
    //---GAME OVER
    public void ActivateGameOverScreen()
    {
        DeactivateAllGameStates();          //reset all current game states
        GameOverObject.SetActive(true);  //Activate Title Screen
    }

    //====| FUNCTIONS |====
    //instantiates the Player in the transform of the playerspawner
    private void SpawnPlayer(int id)
    {

        //Player set-up References
        GameObject controller;
        //Assign Player 1
        if (id < 1)
        {
            controller = Instantiate(player1ControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        }
        //Assign Player 2
        else { controller = Instantiate(player2ControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; }

        //Make a Player controller into the scene
        playerCharacter = Instantiate(playerPrefab, mapGenerator.getRandPawnSpawn().transform) as GameObject; //Make a Player Pawn into the scene


        controller.GetComponent<Controller>().pawn = playerCharacter.GetComponent<Pawn>(); // Attach the spawned player pawn to the spawned controller
        playerCharacter.GetComponent<Pawn>().controller = controller.GetComponent<Controller>(); // Attach the player controller to the spawned pawn
    }

    //Set the DeafaultAITargets to a specific target
    private void SetDefaultAITarget(GameObject target)
    {
        //For every AI Controller Found...
        foreach (AIController AI in AIControllerList)
        {
            //DEBUG: Debug.Log("Testing target of AI: " + AI.gameObject);
            //and if that AIController doesn't have a target Set...
            if (!AI.target)
            {
                //Set it to the specific target
                AI.target = target;
            }
        }
    }
}