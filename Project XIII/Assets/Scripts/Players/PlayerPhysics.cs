using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {
    protected Rigidbody2D myRigidbody;
    protected Animator myAnimator;
    protected PlayerProperties playerProperties;
    protected PlayerPhysicStats physicStats;
    protected PlayerBoostStats boostStats;
    protected KeyPress myKeyPress;
    protected PlayerInput myPlayerInput;
    protected bool isJumping;
    protected bool isFacingRight;
    protected bool cannotMove;
     
    protected void Start () {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        playerProperties = GetComponent<PlayerProperties>();
        physicStats = playerProperties.GetPhysicStats();
        boostStats = playerProperties.GetBoostStats();
        myPlayerInput = GetComponent<PlayerInput>();
        myKeyPress = myPlayerInput.getKeyPress();
        isFacingRight = true;
        isJumping = false;
        cannotMove = false;
        ClassSpecificStart();
    }

    protected void Update()
    {
        myPlayerInput.GetInput();
        myKeyPress = myPlayerInput.getKeyPress();
    }

    protected void FixedUpdate()
    {
        Movement();
        if (myKeyPress.jumpPress)
            Jump();
        if (myKeyPress.quickAttackPress)
            QuickAttack();
        if (myKeyPress.heavyAttackPress)
            HeavyAttack();
        Landing();

        ClassSpecificUpdate();
        myPlayerInput.ResetKeyPress();

    }
    protected void LateUpdate()
    {

    }

    public virtual void ClassSpecificStart()
    {
        //This function is used when a specific class need to use Start
    }

    public virtual void ClassSpecificUpdate()
    {
        //This function is used when a specific class need to use FixedUpdate
    }

    protected void Movement()
    {
        if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidbody.velocity = new Vector2(myKeyPress.horizontalAxisValue * physicStats.movementSpeed, myRigidbody.velocity.y);
            myAnimator.SetFloat("speed", Mathf.Abs(myKeyPress.horizontalAxisValue));
            cannotMove = false;
            if (!isJumping)
                Flip();
        }
        else if(cannotMove)
        {
            myRigidbody.velocity = new Vector2(0, -0.001f);
            isJumping = true;
        }
    }

    protected void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            if (Mathf.Abs(myKeyPress.horizontalAxisValue) > 0.1)
                myAnimator.SetTrigger("jumpForward");
            else
                myAnimator.SetTrigger("jumpIdle");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, physicStats.jumpForce), ForceMode2D.Impulse);
        }

    }

    protected void Landing()
    {
        if (myRigidbody.velocity.y == 0)
            isJumping = false;
        else
            isJumping = true;

        if (!isJumping)
        {
            myAnimator.SetTrigger("landing");
            myAnimator.SetBool("land", true);
        }
        else
            myAnimator.SetBool("land", false);
    }

    protected virtual void QuickAttack()
    {
        myRigidbody.velocity = new Vector2(0, 0);

        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (isJumping)
            {
                myAnimator.SetTrigger("airQuickAttack");
                cannotMove = true;
            }
            else
                myAnimator.SetTrigger("quickAttack");
        }
    }
    protected virtual void HeavyAttack()
    {
        myRigidbody.velocity = new Vector2(0, 0);

        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (isJumping)
            {
                myAnimator.SetTrigger("airHeavyAttack");
                cannotMove = true;
            }
            else
                myAnimator.SetTrigger("heavyAttack");
        }
    }

    protected void Flip()
    {
        if (myKeyPress.horizontalAxisValue > 0 && !isFacingRight || myKeyPress.horizontalAxisValue < 0 && isFacingRight)
        {
            myAnimator.SetTrigger("switch");
            isFacingRight = !isFacingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }

    protected void knockBack(float knockBackForce)
    {
        knockBackForce *= isFacingRight ? -1 : 1;
        Debug.Log(knockBackForce);

        GetComponent<Rigidbody2D>().AddForce(new Vector2(knockBackForce * 2, 0), ForceMode2D.Impulse);

    }
}
