using UnityEngine;
using System.Collections;

public class CowBoyClass : PlayerCharacter{

    public int AttackBoost = 0;
    public int SpeedBoost = 0;
    public CowBoyClass()
    {
        this.PlayerSpeed += 1;
    }

    public new int Attack(int attackType)
    {
        int AttackPower = base.Attack(attackType);
        //Spawn Attack Animation based off of attackType. Send them AttackPower, which is the amnt of damage it causes.
        return -1; 
        // if this player collider comes into contact with the enemy, even if attacking, nothing should happen.
        // All attack damage comes from ranged weapons.
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
