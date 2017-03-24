using UnityEngine;
using System.Collections;

public class SwordsmanPhysics : PlayerPhysics{

    //SENSITVITY CONTROLS
    const float Y_INPUT_THRESHOLD = .5f;        //Threshold before considering input
    const float INPUT_SOFT_THRESHOLD = .1f;

    //Constants for managing quick dashing skill
    const float DASH_RECOVERY_TIME = 0.5f;      //Time it takes to recover dashes
    const float MAX_CHAIN_DASH = 1;             //Max amount of dashes that can be chained
    const float STOP_AFTER_IMAGE = .005f;       //Time to stop creating afterimages
    const float DASH_FORCE = 5000f;             //Amount of force to apply on character to perform movement

    //Constant for charging ground heavy slash attack
    const float MAX_CHARGE = 1.8f;                //Max amount of time multiplier allowed to be applied to charge distance
    const float TIER_1_CHARGE = .6f;            //Tier one charge for beginning to flash white
    const float TIER_2_FORCE_MOD = 1f;          //Amount to multiply charge for tier 2 charge
    const float TIER_1_FORCE_MOD = .8f;         //Amount to multiply charge for tier 1 charge
    const float TIER_0_FORCE_MOD = .4f;         //Amount to multiply charge for basic charge
    const float CHARGE_FORCE_MULTIPLIER = 1800f;//Multiplier for distance to travel after charging attack

    //Constant for max combo hit types
    const int MAX_COMBO = 3;                    //Maximum combo hit types
    const float COMBO_GRACE_TIME = 0.4f;        //Time after finishing a combo hit's animation before the next combo hit no longer continues the chain

    

    //Attack box
    public GameObject attackBox;                //Collider for dealing all melee attacks
    public SwordsmanAttackScript attackScript;  //Script for managing attack

    //Dash variables
    float xInputAxis = 0f;                               
    float yInputAxis = 0f;
    int dashCount = 0;                          //Checks how many dashes have been chained
    bool checkGroundForDash = false;            //Bool that determines to check for grounded before resetting dash count
    bool disableDash = false;
    float myXScale = 0f;                        //Gets x scale for sprites not appropriately sized


    //Combo Variable
    bool inCombo = false;                       //Checks if swordsman able to combo
    bool comboPressed = false;                  //Check if the combo button was pressed during combo
    bool checkForCombo = false;                 //Check if script should check for next combo press input
    bool comboAnimFinished = false;             //Checks if the combo animation was finished but still should check for combo input
    int currentCombo = 0;                       //Checks what combo hit was last played

    //Heavy attack variables
    bool checkChargeTime = false;               //Determines if should check for time charging
    float timeCharged;                          //Time attack has been charged
    int chargeLevel = 0;                        //Level of charge
    Material defaultMat;                        //Default sprite material
    public Material flashWhiteMat;              //White flashing material
    public Material flashGoldMat;               //Gold flashing material
    bool isFlashingWhite = false;               //Determines if flashing white color
    bool isFlashingGold = false;                //Determines if flashing gold color

    public float magHeavyAttackScreenShake = 1.5f;
    public float durHeavyAttackScreenShake = 0.7f;

    SwordsmanParticleEffects playerParticleEffects;
    SwordsmanSoundEffects playerSoundEffects;

    PlayerEffectsManager playerEffectsManager;

    public override void ClassSpecificStart()
    {
        playerParticleEffects = GetComponent<SwordsmanParticleEffects>();
        playerSoundEffects = GetComponent<SwordsmanSoundEffects>();

        playerEffectsManager = transform.parent.GetComponent<PlayerEffectsManager>();
        if(GetComponent<SpriteRenderer>()!= null)
            defaultMat = GetComponent<SpriteRenderer>().material;

        myXScale = transform.localScale.x;

    }

    public override void ClassSpecificUpdate()
    {
        if (!GetComponent<PlayerProperties>().alive)
        {
            if (checkChargeTime)
            {
                CancelHeavyCharge();
                myAnimator.enabled = true;
            }
            return;
        }

        if (inCombo)
            WatchForCombo();
        if (checkGroundForDash)
            ResetDashCount();
        if (checkChargeTime)
            ManageChargeEffect();

    }

    public override bool CheckClassSpecificInput()
    {
        float xMove = myKeyPress.horizontalAxisValue;
        float yMove = myKeyPress.verticalAxisValue;

        //Up attack input
        if (CanAttackStatus() && (yMove > Y_INPUT_THRESHOLD) && GetComponent<PlayerInput>().getKeyPress().quickAttackPress && isGrounded())
        {
            GetComponent<Animator>().SetTrigger("upQuickAttack");
            return true;
        }

        if(CanAttackStatus() && GetComponent<PlayerInput>().getKeyPress().quickAttackPress && checkForCombo)
        {
            if (comboAnimFinished)
            {
                CancelInvoke("StopCheckForCombo");
                PlayNextComboHit();
            }
            return true;
        }

        if (CanAttackStatus() && GetComponent<PlayerInput>().getKeyPress().heavyAttackPress)
        {
            StartHeavyGroundCharge();
            CheckForHeavyRelease();
            return true;
        }

        return false;
    }

