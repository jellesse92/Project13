using UnityEngine;
using System.Collections;

public class BasicEnemy : Enemy {

    Animator anim;

	// Use this for initialization
	void Start () {
        fullHealth = 100;
        health = 100;


        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isVisible && inPursuit)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, .03f);
            AnimationCheck();
        }
        else
            anim.SetBool("isIdle",true);
    }

    //Checks what animation to play
    void AnimationCheck()
    {
        int x = 0;
        int y = 0;

        if (target.transform.position.y > transform.position.y)
            y = 1;
        else if (target.transform.position.y < transform.position.y)
            y = -1;
        else if (target.transform.position.x > transform.position.x)
            x = 1;
        else if (target.transform.position.x < transform.position.x)
            x = -1;

        if (y == 0 && x == 0)
        {
            anim.SetBool("isIdle", true);
        }
        else
        {
            anim.SetBool("isIdle", false);
        }

        anim.SetInteger("x", x);
        anim.SetInteger("y", y);

    }

}
