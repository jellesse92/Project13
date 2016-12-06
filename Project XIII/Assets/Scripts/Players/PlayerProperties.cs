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

    const float reviveImmuneTime = .2f;                     //Time immune to damage after revive

    public AudioClip[] lastDeathVoices;                    //Array of death voices for characters

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

    //Camera stuff for when player dies
    private bool isVisible = true;                  //Visible on the screen
    private GameObject cam;

    //Death voice stuff
    bool checkVoiceDone = false;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject;
    } 

    void Start()
    {
        psScript = GameObject.FindGameObjectWithTag("In Game Status Panel");

    }

    public virtual void Update()
    {
        if (checkVoiceDone)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                Time.timeScale = .5f;
                checkVoiceDone = false;
            }

        }
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
        bool isLast = false;

        GetComponent<PlayerPhysics>().ConstrainX();
        MakeInvuln();
        alive = false;
        
        isLast = transform.parent.GetComponent<PlayerEffectsManager>().ReportLastDeath();

        if (isLast)
        {
            LastDeathEvent();
        }
        else
            GetComponent<Animator>().SetTrigger("death");


        cam.GetComponent<MultiplayerCamFollowScript>().RemovePlayerFromFocus(gameObject);
        /*
            lives--;

            currentHealth = maxHealth;
        */
    }

    //Plays if character is last to die
    void LastDeathEvent()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "Special Effects";
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        GetComponent<Animator>().SetTrigger("finalDeath");
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void PlayLastDeathVoice()
    {
        int r = lastDeathVoices.Length;
        r = (int)Random.Range(0, r - 1);

        GetComponent<AudioSource>().clip = lastDeathVoices[r];
        GetComponent<AudioSource>().Play();
        checkVoiceDone = true;
    }

    public void Revive()
    {
        cam.GetComponent<MultiplayerCamFollowScript>().AddPlayerToFocus(gameObject);
        GetComponent<PlayerPhysics>().DeconstrainX();
        alive = true;
        GetComponent<Animator>().SetTrigger("revive");
        lives--;
        Invoke("MakeNotInvul", reviveImmuneTime);
    }


    public void TakeDamage(int dmg, float knockBackX = 0f, float knockBackY = 0f, float stunTime = 0f)
    {
        if (isInvincibile)
            return;

        GetComponent<Rigidbody2D>().gravityScale = GetComponent<PlayerPhysics>().GetDefaultGravityForce();

        currentHealth -= dmg;
        //Prevent stacking KnockBack
        knockXPlayer(knockBackX, knockBackY);

        if (alive && currentHealth <= 0)
        {
            PlayerDeath();
            return;
        }

        if (stunnable && !isStunned)
            StartCoroutine(ApplyStun(stunTime));

        if (psScript != null)
            psScript.GetComponent<PlayerStatusUIScript>().ApplyHealthDamage(playerNumber, dmg);
    }

    private void knockXPlayer(float x, float y) 
    {
        //print("Kncking back with values" + x + "  AND  " + y);
        if (!isKnockedBack)
        {
            isKnockedBack = x != 0;
            x = x % 3000;
            Invoke("unsetKnockedBack", 2f);
        } else
        {
            x = 0;
        }
        
        y = y % 5555;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y));

    }

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
