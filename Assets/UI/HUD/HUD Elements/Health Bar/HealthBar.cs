using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : ProgressBar_Script
{
    //Health Component this progess bar is attatched to
    public HealthSystem hp;

    //Updates the Health Bars Display
    public void updateDisplay() { 
        float healthPercent = hp.currHealth/hp.maxHealth;

        setPercent(healthPercent);
    }
}
