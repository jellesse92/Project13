using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerPhysicStats
{
    public int quickAttackStrength = 20; //Attack Power
    public int heavyAttackStrength = 40;
    public int quickAirAttackStrength = 5;
    public int heavyAirAttackStrengh = 10;

    public float quickAttackSpeed = 1f; //Attack Speed
    public float heavyAttackSpeed = 1f;

    public float movementSpeed =10f;
    public float jumpForce = 15f;
}

[System.Serializable]
public class PlayerBoostStats
{
    public int attackBoost = 0;
    public float speedBoost = 0;

}

public class PlayerProperties : MonoBehaviour{

    public bool alive = true;

    public int playerNumber = 0;
    public int lives = 3;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int cash = 0;

    public PlayerPhysicStats physicStats;
    public PlayerBoostStats boostStats;

    protected GameObject psScript;
    protected bool isStunned = false;
    protected bool stunnable = true;

    protected bool isInvincibile = false;

    private bool isKnockedBack = false;
    public bool isKnockedInAir = false;
    private int playerAngle = 0;


    void Start()
    {
        psScript = GameObject.FindGameObjectWithTag("In Game Status Panel");
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    public PlayerPhysicStats GetPhysicStats()
    {
        return physicStats;
    }

    public PlayerBoostStats GetBoostStats()
    {
        return boostStats;
    }

    public void AddLife()
    {
        lives++;
        if(alive == false)
            alive = true;
    }

    public void PlayerDeath()
    {
        lives--;
        currentHealth = maxHealth;
        if(lives == -1)
        {
            alive = false;
            cash = 0;
        }
    }

    public void TakeDamage(int dmg, float knockBackX = 0f, float knockBackY = 0f, float stunTime = 0f)
    {
        if (isInvincibile)
            return;

        currentHealth -= dmg;
        //Prevent stacking KnockBack
        knockXPlayer(knockBackX, knockBackY);
        
        if (stunnable && !isStunned)
            StartCoroutine(ApplyStun(stunTime));

        if (psScript != null)
            psScript.GetComponent<PlayerStatusUIScript>().ApplyHealthDamage(playerNumber, dmg);
    }
    private void knockXPlayer(float x, float y) 
    {
        if (!isKnockedBack)
        {
            isKnockedBack = x > 0;
            Invoke("unsetKnockedBack", 1f);
        }
        
        x = x % 4000; //Avoids Stacking Knockbacks 
        y = y % 4000;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y));
    }
    //void OnCollision
    private void unsetKnockedBack()
    {
        isKnockedBack = false;
    }

    public int GetCash()
    {
        return cash;
    }

    public void AddCash(int amnt)
    {
        cash = cash + amnt;
    }

    public void ClearCash()
    {
        cash = 0;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    IEnumerator ApplyStun(float duration)
    {
        isStunned = true;
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("stun");
        yield return new WaitForSeconds(duration);
        GetComponent<Animator>().SetTrigger("stunRecovery");
        isStunned = false;
    }

    public bool GetStunState()
    {
        return isStunned;
    }

    public void SetStunnableState(bool b)
    {
        stunnable = b;
    }

    public void MakeInvuln()
    {
        isInvincibile = true;
    }

    public void MakeNotInvul()
    {
        isInvincibile = false;
    }
}
