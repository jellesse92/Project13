using UnityEngine;
using System.Collections;

public class JazzCowBoyClass : JazzPlayerCharacter
{
   
    public Transform PrimaryBullets;
    public Transform SecondaryAtk;
    public float SecondaryCoolDown = 1.2f;
    public float dodgeCoolDown = 1.0f;
    private float specialAttackTimestamp;
    private float dodgeTimeStamp;
    public int AttackBoost = 0;
    public float SpeedBoost = 0;

    void Start()
    {
        this.PlayerSpeed += 1;
        specialAttackTimestamp = Time.time;
        dodgeTimeStamp = Time.time;
        this.SecondaryAS = 2.1f;
    }
    public override void Block(char dir)
    {
        if(dodgeTimeStamp <= Time.time)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(dir == 'R' ? -4 : 4, 0), ForceMode2D.Impulse);
            dodgeTimeStamp = Time.time + dodgeCoolDown;
        }
    }
    public override int Attack2D(int attackType, char direction)
    {

        int AttackPower = base.Attack2D(attackType + AttackBoost);
        if(attackType == 0)
        {
            Shoot2DPrimary(AttackPower, direction);
        }
        if(attackType == 1)
        {
            if(specialAttackTimestamp <= Time.time)
            {
                Shoot2dSecondary(AttackPower, direction);
                specialAttackTimestamp = Time.time + SecondaryCoolDown;

            }
        }
        //Do Attack Animation based off of attackType. Send them AttackPower, which is the amnt of damage it causes.
        return -1; 
        // if this player collider comes into contact with the enemy, even if attacking, nothing should happen.
        // All attack damage comes from ranged weapons.
    }

    void Shoot2DPrimary(int AttackPower, char direction)
    {
        Vector2 velocity = direction == 'R' ? Vector2.right : Vector2.left;
        foreach (Transform child in PrimaryBullets)
        {
            float bulletSpeed = PrimaryAS * SpeedBoost;
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<PlayerProjectile>().SetDamageAmount(AttackPower);

                Vector3 gunPoint = transform.position;
                gunPoint.x += 1f;
                gunPoint.y += .7f;

                child.position = gunPoint;
                child.GetComponent<Rigidbody2D>().velocity = velocity * bulletSpeed;
                return;
            }
        }
    }

    void Shoot2dSecondary(int AttackPower, char dir)
    {
        Vector2 velocity = dir == 'R' ? Vector2.right : Vector2.left;
        float bulletSpeed = SecondaryAS * SpeedBoost;
        foreach (Transform child in SecondaryAtk)
        {
           
            if (!child.gameObject.activeSelf)
            {
                GetComponent<Animator>().SetTrigger("heavyAttack");
                GetComponent<JazzPlayer>().TakeDamage(0, dir == 'R' ? -3 : 3);
                PlayerProjectile bullet = child.gameObject.GetComponent<PlayerProjectile>();
                bullet.gameObject.transform.position = gameObject.transform.position;
                child.gameObject.SetActive(true);
                bullet.SetDamageAmount(AttackPower);
                bullet.DamageFadeActive(true);
                bullet.SetMaxDistance(2);
                child.GetComponent<Rigidbody2D>().velocity = velocity * bulletSpeed;
                return;
            }
        }
        
    }
    
}
