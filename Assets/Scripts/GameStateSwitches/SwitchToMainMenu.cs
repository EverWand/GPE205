using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToMainMenu : MonoBehaviour
{
    public void TranstionToMainMenu()
    {
        GameManager.instance.ActivateMainMenuScreen();
    }
}