    public override void ExecuteHeavyButtonRelease()
    {
        myAnimator.SetTrigger("heavyAttack");
    }

    public override void MovementSkill(float xMove, float yMove)
    {
        if (disableDash)
            return;
        base.MovementSkill(xMove,yMove);

        if (Mathf.Abs(xMove) < .01 && Mathf.Abs(yMove) < .01f)
        {
            float xDir = 1f;
            if (transform.localScale.x < 0f)
                xDir = -1f;
            xInputAxis = xDir;
            yInputAxis = 0f;
        }
        else
        {
            xInputAxis = xMove;
            yInputAxis = yMove;
        }

        if(dashCount < MAX_CHAIN_DASH)
            GetComponent<Animator>().SetTrigger("moveSkill");
    }

    //COMBO FUNCTIONS

    public void WatchForCombo()
    {
        if (GetComponent<PlayerInput>().getKeyPress().quickAttackPress)
        {
            comboPressed = true;
        }
    }

    public void StartCombo()
    {
        inCombo = true;
        checkForCombo = true;
        comboAnimFinished = false;
    }

    public void ResetCombo()
    {
        inCombo = false;
        comboAnimFinished = false;
        checkForCombo = false;
        CancelInvoke("StopCheckForCombo");
        currentCombo = 0;
    } 

    public void FinishCombo()
    {
        inCombo = false;
        currentCombo++;
        comboAnimFinished = true;

        if (currentCombo < MAX_COMBO)
        {
            if (comboPressed)
                PlayNextComboHit();
            else
                Invoke("StopCheckForCombo", COMBO_GRACE_TIME);
        }
        else
            StopCheckForCombo();
        
    }

    void PlayNextComboHit()
    {
        if (!GetComponent<PlayerProperties>().alive)
            return;

        string triggerString = "combo" + currentCombo.ToString();
        comboPressed = false;
        comboAnimFinished = false;

        if (isGrounded())
            GetComponent<Animator>().SetTrigger(triggerString);
        else
            GetComponent<Animator>().SetTrigger("air" + triggerString);
    }

    void StopCheckForCombo()
    {
        checkForCombo = false;
        currentCombo = 0;
        comboAnimFinished = false;
    }

    //END COMBO FUNCTIONS

    public void HeavyTransistionToAir()
    {
        GetComponent<Animator>().SetTrigger("heavyToAerial");
    }

    //UP + QUICK ATTACK ATTACK FUNCTIONS

    public void ExecuteDragAttack()
    {
        attackScript.Reset();
        attackScript.SetAttackType("drag");
        MoveSkillExecuted();
    }

    public void EndDragAttack()
    {
        attackScript.DragAttackEnd();
        attackBox.GetComponent<Collider2D>().enabled = false;
        attackScript.ResetDrag();
    }

    public void EndDragDamage()
    {
        attackScript.CancelDragAttackApplyDamage();
    }

    //END UP + QUICK ATTACK ATTACK FUNCTIONS

    //DASHING FUNCTIONS

    public void ExecuteDashSkill()
    {
        dashCount++;
        CancelInvoke("StopAfterImage");
        CancelInvoke("ResetDashCount");

        StartCoroutine("Dashing");

        playerParticleEffects.PlayDashAfterImage(true);
        playerParticleEffects.PlayDashTrail(true);
        gameObject.layer = 14;
    }

    IEnumerator Dashing()
    {
        DeactivateAttackMovementJump();
        VelocityY(0f);
        VelocityX(0f);
        movementSkillActive = true;

        GetComponent<Rigidbody2D>().gravityScale = 0f;

        if(Mathf.Abs(xInputAxis) < INPUT_SOFT_THRESHOLD && Mathf.Abs(yInputAxis) < INPUT_SOFT_THRESHOLD)
        {
            float xDir = 1f;
            if (transform.localScale.x < 0f)
                xDir = -1f;
            xInputAxis = 1f * xDir;
            yInputAxis = 0f;
        }

        //Something is wrong with the warrior that prevents it from X dash distance but not enough time to debug
        //Bandage fix
        Vector2 quickFixForce = new Vector2();

        if (myXScale < 1f)
        {
            quickFixForce = new Vector2(xInputAxis, 0f).normalized * 240000;

        }

        GetComponent<Rigidbody2D>().AddForce(new Vector2(xInputAxis, yInputAxis).normalized * DASH_FORCE + quickFixForce);


        yield return new WaitForSeconds(.1f);

        VelocityX(0);
        VelocityY(0);

        movementSkillActive = false;

        GetComponent<Rigidbody2D>().gravityScale = GetDefaultGravityForce();

        if (isGrounded())
            GetComponent<Animator>().SetTrigger("exitDash");
        else
            GetComponent<Animator>().SetTrigger("heavyToAerial");

        ActivateAttackMovementJump();

        Invoke("ResetDashCount", DASH_RECOVERY_TIME);
        Invoke("StopAfterImage", STOP_AFTER_IMAGE);

        gameObject.layer = 15;
    }

