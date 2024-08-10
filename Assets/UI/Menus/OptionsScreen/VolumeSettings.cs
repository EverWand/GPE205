using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public Slider sliderBGM;
    public Slider sliderSFX;

    public void SetBGMVolume()
    {
        GameManager.instance.volumeBGM = sliderBGM.value;
    }
    public void SetSFXVolume()
    {
        GameManager.instance.volumeSFX = sliderSFX.value;
    }
}
