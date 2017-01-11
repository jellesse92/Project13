using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysics : Enemy{

    public bool facingRight = true;                         //Determine direction facing

    public float attProjectionTime = 0f;                    //Determine how long before enemy should execute attack
    public bool hasAttackProjection = true;                 //Has an attack that can be projected
    public float knockBackForceX = 2000f;                   //Knockback Damage amnt for enemies
    public float knockBackForceY = 10f;

    protected bool canMove = true;
    protected bool canAttack = true;
    protected bool canJump = true;
    protected bool turning = false;


    //Juggling variables
    bool checkGrounded;                                     //Determines if enemy should check if grounded for when it's in a juggled state

    private void Start()
    {
        EnemySpecificStart();
    }

    // Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();

        if (isBouncing)
            ManageJuggleState();
        else if (!dead)
        {
            if (target != null && !target.GetComponent<PlayerProperties>().alive)
                target = GetRandomTarget();

            if (target != null && GetVisibleState() && GetPursuitState() && !stunned && !frozen && !turning)
                RunEngagedBehavior();
        }

        EnemySpecificUpdate();

    }

    public virtual void EnemySpecificUpdate()
    {
       
    }

    protected virtual void EnemySpecificStart()
    {

    }

    public virtual void ApproachTarget()
    {
        if (target.transform.position.x > transform.position.x)
        {
            if (!facingRight)
                Flip();
            anim.SetInteger("x", 1);
        }
        else if (target.transform.position.x < transform.position.x)
        {
            if (facingRight)
                Flip();
            anim.SetInteger("x", 1);
        }

        Vector2 target_location = new Vector2(target.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target_location, speed);
    }

    //Execute attack
    public virtual void ExecuteAttack()
    {
        anim.SetTrigger("Attack");
    }

    //Runs behavior of enemy engaged with player
    public virtual void RunEngagedBehavior()
    {

        if (!inAttackRange && canMove)
            ApproachTarget();
        else if (canAttack)
        {
            anim.SetInteger("x", 0);
            anim.SetTrigger("projectAttack");

            if (attProjectionTime == 0f || !hasAttackProjection)
                ExecuteAttack();
            else
                Invoke("ExecuteAttack", attProjectionTime);
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

        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounceForce));
        Invoke("DelayBounceCount", .2f);
    }

    void DelayBounceCount()
    {
        isSquishing = false;
    }

    //Flips the sprite
    public void Flip()
    {
        anim.SetTrigger("Turn");
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    public void DisableAttack()
    {
        canAttack = false;
    }

    public void EnableAttack()
    {
        canAttack = true;
    }

    public void DisableJump()
    {
        canJump = false;
    }

    public void EnableJump()
    {
        canJump = true;
    }

    public void DisableMove()
    {
        canMove = false;
    }

    public void EnableMove()
    {
        canMove = true;
    }

    public void DisableAttackJumpMove()
    {
        DisableAttack();
        DisableMove();
        DisableJump();
    }

    public void EnableAttackJumpMove()
    {
        EnableAttack();
        EnableMove();
        EnableJump();
    }

 
}

