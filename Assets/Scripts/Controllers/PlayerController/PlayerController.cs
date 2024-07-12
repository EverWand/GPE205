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

    public NoiseMaker noiseMaker; //Makes for AI to sense Noises

    // Start is called before the first frame update
    void Start()
    {
        //add player to the Game Managanger
        GameManager.instance.playerList.Add(this);

        //set reference of pawn's noisemaker component
        noiseMaker = pawn.gameObject.GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void OnDestroy()
    {
        //add player to the Game Managanger
        GameManager.instance.playerList.Remove(this);
    }

    public override void ProcessInputs()
    {
        //============| MOVEMENT |============
        if (Input.GetKey(MoveForward))
        {
            pawn.MoveForward(pawn.movementSpeed);
            noiseMaker.MakeNoise(noiseMaker.movementVolume);
        }
        if (Input.GetKey(MoveBackward))
        {
            pawn.MoveBackwards(pawn.movementSpeed);
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
}