using UnityEngine;
using System.Collections;

public class BasicEnemy2D : Enemy {


    Animator anim;

    public float attProjectionTime = 0f;                    //Determine how long before enemy should execute attack
    bool isAttacking;                                       //Determine if enemy is in the middle of an attack animation to stop movement    
    bool attEnded;                                          //Determine if the attack ended
    bool gotAttackAnim;                                     //Determines if attack animation has been registered

    bool facingRight = true;                                //Determine direction facing

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        isAttacking = false;
        attEnded = true;
        gotAttackAnim = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetVisibleState() && GetPursuitState())
        {
            if (!isAttacking && !GetAttackInRange())
                Approach();
            else if(!isAttacking && attEnded)
            {
                anim.SetInteger("x", 0);
                anim.SetTrigger("projectAttack");
                isAttacking = true;

                if (attProjectionTime == 0f)
                    ExecuteAttack();
                else
                    Invoke("ExecuteAttack", attProjectionTime);
            }
            else if(!attEnded)
            {
                CheckAttackEnd();
            }
        }
        else
            anim.SetInteger("x", 0);

    }

    //Plays out enemy approach
    void Approach()
    {

        if (GetTarget().transform.position.x > transform.position.x)
        {
            if (!facingRight)
                Flip();
            anim.SetInteger("x", 1);
        }
        else if (GetTarget().transform.position.x < transform.position.x)
        {
            if (facingRight)
                Flip();
            anim.SetInteger("x", 1);
        }

        Vector2 target_location = new Vector2(GetTarget().transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target_location, speed);

    }

    //Flips the sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    //Execute attack
    void ExecuteAttack()
    {
        anim.SetTrigger("attack");
        attEnded = false;
        gotAttackAnim = false;
    }

    //Determines when done attacking
    void CheckAttackEnd()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            gotAttackAnim = true;
        }

        if (gotAttackAnim && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            isAttacking = false;
            attEnded = true;
        }

    }
}
