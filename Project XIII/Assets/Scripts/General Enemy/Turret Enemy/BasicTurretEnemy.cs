using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretEnemy : EnemyPhysics
{

    const int AMMO_AMOUNT = 20;                             //Amount of ammo to be created

    public float rangedAttackCooldown = .1f;                //Cooldown between ranged attack

    public GameObject rangedProjectile;                    //Projectiles to be shot
    public Transform projectileList;                       //Transform containing projectiles

    // Use this for initialization
    void Start()
    {
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

    public void Turn()
    {
        anim.SetTrigger("Turn");
        turning = true;
    }

    public void EndTurn()
    {
        Flip();
        turning = false;
    }

    public void AttackCDRecovery()
    {
        Invoke("SetCanAttackTrue", rangedAttackCooldown);
    }

    void SetCanAttackTrue()
    {
        canAttack = true;
    }
}
