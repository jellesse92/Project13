using UnityEngine;
using System.Collections;

public class BasicEnemy : Enemy {

	// Use this for initialization
	void Start () {
        fullHealth = 100;
        health = 100;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(isVisible && inPursuit)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, .03f);
        }
    }


}
