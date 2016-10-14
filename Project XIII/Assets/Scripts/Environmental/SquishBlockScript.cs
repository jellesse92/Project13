using UnityEngine;
using System.Collections;

public class SquishBlockScript : MonoBehaviour {

    Vector3 origin;
    Vector3 destination;

    public float moveSpeed;
    public float returnSpeed;
    public float returnDelay;

    //Determine if block is moving towards destination or returning to origin
    bool moving = false;
    bool returning = false;

	// Use this for initialization
	void Start () {
        origin = transform.position;

        //Testing. Remove so parent script may control
        TriggerMove();
	}

    void OnBecameVisible()
    {
        transform.position = origin;
    }

    void OnBecameInvisible()
    {
        transform.position = origin;
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
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed);
        if(transform.position == destination)
        {
            moving = false;
            StartCoroutine("DelayReturn");
        }
    }

    void ReturnToOrigin()
    {
        returning = true;
        transform.position = Vector2.MoveTowards(transform.position, origin, returnSpeed);
        if (transform.position == origin)
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
