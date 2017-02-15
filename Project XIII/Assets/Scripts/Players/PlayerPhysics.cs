using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {
    //use for shadow
    const float DISTANCE_CENTER_TO_FEET = 2.76f;
    //Variables for bad controller callibration
    float Y_NEGATIVE_ACCEPT = -.2f;
    float X_ABS_ACCEPT = .2f;

    //Use for crouching
    float previousVertical = 0;

    //Constants for changing gravity force for jumping
    const float DEFAULT_GRAVITY_FORCE = 8f;
    const float MIN_GRAVITY_FORCE = 4f;

    const float JUMP_CD = .1f;
    const float JUMP_GRACE_TIME = .3f;

    protected Rigidbody2D myRigidbody;
    protected Animator myAnimator;
    protected PlayerProperties playerProperties;
    protected PlayerStats physicStats;
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
    public float groundCheckingOffset = 2f;
    bool jumpedRecently = false;
    bool wasGrounded = false;
    bool jumpGraceTimeInvoked = false;
    bool jumped = false;
    bool jumpSpent = false;

    public GameObject shadow;                           //For placing shadow at character's feet
    public GameObject shadowSprite;                         

    Vector3 shadowScale;


    protected void Start () {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        playerProperties = GetComponent<PlayerProperties>();
        physicStats = playerProperties.GetPlayerStats();
        boostStats = playerProperties.GetBoostStats();
        myPlayerInput = GetComponent<PlayerInput>();
        myKeyPress = myPlayerInput.getKeyPress();
        isFacingRight = true;
        isJumping = false;
        cannotMovePlayer = false;
        cannotJump = false;
        cannotAttack = false;
        zeroVelocity = false;

        if(shadow)
            shadowScale = shadow.transform.localScale;
        //Holding buttons
        checkQuickAttackUp = false;
        quickAttackReleased = true;
        checkHeavyAttackUp = false;
        heavyAttackReleased = true;

        //Checking for if held buttons released
        

        distToGround = GetComponent<Collider2D>().bounds.extents.y;
        layerMask = (LayerMask.GetMask("Default","Item"));

        ClassSpecificStart();
    }

    protected void Update()
    {
        myPlayerInput.GetInput();
        myKeyPress = myPlayerInput.getKeyPress();
        
    }

    protected void FixedUpdate()
    {
        if (!GetComponent<PlayerProperties>().GetStunState() && GetComponent<PlayerProperties>().alive && GetComponent<PlayerInput>().InputActiveState())
        {
            if (!CheckClassSpecificInput())
            {
                float xMove = myKeyPress.horizontalAxisValue;
                float yMove = myKeyPress.verticalAxisValue;

                Movement();
                Crouching();

                //Prioritizes jump in the case both buttons pressed at the same time
                if (myKeyPress.jumpPress)
                    Jump();
                else
                {
                    if (myKeyPress.quickAttackPress)
                        QuickAttack();
                    if (myKeyPress.heavyAttackPress)
                        HeavyAttack();
                }
                if (myKeyPress.jumpReleased)
                    JumpReleased();

                if (myKeyPress.dashPress)
                    MovementSkill(xMove, yMove);
                /* Not all characters have a block move. disabled for now
                if (myKeyPress.blockPress)
                    Block();
                    */
                CheckForButtonReleases();
            }
        }
        ClassSpecificUpdate();
        myPlayerInput.ResetKeyPress();
        Landing();
        if (shadow)
            HandleShadow();
    }

    public virtual void ClassSpecificStart()
    {
        //This function is used when a specific class need to use Start
    }

    public virtual void ClassSpecificUpdate()
    {
        //This function is used when a specific class need to use FixedUpdate
    }

    public virtual bool CheckClassSpecificInput()
    {
        //This function is used when a specific class has specific inputs to look for
        return false;
    }

    public virtual void MovementSkill(float xMove, float yMove)
    {
        if (!cannotMovePlayer)
            return;
        //This function is used for when specific class movement based skills
    }

    protected void Movement()
    {
        if (!cannotMovePlayer && !myAnimator.GetBool("crouch"))
        {
                myRigidbody.velocity = new Vector2(myKeyPress.horizontalAxisValue * physicStats.movementSpeed, myRigidbody.velocity.y);
                myAnimator.SetFloat("speed", Mathf.Abs(myKeyPress.horizontalAxisValue));
                if (!isJumping)
                    Flip();
        }        
        if(zeroVelocity)
            myRigidbody.velocity = new Vector2(0, 0);
    }

    void Crouching()
    {
        if (myKeyPress.verticalAxisValue < previousVertical && myKeyPress.verticalAxisValue < 0 && myKeyPress.verticalAxisValue < Y_NEGATIVE_ACCEPT)
            myAnimator.SetBool("crouch", true);
        else if (myKeyPress.verticalAxisValue > previousVertical || myKeyPress.verticalAxisValue >= 0 && myKeyPress.verticalAxisValue >= Y_NEGATIVE_ACCEPT)
            myAnimator.SetBool("crouch", false);
        previousVertical = myKeyPress.verticalAxisValue;
    }

    protected void Jump()
    {
        if (!isJumping && !cannotJump && !jumpedRecently)
        {
            VelocityY(0);
            CancelInvoke("CancelWasGrounded");
            CancelWasGrounded();
            jumpedRecently = true;
            Invoke("CancelWaitJump", JUMP_CD);

            isJumping = true;
            GetComponent<Rigidbody2D>().gravityScale = MIN_GRAVITY_FORCE;

            if (Mathf.Abs(myKeyPress.horizontalAxisValue) > 0.1)
                myAnimator.SetTrigger("jumpForward");
            else
                myAnimator.SetTrigger("jumpIdle");

            //Debug.Log(GetComponent<Rigidbody2D>().velocity.y);
            
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, physicStats.jumpForce), ForceMode2D.Impulse);
        }
    }

    void CancelWaitJump()
    {
        jumpedRecently = false;
        jumped = true;
        jumpGraceTimeInvoked = false;
        CancelInvoke("CancelWasGrounded");
    }

    protected void JumpReleased()
    {
        GetComponent<Rigidbody2D>().gravityScale = DEFAULT_GRAVITY_FORCE;
    }

    protected void Landing()
    {
        if (isGrounded())
            isJumping = false;
        else
        {
            isJumping = true;
        }


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

            if (isJumping 
                || (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump Start Foward")
                || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump Idle"))
                || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump Start Idle"))
                myAnimator.SetTrigger("airQuickAttack");
            else
                myAnimator.SetTrigger("quickAttack");
        }
    }
    protected virtual void HeavyAttack()
    {       
        if (!cannotAttack)
        {
            if (isJumping 
                || (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump Start Foward")
                || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump Idle"))
                || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump Start Idle"))
                myAnimator.SetTrigger("airHeavyAttack");
            else
                myAnimator.SetTrigger("heavyAttack");
        }
    }
    protected virtual void Block()
    {
        if (!cannotAttack)
        {
            if (!isJumping)
            {
                myAnimator.SetTrigger("block");
            }
        }
    }

    protected void Flip()
    {
        if (myKeyPress.horizontalAxisValue > 0 && !isFacingRight || myKeyPress.horizontalAxisValue < 0 && isFacingRight)
        {
            ApplyFlip();
        }
    }

    public void SetFacing(bool inRightDirection)
    {
        if((inRightDirection && !isFacingRight) || (!inRightDirection && isFacingRight))
        {
            ApplyFlip();
        }     
    }   

    void ApplyFlip()
    {
        if (myAnimator == null)
            myAnimator = GetComponent<Animator>();
        myAnimator.SetTrigger("switch");
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
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

    public bool CanAttackStatus()
    {
        return !cannotAttack;
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
        if (Physics2D.Raycast(transform.position, -Vector3.up, distToGround +groundCheckingOffset, layerMask))
        {
            wasGrounded = true;
            jumped = false;
            return true;
        }
        else if (!jumpGraceTimeInvoked)
        {
            jumpGraceTimeInvoked = true;
            Invoke("CancelWasGrounded", JUMP_GRACE_TIME);
        }

        if (wasGrounded && !jumped)
            return true;

        return false;
    }

    void HandleShadow()
    {        
        float scaleChange;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 30, layerMask);
        if (hit && hit.distance > DISTANCE_CENTER_TO_FEET)
        {            
            scaleChange = (1/Mathf.Clamp(Mathf.Log(hit.distance - DISTANCE_CENTER_TO_FEET), 1, 30));            
            shadow.transform.position = hit.point;
            shadow.transform.localScale = new Vector3(shadowScale.x * scaleChange, shadowScale.y * scaleChange, shadowScale.z);
        }

        shadowSprite.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

        if (shadowSprite && hit.distance > DISTANCE_CENTER_TO_FEET)
        {
            Vector3 newPosition = shadowSprite.transform.position;
            newPosition.y = transform.position.y - (hit.distance - DISTANCE_CENTER_TO_FEET)*2 - 5.6f;
            shadowSprite.transform.position = newPosition;
        }
        
    }

    void CancelWasGrounded()
    {
        wasGrounded = false;
        jumpGraceTimeInvoked = false;
    }

    public void CancelWasGroundedInvoke()
    {
        CancelInvoke("CancelWasGrounded");
        CancelWasGrounded();
    }
    
    protected void CheckForButtonReleases()
    {
        if (checkQuickAttackUp)
        {
            if (myKeyPress.quickAttackReleased)
            {
                myAnimator.enabled = true;
                quickAttackReleased = true;
                checkQuickAttackUp = false;
            }
        }

        if (checkHeavyAttackUp)
        {
            if (myKeyPress.heavyAttackReleased)
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

    //Specific to freezing animations connected to whether a button has been released or not
    public void DisableAnimator()
    {
        if(!(quickAttackReleased && heavyAttackReleased) && GetComponent<PlayerInput>().InputActiveState())
            myAnimator.enabled = false;
    }

    void TotalDisableAnimator()
    {
        myAnimator.enabled = false;
    }

    public void CheckForHeavyRelease()
    {
        heavyAttackReleased = false;
        checkHeavyAttackUp = true;
    }

    public void ConstrainY()
    {
        myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void DeconstrainY()
    {
        myRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }

    public void ConstrainX()
    {
        myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void DeconstrainX()
    {
        myRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
    }

    public void AlterGravityForce(float force)
    {
        GetComponent<Rigidbody2D>().gravityScale = force;
    }

    public void RevertOriginalGravityForce()
    {
        GetComponent<Rigidbody2D>().gravityScale = DEFAULT_GRAVITY_FORCE;
    }

    public float GetDefaultGravityForce()
    {
        return DEFAULT_GRAVITY_FORCE;
    }

}
