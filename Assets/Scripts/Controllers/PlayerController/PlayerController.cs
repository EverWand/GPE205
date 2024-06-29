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

    private NoiseMaker noiseMaker; //Makes for AI to sense Noises
    public float noiseDistance = 10;
    //Structure of Volumes used for different noises the player makes
    public struct noiseVolumeStruct
    {
        public float movementVolume;
        public float turnVolume;
        public float primaryFireVolume;
    } 
    public noiseVolumeStruct noiseVolumes;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //add player to the Game Managanger
        GameManager.instance.PlayersList.Add(this);

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
        GameManager.instance.PlayersList.Remove(this);
    }
    public override void ProcessInputs()
    {
        //============| MOVEMENT |============
        if (Input.GetKey(MoveForward))
        {
            pawn.MoveForward();
            //Is there a noise maker?
            if (noiseMaker != null)
            {
                //Is there a noise maker?
                if (noiseMaker != null)
                {
                    noiseMaker.volumeDistance = noiseDistance; //make movement sound
                }
                else
                {
                    noiseMaker.volumeDistance = 0;  // no movement sound
                }
            }
        }
        if (Input.GetKey(MoveBackward))
        {
            pawn.MoveBackwards();
            //Is there a noise maker?
            if (noiseMaker != null)
            {
                noiseMaker.volumeDistance = noiseDistance; //make movement sound
            }
            else
            {
                noiseMaker.volumeDistance = 0;  // no movement sound
            }
        }
        if (Input.GetKey(TurnLeft))
        {
            pawn.TurnCounterClockwise(pawn.turnSpeed);
        }
        if (Input.GetKey(TurnRight))
        {
            pawn.TurnClockwise(pawn.turnSpeed);
        }

        //============| ACTIONS |============
            // - PRIMARY ABILITY
        if (Input.GetKeyDown(Primary))
        {
            //tank Shooting goes here
            pawn.Primary();
            MakeNoise(noiseVolumes.movementVolume);
        }
    }

    private void MakeNoise(float noiseVolume) { 
        noiseDistance = noiseVolume;
    }
}
