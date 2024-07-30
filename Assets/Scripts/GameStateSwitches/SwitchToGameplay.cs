using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToGameplay : MonoBehaviour
{
    public void TranstionToGameplay()
    {
        GameManager.instance.ActivateGameplay();
    }
}
