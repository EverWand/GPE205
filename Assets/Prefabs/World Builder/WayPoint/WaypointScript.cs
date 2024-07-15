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
        GameManager.instance.wayPoints.Add(this); //add 
    }
    private void OnDestroy()
    {
        GameManager.instance.wayPoints.Remove(this);
    }
}
