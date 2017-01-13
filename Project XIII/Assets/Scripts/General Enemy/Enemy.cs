using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class to be inherited by all enemy scripts
public class Enemy : MonoBehaviour {

    HashSet<GameObject> playersInView = new HashSet<GameObject>();

    protected GameObject centerObjectPosition;          //use to get center of enemy, might be different for each child

    protected const int ALLOWED_BOUNCES = 2;            //Bounces allowed starting at 3 to allow 1 distinct bounce
    const float AIR_TO_GROUND_STUN_RECOVERY_TIME = .2f; //Time it takes to recover from stun after being in a stun state in the air as a non-flying enemy

    public float groundedOffset = 1f;                   //Offset for ground checking

    //Animator
    protected Animator anim;

    //In-Game information                 
    public int health;                                  //Enemy health
    int fullHealth;                                     //Health at full health to restore to
    public float speed;                                 //Speed of enemy
    public int attackPower;                             //Base attack power of enemy
    public bool stunnable;                              //Able to be stunned
    public float stunEffectiveness;                     //Effectiveness of stun
    public bool grounded;                               //Checks if enemy is on the ground
    public bool flyingEnemy = false;                    //Determines if is a flying enemy or not for stun recovery to take place only on the ground 

    protected bool dead;                                //Determines if enemy is dead
    protected bool stunned;                             //Determines if enemy is stunned
    protected bool isFrozen = false;                    //Determines if enemy is debuff frozen
    protected bool frozen;                              //Determines if enemy is not meant to move
    float currentStunMultiplier;                        //Current stun time multiplier to be applied

    //Detection and Pursuit Variables
    protected bool isVisible;                           //Determine if enemy is visible on screen
    protected bool inPursuit;                           //Determine if enemy is in pursuit of player character
    protected GameObject target;                        //Target to chase
    protected GameObject playerList;                    //List of players able to be targted

    //Attack Variables
    protected bool inAttackRange;                       //Detects if in range to begin attacking

    //Damage Variables
    protected bool isInvincible = false;                //Determine if enemy is invincible when attacked;

    //Ground detection
    float distToGround;                                 //Distance from the ground
    int layerMask;                                      //Layers to check for ground

    //Juggling Variables
    protected bool isBouncing = false;                  //Determines if enemy is bouncing
    protected bool isSquishing = false;                 //Determines if enemy is being squished to ground
    protected int bounceCount = 0;                      //Times bounced
    protected float bounceForce = 0f;                   //Force which enemy should be lifted during bounce
    protected int comboCount = 0;                       //Number of times enemy has been hit in a consistent combo

    //Check if grounded after being knocked in air for stun recover
    private bool waitForRemoveStunLand = false;

    Color default_color;
    protected int currentAmmo = 0;                      //Ammo count for enemies with ammo

    // Use this for initialization
    void Awake()
    {
        frozen = false;
        isVisible = true;
        anim = GetComponent<Animator>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
        layerMask = (LayerMask.GetMask("Default"));
        default_color = GetComponent<SpriteRenderer>().color;
        fullHealth = health;
        Reset();

        CreateCenterObject();
        ChangeCenter(transform.position);
    }

    void CreateCenterObject()
    {
        centerObjectPosition = new GameObject();
        centerObjectPosition.name = "Center Position";
        centerObjectPosition.transform.parent = transform;
    }

    public Vector3 GetCenter() //Use by hitspark
    {
        return centerObjectPosition.transform.position;
    }

    protected void ChangeCenter(Vector3 position) //Use to change position of center, put all things that needs the center here. Mostly use for child
    {
        centerObjectPosition.transform.position = position;
    }

    public virtual void FixedUpdate()
    {
        if (waitForRemoveStunLand && IsGrounded())
        {
            waitForRemoveStunLand = false;
            Invoke("RecoverFromStun", AIR_TO_GROUND_STUN_RECOVERY_TIME);

        }
    }

    //Call when enter a trigger field. If entering player trigger field and visible, activate pursuit status
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Detection Field")
        {
            inPursuit = true;
            target = col.transform.parent.gameObject;

            if (!playersInView.Contains(target))
                playersInView.Add(target);

            if (playerList == null)
                playerList = target.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Detection Field")
        {
            if (playersInView.Contains(collision.transform.parent.gameObject))
                playersInView.Remove(collision.transform.parent.gameObject);
        }
    }

