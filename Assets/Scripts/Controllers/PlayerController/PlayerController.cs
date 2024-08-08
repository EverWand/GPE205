using UnityEngine;

public class PlayerController : Controller
{
    //MOVEMENT KEYS
    public KeyCode MoveForward;
    public KeyCode MoveBackward;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    //ACTION KEYS
    public KeyCode Primary;

    //COMPONENT REFERENCES
    public NoiseMaker noiseMaker; //Makes for AI to sense Noises

    //===|SCHEDULES|===
    private void Start()
    {
        Debug.Log("PLAYER CONTROLLER STARTING!!!");

        //set reference of pawn's noisemaker component
        noiseMaker = pawn.gameObject.GetComponent<NoiseMaker>();
    }
    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }
    //When the controller gets destroyed
    private void OnDestroy()
    {
        
        GameManager manager = GameManager.instance;

        manager.SetHighScore(score);

        //Remove player to the Game Managanger
        manager.playerList.Remove(this);

        //Remove self from the Target Lsit for AIs
        foreach (AIController ai in manager.AIControllerList)
        {
            ai.targetList.Remove(this.gameObject);
            ai.CleanupTargetList();
        }
        CheckGameOver();    //check if this causes a gameover
    }

    //===|OVERRIDES|===
    public override void ProcessInputs()
    {
        //============| MOVEMENT |============
        if (Input.GetKey(MoveForward))
        {
            pawn.MoveForward(pawn.BaseMovementSpeed);
            noiseMaker.MakeNoise(noiseMaker.movementVolume);
        }
        if (Input.GetKey(MoveBackward))
        {
            pawn.MoveBackwards(pawn.BaseMovementSpeed);
            noiseMaker.MakeNoise(noiseMaker.movementVolume);

        }
        if (Input.GetKey(TurnLeft))
        {
            pawn.TurnCounterClockwise(pawn.turnSpeed);
            noiseMaker.MakeNoise(noiseMaker.turnVolume);
        }
        if (Input.GetKey(TurnRight))
        {
            pawn.TurnClockwise(pawn.turnSpeed);
            noiseMaker.MakeNoise(noiseMaker.turnVolume);
        }

        //============| ACTIONS |============
        // - PRIMARY ABILITY
        if (Input.GetKeyDown(Primary))
        {
            //tank Shooting goes here
            pawn.Primary();
            noiseMaker.MakeNoise(noiseMaker.primaryFireVolume);
        }
    }
    public override void addToManager()
    {
        GameManager.instance.playerList.Add(this);
    }

    //===|FUNCTION|===
    //Gets the index that this controller is in
    public int GetPlayerIndex()
    {
        return GameManager.instance.playerList.IndexOf(this);
    }

    //Tries to activate the Gameover state
    private void CheckGameOver()
    {
        GameManager.instance.ActivateGameOverScreen();
    }
}