    void ResetDashCount()
    {
        if (isGrounded())
        {
            dashCount = 0;
            checkGroundForDash = false;
        }
        else
            checkGroundForDash = true;
    }

    void StopAfterImage()
    {
        playerParticleEffects.PlayDashAfterImage(false);
        playerParticleEffects.PlayDashTrail(false);
    }

    //END DASHING FUNCTIONS

    //HEAVY CHARGING ATTACK FUNCTIONS

    void ManageChargeEffect()
    {
        timeCharged += Time.fixedDeltaTime;
        if (timeCharged >= 2f && !isFlashingGold)
        {
            isFlashingGold = true;
            CancelInvoke("ChargingFlashWhite");
            playerSoundEffects.playSound(playerSoundEffects.chargingSecondCharge);
            playerParticleEffects.PlayParticle(playerParticleEffects.chargingSecondCharge);
            InvokeRepeating("ChargingFlashGold", 0f, .09f);
        }
        else if (timeCharged >= 1f && timeCharged < 2f && !isFlashingWhite)
        {
            isFlashingWhite = true;
            playerSoundEffects.playSound(playerSoundEffects.chargingFirstCharge);
            playerParticleEffects.PlayParticle(playerParticleEffects.chargingFirstCharge);
            InvokeRepeating("ChargingFlashWhite", 0f, .15f);
        }
    }

    void StartHeavyGroundCharge()
    {
        checkChargeTime = true;
        timeCharged = 0f;
        GetComponent<SwordsmanParticleEffects>().PlayChargingDust(true);
    }

    void ExecuteHeavyAttack()
    {
        float chargeMultiplier = GetChargeMod();
        GetComponent<SwordsmanParticleEffects>().PlayChargingDust(false);
        CancelFlashing();
        playerEffectsManager.FlashScreen();
        GetComponent<SwordsmanParticleEffects>().PlayChargingTrail(true);
        checkChargeTime = false;
        attackScript.SetHeavyTier(chargeLevel);
        AddForceX(CHARGE_FORCE_MULTIPLIER * chargeMultiplier);
    }

    float GetChargeMod()
    {
        if (timeCharged < TIER_1_CHARGE)
        {
            chargeLevel = 0;
            return TIER_0_FORCE_MOD;
        }
        else if (timeCharged < MAX_CHARGE)
        {
            chargeLevel = 1;
            return TIER_1_FORCE_MOD;
        }
        chargeLevel = 2;
        return TIER_2_FORCE_MOD;
    }

    void EndHeavyAttack()
    {
        attackBox.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(EndChargingTrail(0.5f));
        attackScript.Launch();
        attackScript.Reset();
    }

    IEnumerator EndChargingTrail(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<SwordsmanParticleEffects>().PlayChargingTrail(false);
    }

    void FlashColor(Material mat)
    {
        if (GetComponent<SpriteRenderer>() == null)
            return;
        if (GetComponent<SpriteRenderer>().material == defaultMat)
            GetComponent<SpriteRenderer>().material = mat;
        else
            GetComponent<SpriteRenderer>().material = defaultMat;
    }

    void ChargingFlashWhite()
    {
        FlashColor(flashWhiteMat);
    }

    void ChargingFlashGold()
    {
        FlashColor(flashGoldMat);
    }

    void CancelFlashing()
    {
        CancelInvoke("ChargingFlashWhite");
        CancelInvoke("ChargingFlashGold");
        GetComponent<SpriteRenderer>().material = defaultMat;
        isFlashingWhite = false;
        isFlashingGold = false;
    }

    public void CancelHeavyCharge()
    {
        GetComponent<SwordsmanParticleEffects>().PlayChargingDust(false);
        checkChargeTime = false;
        CancelFlashing();
        attackScript.Reset();
    }

    //END HEAVY CHARGING ATTACK FUNCTIONS

    public void HeavyAttackScreenShake()
    {
        playerEffectsManager.ScreenShake(magHeavyAttackScreenShake, durHeavyAttackScreenShake);
    }

    public void SetAttackType(string type)
    {
        attackScript.SetAttackType(type);
    }

    public void EnableDash()
    {
        disableDash = false;
    }

    public void DisableDash()
    {
        disableDash = true;
    }

    public void AttackCAnimationAdjustment()
    {
        float xDir = 1f;
        if (transform.localScale.x < 0)
            xDir = -1f;
        myRigidbody.velocity = new Vector2(14f * xDir / Mathf.Abs(transform.localScale.x), myRigidbody.velocity.y);
        //Vector3 newPosition = transform.position;
        //newPosition.x += (1.5f * transform.localScale.x / Mathf.Abs(transform.localScale.x));
        //transform.position = newPosition;
    }
}
