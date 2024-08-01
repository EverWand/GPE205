using System;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    Camera camera;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize references
        camera = GetComponent<Camera>();             //Camera
        Pawn pawn = GetComponentInParent<Pawn>();

        player = pawn.controller as PlayerController;  //Player controller

        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        int playerIndex = player.GetPlayerIndex();
        int playerAmount = GameManager.instance.numberOfPlayers;

        float viewportHeight = 1f / playerAmount;   // Each player gets 1/playerAmount of the screen width

        //Debug.Log("height of Player " + playerIndex + "is " + viewportHeight);


        float x = 0;                                // Horizontal position based on player's index
        float y = playerIndex * viewportHeight;     // Vertical Position   

        //Debug.Log("y-pos of Player " + playerIndex + "cam is " + y);

        // Set the viewport rectangle for this player's camera
        camera.rect = new Rect(x, y, 1f, viewportHeight);
    }
}