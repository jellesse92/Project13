using UnityEngine;
using System.Collections;

/// <summary>
/// Player Character Class
/// Functions ---- 
/// AddLife,
/// AddCash,
/// ClearCash,
/// GetCash,
/// Get Max Health,
/// Get Current Health,
/// Player Death,
/// Take Damage
/// ---
/// </summary>

public class PlayerCharacter : MonoBehaviour{

    public bool Alive = true;
    public int PlayerNumber = 0;
    public int LivesLeft = 3;
    public int CurrentHealth = 100;
    public int PrimaryAP = 20; //Attack Power
    public int SecondaryAP = 40;
    public int Cash = 0;
    public int MaxHealth = 100;
    public float PlayerSpeed = 2.0f;
    public float PrimaryAS = 1.0f; //Attack Speed
    public float SecondaryAS = 2.0f;

    public void AddLife()
    {
        LivesLeft++;
        if(Alive == false)
            Alive = true;
    }

    public void PlayerDeath()
    {
        LivesLeft--;
        CurrentHealth = MaxHealth;
        if(LivesLeft == -1)
        {
            Alive = false;
            Cash = 0;
        }
    }

    public virtual void Block(char dir)
    {
        
    }

    public void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;
    }

    public int GetCash()
    {
        return Cash;
    }

    public void AddCash(int amnt)
    {
        Cash = Cash + amnt;
    }

    public void ClearCash()
    {
        Cash = 0;
    }
    public int GetMaxHealth()
    {
        return MaxHealth;
    }
    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }
    public virtual int Attack2D(int attackType, char direction = ' ')
    {
        if (attackType == 1)
        {
            return PrimaryAP;
        } else
        {
            return SecondaryAP;
        }
    }




}
