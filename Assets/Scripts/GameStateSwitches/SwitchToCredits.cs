using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToCredits : MonoBehaviour
{
    public void TranstionToCredits()
    {
        GameManager.instance.ActivateCredits();
    }
}
