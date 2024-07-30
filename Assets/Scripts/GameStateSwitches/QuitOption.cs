using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOption : MonoBehaviour
{
   public void Quit() {
        GameManager.instance.QuitGame();     
   }
}
