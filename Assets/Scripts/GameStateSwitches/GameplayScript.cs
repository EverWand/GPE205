using UnityEngine;

public class GameplayScript : MonoBehaviour
{
    public AudioSource BGM;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.On_Game_Start += PlayBGM;  //Subscribe BGM to when starting the game
    }

    private void PlayBGM()
    {
        //set the volume
        BGM.volume = GameManager.instance.volumeBGM;
        //Play the BGM
        BGM.Play();
    }
}
