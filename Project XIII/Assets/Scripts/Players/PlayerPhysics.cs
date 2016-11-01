using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {
    [HideInInspector]
    public Rigidbody2D myRigidbody;
    [HideInInspector]
    public Animator myAnimator;
    [HideInInspector]
    public PlayerPhysicStats physicStats;
    [HideInInspector]
    public PlayerBoostStats boostStats;
    [HideInInspector]
    public KeyPress myKeyPress;
    [HideInInspector]
    public PlayerInput myPlayerInput;
    [HideInInspector]
    public bool isJumping;
    [HideInInspector]
    public bool isFacingRight;

    public void Start () {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        physicStats = GetComponent<PlayerProperties>().GetPhysicStats();
        boostStats = GetComponent<PlayerProperties>().GetBoostStats();
        myPlayerInput = GetComponent<PlayerInput>();
        myKeyPress = myPlayerInput.getKeyPress();
        isFacingRight = true;
        isJumping = false;
        ClassSpecificStart();
    }

    public void Update()
    {
        myPlayerInput.GetInput();
        myKeyPress = myPlayerInput.getKeyPress();
    }

    public void FixedUpdate()
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

    public virtual void ClassSpecificStart()
    {
        //This function is used when a specific class need to use Start
    }

    public virtual void ClassSpecificUpdate()
    {
        //This function is used when a specific class need to use FixedUpdate
    }

    public void Movement()
    {
        if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidbody.velocity = new Vector2(myKeyPress.horizontalAxisValue * physicStats.movementSpeed, myRigidbody.velocity.y);
            myAnimator.SetFloat("speed", Mathf.Abs(myKeyPress.horizontalAxisValue));
            if (!isJumping)
                Flip();
        }
        else
        {
            myRigidbody.velocity = new Vector2(0, 0);
            isJumping = true;
        }
    }

    public void Jump()
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

    public void Landing()
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

    public virtual void QuickAttack()
    {
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (isJumping)
                myAnimator.SetTrigger("airQuickAttack");
            else
                myAnimator.SetTrigger("quickAttack");
        }
    }
    public virtual void HeavyAttack()
    {
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (isJumping)
                myAnimator.SetTrigger("airHeavyAttack");
            else
                myAnimator.SetTrigger("heavyAttack");
        }
    }

    public void Flip()
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
