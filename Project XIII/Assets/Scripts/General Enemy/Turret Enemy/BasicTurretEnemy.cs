using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretEnemy : EnemyPhysics
{

    const int AMMO_AMOUNT = 20;                             //Amount of ammo to be created
    const float BULLET_SPEED = 15f;                         //Speed of bullet
    const float RANGED_ATTACK_COOLDOWN = 1f;                //Cooldown for attack

    public GameObject rangedProjectile;                    //Projectiles to be shot
    public Transform projectileList;                       //Transform containing projectiles

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

        if (!inAttackRange && canMove)
            ApproachTarget();
        else if (canAttack)
            ExecuteAttack();
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

    void SetCanAttackTrue()
    {
        canAttack = true;
    }

    public override void ExecuteAttack()
    {
        if (currentAmmo > 0)
        {
            base.ExecuteAttack();
        }
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
                projectileList.GetChild(i).position = projectileList.position;
                projectileList.GetChild(i).GetComponent<Rigidbody2D>().velocity = -(new Vector3(transform.position.x, transform.position.y + 1.5f,transform.position.z) - target.transform.position).normalized * BULLET_SPEED;
                return;
            }
        }

    }




}
