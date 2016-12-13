using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : Enemy {

    public float attProjectionTime = 0f;                    //Determine how long before enemy should execute attack
    bool isAttacking;                                       //Determine if enemy is in the middle of an attack animation to stop movement    
    bool attEnded;                                          //Determine if the attack ended
    bool gotAttackAnim;                                     //Determines if attack animation has been registered
    public bool facingRight = true;                         //Determine direction facing
    public float attCoolDownTime = 3;
    public float knockBackForceX = 2000f;                   //Knockback Damage amnt for enemies
    public float knockBackForceY = 3000f;
    private int location = 1;
    public bool isCoolingDown = false;
    Vector2 xyBoundary;
    GameObject leftCameraWall, rightCameraWall;

    //Juggling Variables
    bool checkGrounded;                                     //Determines if enemy should check if grounded for when it's in a juggled state

    // Use this for initialization
    void Start()
    {
        leftCameraWall = GameObject.FindGameObjectWithTag("Left Camera Wall");
        rightCameraWall = GameObject.FindGameObjectWithTag("Right Camera Wall");

        isAttacking = false;
        isInvincible = false;
        attEnded = true;
        gotAttackAnim = true;
        checkGrounded = true;
    }





    // Update is called once per frame
    public override void FixedUpdate()
    {
        xyBoundary[0] = leftCameraWall.transform.position.x + leftCameraWall.GetComponent<Collider2D>().bounds.size.x;
        xyBoundary[1] = rightCameraWall.transform.position.x - rightCameraWall.GetComponent<Collider2D>().bounds.size.x;
        if (isBouncing)
            ManageJuggleState();
        else if (!dead)
        {
            if (GetVisibleState() && GetPursuitState() && !stunned && !frozen)
            {
                if (isCoolingDown)
                {
                    anim.Play("Idle");
                }
                if (!isAttacking && !inAttackRange && !isCoolingDown)
                    Approach();
                else if (!attEnded)
                {

                    CheckAttackEnd();
                }
            }
            else if (!stunned)
                anim.SetInteger("x", 0);
        }


    }

    //Check when to next check if enemy bounced
    IEnumerator DelayBounceCheck()
    {
        checkGrounded = false;
        yield return new WaitForSeconds(.3f);
        checkGrounded = true;
    }

    //Manages juggling state
    void ManageJuggleState()
    {
        if (isSquishing)
            return;

        if (bounceCount >= ALLOWED_BOUNCES)
        {
            isBouncing = false;
        }
        else if (IsGrounded())
        {
            StartCoroutine("BounceSquish");
            bounceCount++;
        }
    }

    IEnumerator BounceSquish()
    {
        float delayTime = .01f;
        isSquishing = true;

        for (int i = 0; i < 3; i++)
        {
            transform.localScale += new Vector3(0, -.15f, 0f);
            yield return new WaitForSeconds(delayTime);
        }

        for (int i = 0; i < 3; i++)
        {
            transform.localScale += new Vector3(0, .15f, 0f);
            yield return new WaitForSeconds(delayTime);
        }

        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10000f));
        Invoke("DelayBounceCount", .2f);
    }

    void DelayBounceCount()
    {
        isSquishing = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag != "Player" && col.collider.tag != "Enemy" && col.collider.tag != "Ground")
        {
            StopCharging();
        }
    }

    private void StopCharging()
    {
        isCoolingDown = true;
        isInvincible = false;
        Invoke("UnsetCoolDown", attCoolDownTime);
    }
    //Plays out enemy approach
    void Approach()
    {
        if (!isCoolingDown)
        {
            Vector2 target_location = new Vector2(xyBoundary[location], transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target_location, speed);
            if (transform.position.x == xyBoundary[location])
            {
                StopCharging();
            }
            anim.SetTrigger("projectAttack");
        }

    }
    void UnsetCoolDown()
    {
        isCoolingDown = false;
        isInvincible = false;
        anim.SetInteger("x", 0);
        location = location == 1 ? 0 : 1;

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


    /*
     *   Melee Damage Control
     */

    //Applies damage to player
    void ApplyDamage()
    {
        transform.GetChild(1).GetComponent<EnemyMeleeDamage>().ApplyDamage();
    }

    //Resets list of players attacked during last attack
    void ResetDamageApply()
    {
        //transform.GetChild(1).GetComponent<EnemyMeleeDamage>().knockBackForceX = knockBackForceX;
        //transform.GetChild(1).GetComponent<EnemyMeleeDamage>().knockBackForceY = knockBackForceY;
        //transform.GetChild(1).GetComponent<EnemyMeleeDamage>().ResetAttackApplied();
    }
}
