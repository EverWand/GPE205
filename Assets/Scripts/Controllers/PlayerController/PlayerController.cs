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

    // Start is called before the first frame update
    void Start()
    {
        //set reference of pawn's noisemaker component
        noiseMaker = pawn.gameObject.GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
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
            pawn.TurnCounterClockwise();
        }
        if (Input.GetKey(TurnRight))
        {
            pawn.TurnClockwise();
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
