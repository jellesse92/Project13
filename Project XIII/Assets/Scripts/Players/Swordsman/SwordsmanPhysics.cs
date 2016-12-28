using UnityEngine;
using System.Collections;

public class SwordsmanPhysics : PlayerPhysics{

    //SENSITVITY CONTROLS
    const float Y_INPUT_THRESHOLD = .5f;        //Threshold before considering input

    //Constants for managing quick dashing skill
    const float DASH_DISTANCE = 10f;            //Distance of dash
    const float DASH_RECOVERY_TIME = 1f;        //Time it takes to recover dashes
    const float MAX_CHAIN_DASH = 3;             //Max amount of dashes that can be chained
    const float STOP_AFTER_IMAGE = .005f;        //Time to stop creating afterimages

    //Constant for charging ground heavy slash attack
    const float MAX_CHARGE = 2f;                //Max amount of time multiplier allowed to be applied to charge distance
    const float TIER_1_CHARGE = .8f;            //Tier one charge for beginning to flash white
    const float CHARGE_FORCE_MULTIPLIER = 3000f;//Multiplier for distance to travel after charging attack

    //Attack box
    public GameObject attackBox;                //Collider for dealing all melee attacks
    public SwordsmanAttackScript attackScript;  //Script for managing attack

    //Dash variables
    float xInputAxis = 0f;                               
    float yInputAxis = 0f;
    int dashCount = 0;                          //Checks how many dashes have been chained
    bool checkGroundForDash = false;            //Bool that determines to check for grounded before resetting dash count

    //Combo Variable
    bool inCombo = false;                       //Checks if swordsman able to combo

    //Heavy attack variables
    bool checkChargeTime = false;               //Determines if should check for time charging
    float timeCharged;                          //Time attack has been charged
    Material defaultMat;                        //Default sprite material
    public Material flashWhiteMat;              //White flashing material
    public Material flashGoldMat;               //Gold flashing material
    bool isFlashingWhite = false;               //Determines if flashing white color
    bool isFlashingGold = false;                //Determines if flashing gold color

    PlayerParticleEffects playerParticleEffects;

    public override void ClassSpecificStart()
    {
        playerParticleEffects = GetComponent<PlayerParticleEffects>();
        defaultMat = GetComponent<SpriteRenderer>().material;
    }

    public override void ClassSpecificUpdate()
    {
        if (inCombo)
            WatchForCombo();
        if (checkGroundForDash)
            ResetDashCount();
        if (checkChargeTime)
        {
            timeCharged += Time.fixedDeltaTime;
            if(timeCharged >= 2f && !isFlashingGold)
            {
                isFlashingGold = true;
                CancelInvoke("ChargingFlashWhite");
                InvokeRepeating("ChargingFlashGold", 0f, .09f);
            }
            else if(timeCharged >= 1f && timeCharged < 2f && !isFlashingWhite)
            {
                isFlashingWhite = true;
                InvokeRepeating("ChargingFlashWhite", 0f, .15f);
            }
        }

    }

    public override bool CheckClassSpecificInput()
    {
        float xMove = myKeyPress.horizontalAxisValue;
        float yMove = myKeyPress.verticalAxisValue;

        if (CanAttackStatus() && (yMove > Y_INPUT_THRESHOLD) && GetComponent<PlayerInput>().getKeyPress().quickAttackPress && isGrounded())
        {
            GetComponent<Animator>().SetTrigger("upQuickAttack");
            return true;
        }

        return false;
    }

    public override void MovementSkill(float xMove, float yMove)
    {
        base.MovementSkill(xMove,yMove);

        xInputAxis = xMove;
        yInputAxis = yMove;

        if(dashCount < MAX_CHAIN_DASH)
            GetComponent<Animator>().SetTrigger("moveSkill");
    }

    //COMBO FUNCTIONS

    public void WatchForCombo()
    {
        if (GetComponent<PlayerInput>().getKeyPress().quickAttackPress)
        {
            inCombo = false;
            GetComponent<Animator>().SetTrigger("combo");
        }
    }

    public void StartCombo()
    {
        inCombo = true;
    }

    public void FinishCombo()
    {
        inCombo = false;
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
    }

    public void EndDragAttack()
    {
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
        RaycastHit2D hit;
        Vector2 dir = new Vector2(xInputAxis, yInputAxis).normalized;

        dashCount++;
        CancelInvoke("StopAfterImage");
        CancelInvoke("ResetDashCount");

        if (dir.x == 0f && dir.y == 0f)
            dir.x = transform.localScale.x;

        hit = Physics2D.Raycast(transform.position, dir, DASH_DISTANCE, LayerMask.GetMask("Default"));

        if (hit.collider == null)
            StartCoroutine(Dashing(transform.position + new Vector3(dir.x * DASH_DISTANCE, dir.y * DASH_DISTANCE, transform.position.z)));
        else
        {
            float xOffset = 0f;
            float yOffset = 0f;

            StartCoroutine(Dashing(new Vector3(hit.point.x + xOffset, hit.point.y + yOffset, transform.position.z)));
        }

        playerParticleEffects.PlayDashAfterImage(true);
    }

    IEnumerator Dashing(Vector3 destination)
    {
        float gravity = GetComponent<Rigidbody2D>().gravityScale;

        DeactivateAttackMovementJump();
        VelocityY(0f);
        VelocityX(0f);

        GetComponent<Rigidbody2D>().gravityScale = 0f;

        transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE / 5f);

        for (int i = 0; i < 4; i++)
        {
            if (transform.position.x == destination.x && transform.position.y == destination.y)
                break;
            yield return new WaitForSeconds(.01f);
            transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE / 5f);
        }

        GetComponent<Rigidbody2D>().gravityScale = gravity;

        if (isGrounded())
            GetComponent<Animator>().SetTrigger("exitDash");
        else
            GetComponent<Animator>().SetTrigger("heavyToAerial");

        ActivateAttackMovementJump();

        Invoke("ResetDashCount", DASH_RECOVERY_TIME);
        Invoke("StopAfterImage", STOP_AFTER_IMAGE);
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
    }

    //END DASHING FUNCTIONS

    //HEAVY CHARGING ATTACK FUNCTIONS

    void StartHeavyGroundCharge()
    {
        checkChargeTime = true;
        timeCharged = 0f;
        GetComponent<SwordsmanParticleEffects>().PlayChargingDust(true);
        InvokeRepeating("ChargingShake", .7f, .1f);
    }

    void ExecuteHeavyAttack()
    {
        GetComponent<SwordsmanParticleEffects>().PlayChargingDust(false);
        CancelFlashing();

        checkChargeTime = false;
        timeCharged = Mathf.Min(timeCharged, MAX_CHARGE);
        attackScript.SetForceMulti(timeCharged);
        AddForceX(CHARGE_FORCE_MULTIPLIER * timeCharged);
    }

    void EndHeavyAttack()
    {
        attackBox.GetComponent<Collider2D>().enabled = false;
        attackScript.Launch();
        attackScript.Reset();
    }

    void ChargingShake()
    {
        if (transform.rotation.z == 0f)
            transform.Rotate(new Vector3(0f, 0f, -1f));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

    }

    void FlashColor(Material mat)
    {
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
        CancelInvoke("ChargingShake");
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
        ScreenShake(.1f, .03f);
    }

    public void SetAttackType(string type)
    {
        attackScript.SetAttackType(type);
    }

}
