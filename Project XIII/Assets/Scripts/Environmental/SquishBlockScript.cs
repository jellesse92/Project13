using UnityEngine;
using System.Collections;

public class SquishBlockScript : MonoBehaviour {

    //Heirarchy objects
    public Transform destination;               //Destination location
    public Transform block;                     //Moving block
    GameObject cam;                             //Game camera

    Vector3 origin;                             //Origin position of block to return to

    //Adjustable variables for balance
    public float moveSpeed;                     //Speed which block moves "down" towards destination
    public float returnSpeed;                   //Speed which block returns to origin location
    public float returnDelay;                   //Delay before initiating return

    //Determine if block is moving towards destination or returning to origin
    bool moving = false;
    bool returning = false;

    bool isVisible = false;

	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        origin = block.position;
	}

    void FixedUpdate()
    {
        if (moving)
            MoveTowardsDestination();
        else if (returning)
            ReturnToOrigin();
    }

    public void TriggerMove()
    {
        if(!returning && isVisible)
            moving = true;
    }

    void MoveTowardsDestination()
    {
        block.position = Vector2.MoveTowards(block.position, destination.position, moveSpeed);

        if (block.position.x == destination.position.x && block.position.y == destination.position.y)
        {
            moving = false;
            cam.GetComponent<CamShakeScript>().StartShake(.1f);
            Invoke("DelayReturn", returnDelay);
        }
    }

    void ReturnToOrigin()
    {
        returning = true;
        block.position = Vector2.MoveTowards(block.position, origin, returnSpeed);
        if (block.position == origin)
        {
            returning = false;
        }

    }

    void DelayReturn()
    {
        returning = true;
    }
	
    //Function to run when sprite is visible
    public void VisibleFunc()
    {
        this.GetComponent<SquishBlockScript>().enabled = true;
        block.position = origin;
        isVisible = true;
    }

    //Function to run when sprite is invis
    public void InvisFunc()
    {
        this.GetComponent<SquishBlockScript>().enabled = false;
        block.position = origin;
        moving = false;
        returning = false;
        isVisible = false;
    }

}
