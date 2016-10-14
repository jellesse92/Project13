using UnityEngine;
using System.Collections;

public class SquishBlockScript : MonoBehaviour {
    public Transform destination;
    public Transform block;

    Vector3 origin;

    public float moveSpeed;
    public float returnSpeed;
    public float returnDelay;

    //Determine if block is moving towards destination or returning to origin
    bool moving = false;
    bool returning = false;

	// Use this for initialization
	void Start () {
        origin = block.position;

        //Testing. Remove so parent script may control
        TriggerMove();
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
