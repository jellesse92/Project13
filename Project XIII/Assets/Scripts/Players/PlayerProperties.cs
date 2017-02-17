using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerStats
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
    const float damageImmuneTime = 1f;                     //Time immune to damage after taking damage


    public bool alive = true;

    public int playerNumber = 0;
    public int lives = 3;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int healingItem = 3;
    public int cash = 0;

    public bool isKnockedInAir = false;

    public PlayerStats playerStats;
    public PlayerBoostStats boostStats;

    protected GameObject psScript;
    protected bool isStunned = false;
    protected bool stunnable = true;

    protected bool isInvincibile = false;

    private bool isKnockedBack = false;
    private int playerAngle = 0;

    //Camera stuff for when player dies
    private bool isVisible = true;                  //Visible on the screen
    private GameObject cam;

    //Death voice stuff
    bool checkVoiceDone = false;

    //Screenshak and Screenflash
    public float magDamageScreenShake = 0.2f;
    public float durDamageScreenShake = 0.5f;
    //Might put this in physics instead

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject;
    } 

    void Start()
    {
        psScript = GameObject.FindGameObjectWithTag("In Game Status Panel");

    }

    public virtual void Update() //move this to player physics maybe? 
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

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
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
        Invoke("RestartLevel", 3f);
    }

    void RestartLevel()
    {
        Debug.Log("TEMPORARY");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopTime() //move this to player physics maybe? 
    {
        Time.timeScale = 0f;
    }

    public void PlayLastDeathVoice() //move this to player physics maybe? 
    {
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

        if (transform.parent.GetComponent<PlayerEffectsManager>())
        {
            //Activate Screen Flash
            transform.parent.GetComponent<PlayerEffectsManager>().ScreenShake(magDamageScreenShake, durDamageScreenShake);
            //Activate Screen Shake
            transform.parent.GetComponent<PlayerEffectsManager>().DamageFlashScreen();
        }

        GetComponent<Rigidbody2D>().gravityScale = GetComponent<PlayerPhysics>().GetDefaultGravityForce();

        currentHealth -= dmg;
        //Prevent stacking KnockBack
        StartCoroutine(knockXPlayer(knockBackX, knockBackY));
        //knockXPlayer(knockBackX, knockBackY);

        //Play death
        if (alive && currentHealth <= 0)
        {
            PlayerDeath();
            return;
        }

        //Applies invincibility
        StartCoroutine("ApplyInvin");

        //Applies stun
        if (stunnable && !isStunned)
            StartCoroutine(ApplyStun(stunTime));

        //Updates player health ui
        if (psScript != null)
            psScript.GetComponent<PlayerStatusUIScript>().ApplyHealthDamage(playerNumber, dmg);
    }

    private IEnumerator knockXPlayer(float x, float y, float knockDur = 0.02f) 
    {
        //print("Knockback X = " + x + " Knockback y = " + y);
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            float timer = 0;

            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

            while (knockDur > timer)
            {
                timer += Time.deltaTime;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(x, y, 0));
            }
            Invoke("unsetKnockedBack", .5f);
        }
        yield return 0;

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
        GetComponent<PlayerPhysics>().CancelWasGroundedInvoke();
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("stun");
        yield return new WaitForSeconds(duration);
        GetComponent<Animator>().SetTrigger("stunRecovery");
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        isStunned = false;
    }

    IEnumerator ApplyInvin()
    {
        isInvincibile = true;

        for (int i = 0; i < 3; i++)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
            yield return new WaitForSeconds(damageImmuneTime/7f);
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .50f);
            yield return new WaitForSeconds(damageImmuneTime / 7f);
        }

        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
        yield return new WaitForSeconds(damageImmuneTime / 7f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        isInvincibile = false;
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

    public void Heal()
    {
        currentHealth = maxHealth;
        healingItem--;
        if (psScript != null)
        {
            psScript.GetComponent<PlayerStatusUIScript>().SetHealth(playerNumber, 100);
            psScript.GetComponent<PlayerStatusUIScript>().SetHealthItem(playerNumber, healingItem);
        }
    }
}
