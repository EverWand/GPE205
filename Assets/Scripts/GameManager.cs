using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    //====| VARIABLES |====
    public static GameManager instance;
    //---SOUND SETTINGS
    public float volumeBGM = 1.0f;
    public float volumeSFX = 1.0f;

    public MapGenerator mapGenerator;

    [HideInInspector] public int highScore = 0; //Saves the Game's Highscore
    public event Action On_HighScore_Change;

    //---PLAYER
    //Player Settings
    public int PLAYER_MAX;  //The Maximum amount of players
    [HideInInspector] public int numberOfPlayers = 1; //Decides how many players should be spawned {hidden from editor view}
    //Player References
    public GameObject player1ControllerPrefab;
    public GameObject player2ControllerPrefab;
    public GameObject playerPrefab;
    public Transform playertransform;
    private GameObject playerCharacter;

    //---GAME STATES
    public GameObject TitleScreenObject;
    public GameObject OptionsObject;
    public GameObject CreditsObject;
    //Gameplay state
    public GameObject GameplayObject;
    public event Action On_Game_Start;
    public GameObject GameOverObject;

    //====| GameObject Lists |====
    //---Players
    public List<PlayerController> playerList = new();   //Controllers

    //---AI
    //------Controllers
    public List<AIController> AIControllerList = new();
    //------AI Default Targets
    public List<GameObject> DefaultAITargets = new();

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

    private void Update()
    {
        SetHighScore(100);
    }
    //====| FUNCTIONS |====

    //Function for starting the Game
    private void StartGame()
    {
        Debug.Log("============| GAME START STARTING |============");

        if (mapGenerator != null)
        {
            //MAKE SURE WE HAVE AN EMPTY SCENE TO MAKE A NEW MAP
            mapGenerator.DestroyMap();

            //Generate the Level
            mapGenerator.GenerateMap();

            if (playerPrefab != null)
            {
                //for every players there are meant to be:
                for (int id = 0; id < numberOfPlayers; id++)
                {
                    SpawnPlayer(id); //Spawn the Player into the Scene
                }
            }

            foreach (PlayerController player in playerList)
            {
                DefaultAITargets.Add(player.pawn.gameObject);  //add the player object to the list
                //Debug.Log(player.pawn.gameObject);
            }


            //INVOKE EVENTS
            On_Game_Start?.Invoke();
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

    //instantiates the Players
    private void SpawnPlayer(int id)
    {
        Debug.Log("---Spawning Player: " + id);
        //Player set-up References
        GameObject controller;
        //Assign Player 1
        if (id == 0)
        {
            controller = Instantiate(player1ControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        }
        //Assign Player 2
        else { controller = Instantiate(player2ControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; }


        //CONSTRUCT PLAYER:
        playerCharacter = Instantiate(playerPrefab, mapGenerator.getRandPawnSpawn().transform) as GameObject; //Make a Player Prefab into the scene

        controller.GetComponent<Controller>().pawn = playerCharacter.GetComponent<Pawn>(); // Attach the spawned player pawn to the spawned controller
        playerCharacter.GetComponent<Pawn>().controller = controller.GetComponent<Controller>(); // Attach the player controller to the spawned pawn

        Debug.Log("---FINISHED: Spawning Player: " + id + "|| PAWN = " + playerList[id].pawn.name + "/ GAME OBJECT = " + playerList[id].pawn.gameObject);
    }

    // Set the Default AI Targets for all AI Controllers
    /* private void SetDefaultAITargets()
     {
         Debug.Log("SETTING DEFAULT TARGETS");

         foreach (GameObject player in DefaultAITargets)
         {
             Debug.Log("PLAYER " + (DefaultAITargets.IndexOf(player) + 1) + ": " + player.name);
         }

         AIController debugAiController = null;

             //For Every AI that exists in the Game
         foreach (AIController ai in AIControllerList)
         {
             if (ai.targetList == null)
             {
                     ai.targetList = new List<GameObject>();
             }

             ai.targetList.AddRange(DefaultAITargets);
             debugAiController = ai;
         }

         //DEBUG : CHECKING WHAT GAME OBJECTS ARE NOW INSIDE OF TARGETLIST OF AI
         foreach (GameObject newObject in debugAiController.targetList) 
         {
             Debug.Log("NEW TARGET FOR " + debugAiController.gameObject.name + ": " + newObject.name);
         }
     }
    */

    //Checks if the given score is higher than the current highscore
    public void SetHighScore(int score)
    {
        //is the score being tested greater than the current highscore?
        if (score > highScore)
        {
            highScore = score;            //set the new highscore
            On_HighScore_Change.Invoke(); //Signal the highscore has changed
        }
    }

    //==== GAME STATES ==== 
    //Deactivates all Game States
    private void DeactivateAllGameStates()
    {
        TitleScreenObject?.SetActive(false);

        OptionsObject?.SetActive(false);
        CreditsObject?.SetActive(false);
        GameplayObject?.SetActive(false);
        GameOverObject?.SetActive(false);
    }

    //---TITLE
    public void ActivateTitleScreen()
    {
        DeactivateAllGameStates();          //reset all current game states
        TitleScreenObject?.SetActive(true);  //Activate Title Screen
    }
    //---OPTIONS
    public void ActivateOptions()
    {
        DeactivateAllGameStates();          //reset all current game states
        OptionsObject?.SetActive(true);  //Activate Title Screen
    }
    //---CREDITS
    public void ActivateCredits()
    {
        DeactivateAllGameStates();          //reset all current game states
        CreditsObject?.SetActive(true);  //Activate Title Screen
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
        //If all players are dead
        if (playerList == null || playerList.Count <= 0)
        {
            DeactivateAllGameStates();       //reset all current game states
            GameOverObject?.SetActive(true);  //Activate Title Screen
        }
    }
}