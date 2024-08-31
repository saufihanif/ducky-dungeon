using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    //MOVE THE PLATFORM FRAME BY FRAME

    [SerializeField] private GameObject[] waypoints;    //REQUEST - Create an array
    [SerializeField] private float speed = 2f;  // speed move the platform

    private int currentWaypointIndex = 0;   //First index for the first waypoint

    private void Update()
    {
        if(Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)  //If object distance and location = 0f
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) //to know we at last waypoint
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }
}
