using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProgressBar_Script : MonoBehaviour
{
    //References
    public Slider slider;
    
    //====| EVENTS |====
    public UnityEvent ProgressBar_Updated;

    // Sets the percent of the progress bar
    public void setPercent(float percent) 
    {
        percent = Mathf.Clamp01(percent); // Clamps between 0%-100%
        
        Debug.Log($"Setting slider value to: {percent}");

        slider.normalizedValue = percent;
        
        ProgressBar_Updated?.Invoke();
    }
}
