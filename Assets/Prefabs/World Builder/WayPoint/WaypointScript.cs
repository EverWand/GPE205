using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public WaypointScript nextWaypoint; //Saves the next Waypoint this one leads to

    public float posThreshold = 1;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.mapGenerator.wayPoints.Add(this); //add 
    }
    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.mapGenerator.wayPoints.Remove(this);
        }
    }
}
