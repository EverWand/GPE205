using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ProgressBar_Script
{
    //Health Component this progress bar is attached to
    public HealthSystem hp;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (hp != null)
        {
            hp.OnUpdateHealth.AddListener(updateDisplay);
        }*/
    }

    // Updates the Health Bar's Display
    public void updateDisplay()
    {
        Debug.Log("UPDATING HEALTH DISPLAY!!!!!");
        float healthPercent = hp.currHealth / hp.maxHealth;
        setPercent(healthPercent);
    }
}