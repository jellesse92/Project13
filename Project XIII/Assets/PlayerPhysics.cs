using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private PlayerPhysicStats physicStats;
    private KeyPress myKeyPress;
    private PlayerInput myPlayerInput;

    private bool isJumping;
    private bool isFacingRight;
    
    void Start () {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        physicStats = GetComponent<PlayerProperties>().GetPhysicStats();
        myPlayerInput = GetComponent<PlayerInput>();
        myKeyPress = myPlayerInput.getKeyPress();
        isFacingRight = true;
        isJumping = false;
    }

    void Update()
    {
        myKeyPress = myPlayerInput.getKeyPress();
    }

    void FixedUpdate()
    {
        Movement();
        if (myKeyPress.jumpPress)
            Jump();
        if (myKeyPress.quickAttackPress)
            QuickAttack();
        if (myKeyPress.heavyAttackPress)
            HeavyAttack();
        Landing();
    }

    void Movement()
    {
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidbody.velocity = new Vector2(myKeyPress.horizontalAxisValue * physicStats.movementSpeed, myRigidbody.velocity.y);
            myAnimator.SetFloat("speed", Mathf.Abs(myKeyPress.horizontalAxisValue));
            if (!isJumping)
                Flip();
        }
        else
        {
            myRigidbody.velocity = new Vector2(0, -0.01f);
        }
    }

    void Jump()
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

    void Landing()
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

    void QuickAttack()
    {
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (isJumping)
                myAnimator.SetTrigger("airQuickAttack");
            else
                myAnimator.SetTrigger("quickAttack");
        }
    }
    void HeavyAttack()
    {
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (isJumping)
                myAnimator.SetTrigger("airHeavyAttack");
            else
                myAnimator.SetTrigger("heavyAttack");
        }
    }

    void Flip()
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
}
