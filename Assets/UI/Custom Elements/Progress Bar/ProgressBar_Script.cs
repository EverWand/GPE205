using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        slider.value = percent;

        ProgressBar_Updated?.Invoke();
    }
}
