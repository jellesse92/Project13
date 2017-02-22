using UnityEngine;
using System.Collections;

public class GunnerPhysics : PlayerPhysics{


    //Constants for pistol shot
    const int MAX_PISTOL_AMMO = 6;              //Amount of ammo that can be fired before reload
    const float QUICKSHOT_CD = .1f;             //Cooldown between gun shots

    //Constants for down kick
    const float DK_DELTA_X = 1f;                //Amount to move in the x direction per an update for down kick
    const float DK_DELTA_Y = -1f;               //Amount to move in the y direction per an update for down kick

    GunnerStats gunnerStat;

    public GameObject bulletSource;
    public GameObject meleeAttackBox;
    public DownKickScript downKickScript;


    //float yAtStart;
    int dodgeCount = 0;

    //Pistol variables
    bool pistolOnCD = false;
    int pistolAmmo = MAX_PISTOL_AMMO;

    //Down kick variables
    bool checkForDKEnd = false;                     //Checks if the downkick should end
    bool kickFinished = true;                       //For if kick is too close to the ground to properly cancel animation freeze




    //SENSITVITY CONTROLS
    const float INPUT_SOFT_THRESHOLD = .1f;

    //Constants for managing quick dashing skill
    const float DASH_RECOVERY_TIME = 0.5f;      //Time it takes to recover dashes
    const float MAX_CHAIN_DASH = 1;             //Max amount of dashes that can be chained
    const float STOP_AFTER_IMAGE = .005f;       //Time to stop creating afterimages
    const float DASH_FORCE = 5000f;             //Amount of force to apply on character to perform movement

    //Dash variables
    float xInputAxis = 0f;
    float yInputAxis = 0f;
    int dashCount = 0;                          //Checks how many dashes have been chained
    bool checkGroundForDash = false;            //Bool that determines to check for grounded before resetting dash count
    bool disableDash = false;



    GunnerParticleEffects playerParticleEffects;


    public override void ClassSpecificStart()
    {
        gunnerStat = GetComponent<GunnerProperties>().GetGunnerStats();
        playerParticleEffects = GetComponent<GunnerParticleEffects>();








        meleeAttackBox.GetComponent<MeleeAttackScript>().SetAttackStrength(GetComponent<PlayerProperties>().GetPlayerStats().quickAirAttackStrength);
        downKickScript.enabled = false;
    }

    public override void ClassSpecificUpdate()
    {
        if (checkForDKEnd)
        {
            if (isGrounded())
            {
                CancelDownKick();
                transform.parent.GetComponent<PlayerEffectsManager>().ScreenShake(1f, 1f);
            }
            else
                DownKickMove();
        }

        if (kickFinished)
            GetComponent<Animator>().enabled = true;






        if (checkGroundForDash)
            ResetDashCount();

    }

    public override bool CheckClassSpecificInput()
    {
        if (GetComponent<PlayerInput>().getKeyPress().quickAttackPress && isGrounded())
        {
            if (pistolOnCD)
                return true;
            if(pistolAmmo <= 0)
            {
                ReloadPistolAmmo();
                return true;
            }
        }

        return base.CheckClassSpecificInput();
    }

    public override void MovementSkill(float xMove, float yMove)
    {
        if (disableDash)
            return;
        base.MovementSkill(xMove, yMove);

        if (Mathf.Abs(xMove) < .01 && Mathf.Abs(yMove) < .01f)
        {
            xInputAxis = transform.localScale.x;
            yInputAxis = 0f;
        }
        else
        {
            xInputAxis = xMove;
            yInputAxis = yMove;
        }

        if (dashCount < MAX_CHAIN_DASH)
            GetComponent<Animator>().SetTrigger("moveSkill");
    }

    //DASHING FUNCTIONS

    public void ExecuteDashSkill()
    {
        dashCount++;
        CancelInvoke("StopAfterImage");
        CancelInvoke("ResetDashCount");

        StartCoroutine("Dashing");

        playerParticleEffects.PlayDashAfterImage(true);
        //playerParticleEffects.PlayDashTrail(true);
        gameObject.layer = 14;
    }

    IEnumerator Dashing()
    {
        DeactivateAttackMovementJump();
        VelocityY(0f);
        VelocityX(0f);

        GetComponent<Rigidbody2D>().gravityScale = 0f;

        if (Mathf.Abs(xInputAxis) < INPUT_SOFT_THRESHOLD && Mathf.Abs(yInputAxis) < INPUT_SOFT_THRESHOLD)
        {
            xInputAxis = 1f * transform.localScale.x;
            yInputAxis = 0f;
        }

        GetComponent<Rigidbody2D>().AddForce(new Vector2(xInputAxis, yInputAxis).normalized * DASH_FORCE);
        yield return new WaitForSeconds(.1f);
        VelocityX(0);
        VelocityY(0);

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
        //playerParticleEffects.PlayDashTrail(false);
    }

    //END DASHING FUNCTIONS



    void QuickShotRecovery()
    {
        pistolOnCD = false;
    }

    void ShootQuickBullet()
    {
        bulletSource.GetComponent<BulletSourceScript>().QuickShot(physicStats.quickAttackStrength);
        pistolOnCD = true;
        pistolAmmo--;
        Invoke("QuickShotRecovery", QUICKSHOT_CD);
    }

    void ShootHeavyBullet()
    {
        KnockBack(gunnerStat.heavyAttackKnockBackForce);
        bulletSource.GetComponent<BulletSourceScript>().HeavyShot(physicStats.heavyAttackStrength);
    }

    void ExecuteDownKick()
    {
        downKickScript.Reset();
        kickFinished = false;
        checkForDKEnd = true;
        downKickScript.enabled = true;
        downKickScript.InvokeRepeating("ApplyDamageEffect",0f,.1f);
        GetComponent<PlayerProperties>().SetStunnableState(false);
    }

    void StartDownKickMove()
    {
        checkForDKEnd = true;
    }

    void DownKickMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position += new Vector3(DK_DELTA_X * transform.localScale.x,DK_DELTA_Y,0f),100f);
    }

    void CancelDownKick()
    {
        GetComponent<Animator>().enabled = true;
        kickFinished = true;
        checkForDKEnd = false;
        downKickScript.CancelInvoke("ApplyDamageEffect");
        downKickScript.enabled = false;
        GetComponent<PlayerProperties>().SetStunnableState(true);
    }

    void ApplyBounce()
    {
        downKickScript.ApplyBounce();
    }


    void ReloadPistolAmmo()
    {
        GetComponent<Animator>().SetTrigger("reload");
        pistolAmmo = MAX_PISTOL_AMMO;
    }   
}
