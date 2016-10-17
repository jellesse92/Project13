using UnityEngine;
using System.Collections;

public class CowBoyClass : PlayerCharacter{

    public Transform PrimaryBullets;
    public int AttackBoost = 0;
    public int SpeedBoost = 2;
    public CowBoyClass()
    {
        this.PlayerSpeed += 1;
    }

    public override int Attack2D(int attackType, char direction)
    {
        print("Attacking");
        int AttackPower = base.Attack2D(attackType + AttackBoost);
        print(attackType);
        if(attackType == 0)
        {
            print("shooting");
            Shoot2DPrimary(AttackPower, direction);
        }
        //Do Attack Animation based off of attackType. Send them AttackPower, which is the amnt of damage it causes.
        return -1; 
        // if this player collider comes into contact with the enemy, even if attacking, nothing should happen.
        // All attack damage comes from ranged weapons.
    }

    void Shoot2DPrimary(int AttackPower, char direction)
    {
        Vector2 velocity;
        if (direction == 'R')
        {
            velocity = Vector2.right;
        }
        else
        {
            velocity = Vector2.left;
        }
        foreach (Transform child in PrimaryBullets)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<PlayerProjectile>().SetDamageAmount(AttackPower);
                child.position = transform.position;
                child.GetComponent<Rigidbody2D>().velocity = velocity * (PrimaryAS * 2);
                return;
            }
        }
    }

}
