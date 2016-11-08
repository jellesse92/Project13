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
    protected bool cannotMovePlayer;
    protected bool cannotJump;
    protected bool cannotAttack;
    protected bool zeroVelocity;

    //For checking held buttons
    protected bool checkQuickAttackUp;
    protected bool quickAttackReleased;
    protected bool checkHeavyAttackUp;
    protected bool heavyAttackReleased;

    //Ground detection
    float distToGround;                                 //Distance from the ground
    int layerMask;                                      //Layers to check for ground

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
        cannotMovePlayer = false;
        cannotJump = false;
        cannotAttack = false;
        zeroVelocity = false;

        //Holding buttons
        checkQuickAttackUp = false;
        quickAttackReleased = true;
        checkHeavyAttackUp = false;
        heavyAttackReleased = true;

        //Checking for if held buttons released
        

        distToGround = GetComponent<Collider2D>().bounds.extents.y;
        layerMask = (LayerMask.GetMask("Default"));

        ClassSpecificStart();
    }

    protected void Update()
    {
        myPlayerInput.GetInput();
        myKeyPress = myPlayerInput.getKeyPress();
    }

    protected void FixedUpdate()
    {
        if (!GetComponent<PlayerProperties>().GetStunState())
        {
            Movement();
            if (myKeyPress.jumpPress)
                Jump();
            if (myKeyPress.quickAttackPress)
                QuickAttack();
            if (myKeyPress.heavyAttackPress)
                HeavyAttack();
            CheckForButtonReleases();
        }

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
        if (!cannotMovePlayer)
        {
            myRigidbody.velocity = new Vector2(myKeyPress.horizontalAxisValue * physicStats.movementSpeed, myRigidbody.velocity.y);
            myAnimator.SetFloat("speed", Mathf.Abs(myKeyPress.horizontalAxisValue));
            if (!isJumping)
                Flip();
        }
        if(zeroVelocity)
            myRigidbody.velocity = new Vector2(0, 0);
    }

    protected void Jump()
    {
        if (!isJumping && !cannotJump)
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

        if (isGrounded())
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
        if (!cannotAttack)
        {
            if (isJumping)
                myAnimator.SetTrigger("airQuickAttack");
            else
                myAnimator.SetTrigger("quickAttack");
        }
    }
    protected virtual void HeavyAttack()
    {
       
        if (!cannotAttack)
        {
            if (isJumping)
                myAnimator.SetTrigger("airHeavyAttack");
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

    protected void KnockBack(float knockBackForce)
    {
        knockBackForce *= isFacingRight ? -1 : 1;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(knockBackForce * 2, 0), ForceMode2D.Impulse);
    }

    //All function below is use for Animation Event
    public void ActivateMovement()
    {
        cannotMovePlayer = false;
    }

    public void DeactivateMovement()
    {
        cannotMovePlayer = true;
    }

    public void ActivateJump()
    {
        cannotJump = false;
    }

    public void DeactivateJump()
    {
        cannotJump = true;
    }

    public void ActivateAttack()
    {
        cannotAttack = false;
    }

    public void DeactivateAttack()
    {
        cannotAttack = true;
    }

    public void DeactivateVelocity()
    {
        zeroVelocity = true;
    }

    public void ActivateVelocity()
    {
        zeroVelocity = false;
    }
    
    public void VelocityY(float velocityY)
    {
        myRigidbody.velocity = new Vector2(0, velocityY);  
    }

    public void VelocityX(float velocityX)
    {
        myRigidbody.velocity = new Vector2(velocityX, 0);
    }

    public void AddForceX(float forceX)
    {
        myRigidbody.AddForce(new Vector2(forceX * transform.localScale.x, 0f));
    }

    public void AddForceY(float forceY)
    {
        myRigidbody.AddForce(new Vector2(0f, forceY));
    }

    public void ActivateAttackMovementJump()
    {
        cannotAttack = false;
        cannotJump = false;
        cannotMovePlayer = false;
    }

    public void DeactivateAttackMovementJump()
    {
        cannotAttack = true;
        cannotJump = true;
        cannotMovePlayer = true;
    }

    public bool isGrounded()
    {
        if (Physics2D.Raycast(transform.position, -Vector3.up, distToGround +.05f, layerMask))
        {
            return true;
        }
        return false;
    }

    protected void CheckForButtonReleases()
    {
        if (checkQuickAttackUp)
        {
            if (myKeyPress.heavyAttackReleased)
            {
                myAnimator.enabled = true;
                quickAttackReleased = true;
                checkQuickAttackUp = false;
            }
        }

        if (checkHeavyAttackUp)
        {
            if (myKeyPress.quickAttackReleased)
            {
                myAnimator.enabled = true;
                heavyAttackReleased = true;
                checkHeavyAttackUp = false;
            }
        }


    }

    public void CheckForQuickRelease()
    {
        quickAttackReleased = false;
        checkQuickAttackUp = true;
    }

    public void DisableAnimator()
    {

        if(!(quickAttackReleased && heavyAttackReleased))
            myAnimator.enabled = false;
    }

    public void CheckForHeavyRelease()
    {
        heavyAttackReleased = false;
        checkHeavyAttackUp = true;
    }
}
