using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretEnemy : EnemyPhysics
{
    const int AMMO_AMOUNT = 20;                             //Amount of ammo to be created
    const float BULLET_SPEED = 10f;                         //Speed of bullet
    const float RANGED_ATTACK_COOLDOWN = 1f;              //Cooldown for attack
    const float TARGETING_DELAY = .15f;                      //Delay for attacking after targetting

    public GameObject rangedProjectile;                     //Projectiles to be shot
    public Transform projectileList;                        //Transform containing projectiles
    public Transform projectileOrigin;                      //Where projectiles should spawn from

    bool attackOnCD = false;                                //Determines if attack should be on cool down
    bool acquiredTargetLocation = false;                    //Determines if enemy has tracked player last known position
    bool acquireTargetDelay = false;                        //Delay attack after targetting target location
    Vector3 targetLocation = new Vector3();                 //Player's last known position

    // Use this for initialization
    void Start()
    {
        currentAmmo = AMMO_AMOUNT;

        for (int i = 0; i < 20; i++)
        {
            GameObject bullet = (GameObject)Instantiate(rangedProjectile);
            bullet.GetComponent<EnemyBulletScript>().SetDamage(attackPower);
            bullet.transform.SetParent(projectileList);
            bullet.transform.position = projectileList.position;
            bullet.SetActive(false);
        }
    }

    public override void OnTriggerEnter2D(Collider2D col){}

    public override void OnTriggerExit2D(Collider2D collision){}

    public override void ApproachTarget()
    {
        if (target.transform.position.x > transform.position.x)
        {
            if (!facingRight)
                Turn();
        }
        else if (target.transform.position.x < transform.position.x)
        {
            if (facingRight)
                Turn();
        }
    }

    public override void RunEngagedBehavior()
    {
        if (!inAttackRange && canMove &&  !acquiredTargetLocation)
            ApproachTarget();
        else if (canAttack && !attackOnCD)
            if (!acquiredTargetLocation)
                AcquireTarget();
            else if(!acquireTargetDelay)
                ExecuteAttack();
    }

    public override void SpecificStunCancel()
    {
        acquiredTargetLocation = false;
        targetLocation = new Vector3();
    }

    public void Turn()
    {
        Flip();
        turning = true;
    }

    public void EndTurn()
    {
        turning = false;
    }

    public void AttackCDRecovery()
    {
        Invoke("SetCanAttackTrue", RANGED_ATTACK_COOLDOWN);
    }

    //NOTE TO SELF, CONSIDER WHAT HAPPENS WHEN STUNNED
    void AcquireTarget()
    {
        acquiredTargetLocation = true;
        if (target != null)
            targetLocation = target.transform.position;
        acquireTargetDelay = true;
        Invoke("EndTargetingDelay", TARGETING_DELAY);
    }

    void EndTargetingDelay()
    {
        acquireTargetDelay = false;
    }

    void SetCanAttackTrue()
    {
        canAttack = true;
    }

    public override void ExecuteAttack()
    {
        if (currentAmmo > 0)
        {
            base.ExecuteAttack();
            acquiredTargetLocation = false;
            attackOnCD = true;
            Invoke("EndAttackCD", RANGED_ATTACK_COOLDOWN);
        }
    }

    void EndAttackCD()
    {
        attackOnCD = false;
    }

    public void FireBullet()
    {
        for(int i = 0; i < AMMO_AMOUNT; i++)
        {
            if (!projectileList.GetChild(i).gameObject.activeSelf)
            {
                if (target == null)
                    return;
                projectileList.GetChild(i).gameObject.SetActive(true);

                Vector3 projectileSpawnPoint = projectileOrigin.position;
                projectileList.GetChild(i).position = projectileSpawnPoint;
                projectileList.GetChild(i).GetComponent<Rigidbody2D>().velocity = -(projectileSpawnPoint - new Vector3(targetLocation.x, targetLocation.y - 1f, targetLocation.z)).normalized * BULLET_SPEED;
                return;
            }
        }

    }




}
