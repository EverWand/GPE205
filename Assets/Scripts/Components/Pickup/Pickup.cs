using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public UnityEvent PickedUp;

    //ADD AND REMOVE PICKUPS IN THE WORLD INTO THE GAME MANAGER
    private void Awake()
    {
        GameManager.instance.mapGenerator.Pickups.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.instance.mapGenerator.Pickups.Remove(this);
    }
}