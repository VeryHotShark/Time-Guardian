using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTutorial : Tutorial {

    public Transform[] waypoints;
    public GameObject indicator;

    int currentWaypointIndex = -1;
    Vector3 currentWaypoint;

    public BoxCollider triggerCollider;

    public override void Execute()
    {   
        MoveToNextWaypoint();
        indicator.gameObject.GetComponent<Animator>().enabled = false;
        indicator.gameObject.SetActive(true);
    }

    void MoveToNextWaypoint()
    {
        currentWaypointIndex++;
        Debug.Log(currentWaypointIndex);

        if (currentWaypointIndex == 3)
            base.ChangeText("try to stay on the platform, otherwise you will die.");

        if (currentWaypointIndex == waypoints.Length)
        {
            indicator.SetActive(false); 
            base.TutorialCompleted();
            gameObject.SetActive(false);
            return;
        }


        currentWaypoint = waypoints[currentWaypointIndex].position;
        transform.position = currentWaypoint;
        ActivateSpotLight();
    }

    // Update is called once per frame
    void  OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            MoveToNextWaypoint();
	}

    void ActivateSpotLight()
    {
        indicator.transform.position = currentWaypoint + Vector3.up *2f;
    }
}
