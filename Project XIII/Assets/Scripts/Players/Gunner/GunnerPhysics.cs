using UnityEngine;
using System.Collections;

public class GunnerPhysics : PlayerPhysics{


    //Constants for pistol shot
    const int MAX_PISTOL_AMMO = 6;              //Amount of ammo that can be fired before reload

    //Constants for down kick
    const float DK_DELTA_X = 1f;                //Amount to move in the x direction per an update for down kick
    const float DK_DELTA_Y = -1f;               //Amount to move in the y direction per an update for down kick

    GunnerStats gunnerStat;

    //SENSITVITY CONTROLS
    const float INPUT_SOFT_THRESHOLD = .1f;

    //Constants for managing quick dashing skill
    const float DASH_RECOVERY_TIME = 0.5f;                  //Time it takes to recover dashes
    const float MAX_CHAIN_DASH = 1;                         //Max amount of dashes that can be chained
    const float STOP_AFTER_IMAGE = .005f;                   //Time to stop creating afterimages
    const float DASH_FORCE = 5000f;                         //Amount of force to apply on character to perform movement

    //Constants for mananging light and heavy attack
    const float LIGHT_STUN_MULTI = 1f;                      //Multiplier for how long enemies should be stunned after light light
    const float HEAVY_STUN_MULTI = 1f;                      //Multiplier for how long enemies should be stunned after heavy attack
    const float HIT_DISCREP = .5f;                          //Discrepency from target location allowed for air juggle

    //Force to be applied on hit
    const float QUICK_FORCE_X = 100f;
    const float QUICK_FORCE_Y = 16000f;
    const float HEAVY_FORCE_X = 3500f;
    const float HEAVY_FORCE_Y = 5000f;

    //Pistol variables
    int pistolAmmo = MAX_PISTOL_AMMO;

    //Dash variables
    float xInputAxis = 0f;
    float yInputAxis = 0f;
    int dashCount = 0;                                      //Checks how many dashes have been chained
    bool checkGroundForDash = false;                        //Bool that determines to check for grounded before resetting dash count
    bool disableDash = false;

    //Variables for quick and heavy attack raycasting
    LayerMask layermask;                                    //Prevent raycast from hitting unimportant layers
    RaycastHit2D[] hit = new RaycastHit2D[5];               //What was hit by raycast
    RaycastHit2D[][] heavyHit = new RaycastHit2D[5][];

    //Down kick variables
    bool checkForDKEnd = false;                             //Checks if the downkick should end
    bool kickFinished = true;                               //For if kick is too close to the ground to properly cancel animation freeze

    public Transform bulletSource;                          //Source of bullets
    public GameObject meleeAttackBox;                       //Attack hit box for melee attacks and scripts

    GunnerParticleEffects playerParticleEffects;
    public Material tier1Mat;                               //Material to recolor gunner sprite for charge
    public Material tier2Mat;                               //Material to recolor gunner sprite for tier 2 charge

    //Combo Variable
    bool inCombo = false;                                   //Checks if gunner is able to combo into mid shot animation
    bool comboPressed = false;                              //Check if the combo button was pressed during combo
    bool checkForCombo = false;                             //Check if script should check for next combo press input
    bool midAnimReached = false;                            //Checks if midpoint animation for combo pistol attack has been reached


    public override void ClassSpecificStart()
    {
        gunnerStat = GetComponent<GunnerProperties>().GetGunnerStats();
        playerParticleEffects = GetComponent<GunnerParticleEffects>();
        layermask = (LayerMask.GetMask("Default", "Enemy"));
        meleeAttackBox.GetComponent<GunnerMeleeAttackScript>().enabled = false;
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

        if (inCombo)
            WatchForCombo();

        if (kickFinished)
            GetComponent<Animator>().enabled = true;

        if (checkGroundForDash)
            ResetDashCount();

    }

