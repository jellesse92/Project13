using UnityEngine;
using System.Collections;

public class SquishBlockScript : MonoBehaviour {

    //Heirarchy objects
    public Transform destination;               //Destination location
    public Transform block;                     //Moving block
    //GameObject camera;                   //Game camera

    Vector3 origin;                             //Origin position of block to return to

    //Adjustable variables for balance
    public float moveSpeed;                     //Speed which block moves "down" towards destination
    public float returnSpeed;                   //Speed which block returns to origin location
    public float returnDelay;                   //Delay before initiating return

    //Determine if block is moving towards destination or returning to origin
    bool moving = false;
    bool returning = false;

	// Use this for initialization
	void Start () {
        //camera = GameObject.FindGameObjectWithTag("MainCamera");
        origin = block.position;
	}

    void OnBecameVisible()
    {
        block.position = origin;
    }

    void OnBecameInvisible()
    {
        block.position = origin;
        moving = false;
        returning = false;
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
        if(!returning)
            moving = true;
    }

    void MoveTowardsDestination()
    {
        block.position = Vector2.MoveTowards(block.position, destination.position, moveSpeed);

        if (block.position.x == destination.position.x && block.position.y == destination.position.y)
        {
            moving = false;
            //camera.GetComponent<CamShakeScript>().StartShake(.005f);
            StartCoroutine("DelayReturn");
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

    IEnumerator DelayReturn()
    {
        yield return new WaitForSeconds(returnDelay);
        returning = true;
    }
	


}