    //Resets position and alert status
    public virtual void Reset()
    {
        if(health < 0)
            anim.SetTrigger("revive");

        isInvincible = false;

        if(playersInView.Count <= 0)
        {
            inPursuit = false;
            target = null;
        }
        else{
            inPursuit = true;
            target = null;
            foreach(GameObject player in playersInView)
            {
                target = player;
                break;
            }
        }

        GetComponent<SpriteRenderer>().color = default_color;
        health = fullHealth;
        gameObject.layer = 9;


    }

    //Damage script to be applied when enemy takes damage
    public void Damage(int damage, float stunMultiplier = 0f)
    {
        if (!isInvincible)
        {
            //Will adjust this later for taking into account other particles to be played?
            //Possibly have a list of children with different responsive particles?

            if (dead)
                return;
            health -= damage;

            if (!isFrozen)
                StartCoroutine("ApplyDamageColor");

            if (health <= 0)
            {
                StopAllCoroutines();
                GetComponent<SpriteRenderer>().color = default_color;
                PlayDeath();
            }
            else if (stunnable && stunMultiplier > 0)
            {
                currentStunMultiplier = stunMultiplier;
                StopCoroutine(ApplyStun());
                StartCoroutine(ApplyStun());
            }
        }
    }

    IEnumerator ApplyDamageColor()
    {
        GetComponent<SpriteRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(.075f);
        GetComponent<SpriteRenderer>().material.color = default_color;
    }

    public virtual void PlayDeath()
    {
        StopCoroutine("ApplyStun");
        anim.SetTrigger("death");
        dead = true;
        gameObject.layer = 14;

        if (transform.parent != null && transform.parent.tag == "Fight Zone")
            transform.parent.GetComponent<FightZoneLockScript>().ReportDead();
    }

    IEnumerator ApplyStun()
    {
        anim.SetTrigger("stun");
        stunned = true;
        yield return new WaitForSeconds(currentStunMultiplier * stunEffectiveness);

        if (!flyingEnemy && !IsGrounded())
            waitForRemoveStunLand = true;
        else
            RecoverFromStun();
    }

    void RecoverFromStun()
    {
        anim.SetTrigger("stunRecovery");
        stunned = false;
    }

    //Gets if enemy is visible or not
    public bool GetVisibleState()
    {
        return isVisible;
    }

    //Get the info whether enemy is in pursuit or not
    public bool GetPursuitState()
    {
        return inPursuit;
    }

    //Sets whether or not enemy is in pursuit
    public void SetPursuitState(bool state)
    {
        inPursuit = state;
    }

    //Gets the character currently targeted
    public GameObject GetTarget()
    {
        return target;
    }

    //Gets a new target from the playerList
    public GameObject GetRandomTarget()
    {
        GameObject[] liveList = new GameObject[4];
        int liveCount = 0;

        foreach(Transform child in playerList.transform)
        {
            if (child.gameObject.activeSelf && child.GetComponent<PlayerProperties>().alive)
            {
                liveList[liveCount] = child.gameObject;
                liveCount++;
            }
        }

        if (playerList != null && liveCount != 0)
        {
            return liveList[(int)Random.Range(0, liveCount - 1)];
        }

        return null;
    }
    
    //Sets the target player
    public void SetTarget(GameObject tar)
    {
        target = tar;
    }
    

    //Sets if enemy is in attack range
    public void SetAttackInRange(bool b)
    {
        inAttackRange = b;
    }

    public void SetFrozenState(bool b)
    {
        frozen = b;
    }

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, -Vector3.up, distToGround - groundedOffset, layerMask))
        {
            return true;
        }
        return false;
    }

    public void Bounce(float forceY = 15000f)
    {
        isBouncing = true;
        isSquishing = false;
        bounceCount = 0;
        bounceForce = forceY;
    }

    /*
     *  DEBUFFS
     */ 

    public IEnumerator ApplyDebuffFreeze(float duration)
    {
        GetComponent<SpriteRenderer>().material.color = new Color(.5f, .8f, 1f,1f);
        isFrozen = true;
        yield return new WaitForSeconds(duration);
        GetComponent<SpriteRenderer>().material.color = default_color;
        isFrozen = false;
    }

    public bool GetStunStatus()
    {
        return stunned;
    }

    public void ReloadAmmo()
    {
        currentAmmo++;
    }

}