    public override bool CheckClassSpecificInput()
    {
        if (CanAttackStatus() && GetComponent<PlayerInput>().getKeyPress().quickAttackPress && isGrounded())
        {
            if (checkForCombo)
            {
                if (midAnimReached)
                {
                    PlayNextComboHit();
                }
                return true;
            }

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

    public void SetAttackType(string type)
    {
        meleeAttackBox.GetComponent<GunnerMeleeAttackScript>().SetAttackType(type);
    }

    //BEGIN QUICK SHOT FUNCTIONS

    //Cast out quick shot ray to apply damage
    public void QuickShot()
    {
        hit[0] = Physics2D.Raycast(bulletSource.position, bulletSource.right * transform.localScale.x, 50, layermask);
        hit[1] = Physics2D.Raycast(bulletSource.position + new Vector3(0, HIT_DISCREP, 0), bulletSource.right * transform.localScale.x, 50, layermask);
        hit[2] = Physics2D.Raycast(bulletSource.position + new Vector3(0, HIT_DISCREP * -1f, 0), bulletSource.right * transform.localScale.x, 50, layermask);

        for (int i = 0; i < 3; i++)
        {
            //make a spark at the hit.point

            if (hit[i].collider != null)
            {
                playerParticleEffects.PlayHitSpark(hit[i].point);
                ApplyQuickDamage(hit[i].collider.gameObject);
                break;
            }

        }

        /*
        Color color = Color.green;

        Debug.DrawRay(bulletSource.position, bulletSource.right * (60f * transform.localScale.x), color);
        Debug.DrawRay(bulletSource.position + new Vector3(0, .5f, 0), bulletSource.right * (60f * transform.localScale.x), color);
        Debug.DrawRay(bulletSource.position + new Vector3(0, -.5f, 0), bulletSource.right * (60f * transform.localScale.x), color);
        */

    }

    void ApplyQuickDamage(GameObject target)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<Enemy>().Damage(physicStats.quickAttackStrength, LIGHT_STUN_MULTI);
            if (!target.GetComponent<Enemy>().IsGrounded())
            {
                target.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -10f);
                target.GetComponent<Rigidbody2D>().AddForce(new Vector2(QUICK_FORCE_X, QUICK_FORCE_Y));
            }
        }

    }

    void ShootQuickBullet()
    {
        QuickShot();
        pistolAmmo--;
    }

    void ReloadPistolAmmo()
    {
        GetComponent<Animator>().SetTrigger("reload");
        pistolAmmo = MAX_PISTOL_AMMO;
    }

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
        midAnimReached = false;
    }

    public void ReportMidAnimReached()
    {
        if (comboPressed && isGrounded())
        {
            if (!GetComponent<PlayerProperties>().alive)
                return;

            PlayNextComboHit();

            return;
        }

        midAnimReached = true;
    }

    public void ResetCombo()
    {
        inCombo = false;
        midAnimReached = false;
        checkForCombo = false;
    }

    public void FinishCombo()
    {
        inCombo = false;
        StopCheckForCombo();
    }

    void PlayNextComboHit()
    {
        comboPressed = false;
        midAnimReached = false;

        if (pistolAmmo > 0)
        {
            myAnimator.SetTrigger("combo1");
            pistolAmmo--;
        }
        else
        {
            FinishCombo();
            myAnimator.SetTrigger("combo2");
            pistolAmmo = MAX_PISTOL_AMMO;
        }
    }

    void StopCheckForCombo()
    {
        checkForCombo = false;
        midAnimReached = false;
    }

    //END QUICK SHOT FUNCTIONS

    //BEGIN HEAVY SHOT FUNCTIONS

    public void HeavyShot()
    {
        heavyHit[0] = Physics2D.RaycastAll(bulletSource.position, bulletSource.right * transform.localScale.x, 5f, layermask);
        heavyHit[1] = Physics2D.RaycastAll(bulletSource.position, new Vector3(1 * transform.localScale.x, .5f, 0), 5, layermask);
        heavyHit[2] = Physics2D.RaycastAll(bulletSource.position, new Vector3(1 * transform.localScale.x, -.5f, 0), 5, layermask);
        heavyHit[3] = Physics2D.RaycastAll(bulletSource.position, new Vector3(1 * transform.localScale.x, -.25f, 0), 5, layermask);
        heavyHit[4] = Physics2D.RaycastAll(bulletSource.position, new Vector3(1 * transform.localScale.x, .25f, 0), 5, layermask);

        if (transform.parent != null)
        {
            transform.parent.GetComponent<PlayerEffectsManager>().ScreenShake(1f, 1f);
        }


        for (int i = 0; i < 5; i++)
            foreach (RaycastHit2D hh in heavyHit[i])
                if (hh)
                    if (hh.collider.tag == "Enemy")
                        ApplyHeavyDamage(hh.collider.gameObject, hit[i].distance);
    }

    void ApplyHeavyDamage(GameObject target, float distance)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<Rigidbody2D>().AddForce(new Vector2(HEAVY_FORCE_X * transform.localScale.x, HEAVY_FORCE_Y));
            target.GetComponent<Enemy>().Damage(physicStats.heavyAttackStrength, HEAVY_STUN_MULTI);
        }
    }

    void ShootHeavyBullet()
    {
        KnockBack(gunnerStat.heavyAttackKnockBackForce);
        HeavyShot();
    }


    //END HEAVY SHOT FUNCTIONS 


    //BEGIN HEAVY AIR ATTACK FUNCTIONS

    void ExecuteDownKick()
    {
        meleeAttackBox.GetComponent<GunnerMeleeAttackScript>().Reset();
        kickFinished = false;
        checkForDKEnd = true;
        meleeAttackBox.GetComponent<GunnerMeleeAttackScript>().enabled = true;
        meleeAttackBox.GetComponent<GunnerMeleeAttackScript>().InvokeRepeating("ApplyDamageEffect",0f,.1f);
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
        meleeAttackBox.GetComponent<GunnerMeleeAttackScript>().CancelInvoke("ApplyDamageEffect");
        meleeAttackBox.GetComponent<GunnerMeleeAttackScript>().enabled = false;
        GetComponent<PlayerProperties>().SetStunnableState(true);
    }

    //END HEAVY AIR ATTAACK FUNCTIONS



}
