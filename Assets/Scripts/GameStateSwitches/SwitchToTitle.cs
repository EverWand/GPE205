using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToTitle : MonoBehaviour
{
   public void TranstionToTitle() {
        GameManager.instance.ActivateTitleScreen();     
   }
}
