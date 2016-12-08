using UnityEngine;
using System.Collections;

public class GunnerPhysics : PlayerPhysics{

    //Constants for Dodge Roll
    const float DODGE_DIST_PER_INVOKE = .2f;     //Amount gunner rolls per invoke
    const float DODGE_RECOVERY_TIME = 1f;
    const float MAX_DODGE_CHAIN = 3;

    //Constants for pistol shot
    const int MAX_PISTOL_AMMO = 6;              //Amount of ammo that can be fired before reload
    const float QUICKSHOT_CD = .1f;             //Cooldown between gun shots

    //Constants for down kick
    const float DK_DELTA_X = 1f;                //Amount to move in the x direction per an update for down kick
    const float DK_DELTA_Y = -1f;               //Amount to move in the y direction per an update for down kick

    GunnerStats gunnerStat;
    BulletProjectile bulletScript;
    Vector3 gunPoint;
    Vector2 velocity;
    float bulletSpeed;

    public GameObject bulletSource;
    public GameObject meleeAttackBox;
    public DownKickScript downKickScript;

    //Dodge rolling variables
    float xInputAxis;
    float yAtStart;
    int dodgeCount = 0;

    //Pistol variables
    bool pistolOnCD = false;
    int pistolAmmo = MAX_PISTOL_AMMO;

    //Down kick variables
    bool checkForDKEnd = false;                     //Checks if the downkick should end
    bool kickFinished = true;                       //For if kick is too close to the ground to properly cancel animation freeze

    public override void ClassSpecificStart()
    {
        gunnerStat = GetComponent<GunnerProperties>().GetGunnerStats();
        meleeAttackBox.GetComponent<MeleeAttackScript>().SetAttackStrength(GetComponent<PlayerProperties>().GetPhysicStats().quickAirAttackStrength);
        downKickScript.enabled = false;
    }

    public override void ClassSpecificUpdate()
    {
        if (checkForDKEnd)
        {
            if (isGrounded())
            {
                CancelDownKick();
                transform.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.08f, .1f);
            }
            else
                DownKickMove();
        }

        if (kickFinished)
            GetComponent<Animator>().enabled = true;

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
        base.MovementSkill(xMove, yMove);

        float dir;

        dir = (xMove >= 0f) ? 1f : -1f;
        dir = (xMove == 0f) ? transform.localScale.x : dir;

        xInputAxis = dir;

        if (dodgeCount < MAX_DODGE_CHAIN && isGrounded())
            GetComponent<Animator>().SetTrigger("moveSkill");
    }

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

    //DODGE ROLL FUNCTIONS

    void ExecuteDodgeSkill()
    {
        CancelInvoke("FinishDodgeCD");
        InvokeRepeating("Roll", 0f, .001f);
        dodgeCount++;
    }

    void Roll()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(xInputAxis*DODGE_DIST_PER_INVOKE,0f,0f), DODGE_DIST_PER_INVOKE);
    }

    void CancelRoll()
    {
        CancelInvoke("Roll");
        Invoke("FinishDodgeCD", DODGE_RECOVERY_TIME);

        if (isGrounded())
            GetComponent<Animator>().SetTrigger("exitDash");
        else
            GetComponent<Animator>().SetTrigger("heavyToAerial");
    }

    void FinishDodgeCD()
    {
        dodgeCount = 0;
    }

    //END DODGE ROLL FUNCTIONS

    void ReloadPistolAmmo()
    {
        GetComponent<Animator>().SetTrigger("reload");
        pistolAmmo = MAX_PISTOL_AMMO;
    }

    
}
