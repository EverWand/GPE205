using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    Camera P_Cam;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize references
        P_Cam = GetComponent<Camera>();             //Camera
        Pawn pawn = GetComponentInParent<Pawn>();

        player = pawn.controller as PlayerController;  //Player controller

        AdjustCameraSize();
    }


    void AdjustCameraSize()
    {
        int playerIndex = player.GetPlayerIndex();
        int playerAmount = GameManager.instance.numberOfPlayers;

        float viewportHeight = 1f / playerAmount;   // Each player gets 1/playerAmount of the screen height

        float x = 0;                                // Horizontal position remains 0
        float y = 1f - (playerIndex + 1) * viewportHeight;  // Vertical Position (flip order)

        // Set the viewport rectangle for this player's camera
        P_Cam.rect = new Rect(x, y, 1f, viewportHeight);

        // Debugging information
        Debug.Log($"Camera for Player {playerIndex + 1}: Position = ({x}, {y}), Size = (1f, {viewportHeight})");
    }
}