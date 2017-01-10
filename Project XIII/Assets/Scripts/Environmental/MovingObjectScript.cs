using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectScript : MonoBehaviour {

    public GameObject movingObject;                         //Object that is to be created and moved
    public Transform travelPoints;                          //Points which the object must travel between

    public float moveSpeed = 1f;                            //Speed which object moves between travel points
    public float moveStartDelay = 5f;                       //Delay before object starts moving again     

    int currentDestination = 1;                             //Destination point currently being traveled too
    bool moving = true;                 

	// Use this for initialization
	void Start () {
        movingObject = (GameObject)Instantiate(movingObject);
        movingObject.transform.SetParent(transform);
        if (travelPoints.childCount > 0)
            movingObject.transform.position = new Vector3(travelPoints.GetChild(0).position.x, travelPoints.GetChild(0).position.y, movingObject.transform.position.z);
        else
            movingObject.transform.position = transform.position;
        /*
        if (travelPoints.childCount > 0)
            movingObject.transform.position = new Vector3(travelPoints.GetChild(0).position.x, travelPoints.GetChild(0).position.y, movingObject.transform.position.z);
	    */
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    void MoveTowardsDestination()
    {

    }


}
