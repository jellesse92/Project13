using UnityEngine;
using System.Collections;

public class BasicAlertEnemy : AlertEnemy {

    Animator anim;

    // Use this for initialization
    void Start()
    {
        health = 100;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetVisibleState() && GetPursuitState())
        {
            if (!GetAttackInRange())
            {
                transform.position = Vector2.MoveTowards(transform.position, GetTarget().transform.position, .01f);
                RunApproachAnim();
            }
            else
            {
                //Should begin attack animation
                anim.SetInteger("x", 0);
                anim.SetInteger("y", 0);
                anim.SetBool("isIdle", true);
            }
        }
        else
            anim.SetBool("isIdle", true);
    }

    //Checks what animation to play
    void RunApproachAnim()
    {
        int x = 0;
        int y = 0;

        if (GetTarget().transform.position.y > transform.position.y)
            y = 1;
        else if (GetTarget().transform.position.y < transform.position.y)
            y = -1;
        else if (GetTarget().transform.position.x > transform.position.x)
            x = 1;
        else if (GetTarget().transform.position.x < transform.position.x)
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
