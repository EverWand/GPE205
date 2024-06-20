using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //Player References
    public GameObject playerControllerPrefab;
    public GameObject playerPrefab;
    public Transform playertransform;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { 
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        SpawnPlayer();
    }
    public void SpawnPlayer() {
        //Player set-up References
        GameObject Controller = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; //Make a Player controller into the scene
        GameObject playerCharacter = Instantiate(playerPrefab, playertransform) as GameObject; //Make a Player Pawn into the scene
        playerCharacter.AddComponent<NoiseMaker>();

        Controller.GetComponent<Controller>().pawn = playerCharacter.GetComponent<Pawn>(); // Attatch the spawned player pawn to the spawned controller
    }
}
