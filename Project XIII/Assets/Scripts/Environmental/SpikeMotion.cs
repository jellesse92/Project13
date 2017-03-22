using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMotion : MonoBehaviour {

    public float speed;
    public float riseFactor;
    public float waitTime;
    public float moveTime;
    public float timer;
    public bool waiting = true;
    public bool falling = false;
    public bool rising = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (waiting)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                waiting = false;
                falling = true;
                timer = 0;
            }
        }
        else if (falling)
        {
            timer += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, transform.position.y - speed);
            if (timer >= moveTime)
            {
                falling = false;
                rising = true;
                timer = 0;
            }
        }
        else if (rising)
        {
            //this isn't the correct way to do this D:
            timer += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, transform.position.y + speed*riseFactor);
            if (timer >= moveTime/riseFactor)
            {
                rising = false;
                waiting = true;
                timer = 0;
            }
        }
	}
}
