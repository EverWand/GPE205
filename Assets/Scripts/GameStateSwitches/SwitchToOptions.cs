using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToOptions : MonoBehaviour
{
    public void TranstionToOptions()
    {
        GameManager.instance.ActivateOptions();
    }
}
