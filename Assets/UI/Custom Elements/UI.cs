using UnityEngine;
using UnityEngine.Audio;


public class UI : MonoBehaviour
{
    public AudioSource OptionSFX;   //Sound source for the Option SFX

    //Plays the Options Sound
    public void PlayOptionSFX() 
    { 
        GameManager.instance.PlaySFX(OptionSFX);
    }
}
