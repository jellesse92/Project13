using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class to be inherited by all enemy scripts
public class Enemy : MonoBehaviour {

    const float END_PURSUIT_TIME = 1.3f;                //Time to end pursuit based on loss of sight
    const float ATTACK_DELAY_AFTER_STUN_TIME = .5f;     //Time delayed from attacking after having been stunned

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
    public int coinDropAmount;
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
    protected bool attackDelay = false;                 //Attack should be delayed irrelevant to projection time
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

    //Use in shakecam
    protected Camera mainCamera;

    public float magShakeDeath = 0.5f;
    public float durShakeDeath = 1.5f;
    // Use this for initialization
    void Awake()
    {
        frozen = false;
        isVisible = true;
        if(GetComponent<Animator>())
            anim = GetComponent<Animator>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
        layerMask = (LayerMask.GetMask("Default"));
        default_color = GetComponent<SpriteRenderer>().color;
        fullHealth = health;
        Reset();

        CreateCenterObject();
        ChangeCenter(transform.position);

        mainCamera = Camera.main;
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
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Detection Field")
        {
            target = col.transform.parent.gameObject;
            AddTargetList(target);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Detection Field")
            RemoveTargetList(collision.transform.parent.gameObject);
    }

    //Resets position and alert status
    public virtual void Reset()
    {

        if(anim && health <= 0)
            anim.SetTrigger("revive");

        CancelInvoke("RemoveTarget");
        isInvincible = false;
        dead = false;

        if(playersInView.Count <= 0)
        {
            target = null;
        }
        else{
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
    public virtual void Damage(int damage, float stunMultiplier = 0f, float xForce = 0, float yForce = 0)
    {
        if (!isInvincible)
        {
            //Will adjust this later for taking into account other particles to be played?
            //Possibly have a list of children with different responsive particles?

            if (dead)
                return;

            health -= damage;

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
                if (!isFrozen)
                    StartCoroutine("ApplyDamageColor");
            }

            if(xForce != 0 && yForce != 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(xForce, yForce, 0));
            }
        }
    }

    public virtual void SetPos(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    IEnumerator ApplyDamageColor()
    {
        GetComponent<SpriteRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(.075f);
        GetComponent<SpriteRenderer>().material.color = default_color;
    }

    public virtual void PlayDeath()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Death Particles")
                child.GetComponent<ParticleSystem>().Play();
            if (child.name == "Shadow")
                child.GetComponent<ShadowSpriteGenerator>().FadeShadow();
        }
        mainCamera.GetComponent<CamShakeScript>().StartShake(magShakeDeath, durShakeDeath);
        if(GetComponent<EnemyParticleEffects>())
            GetComponent<EnemyParticleEffects>().SpawnCoins(coinDropAmount);
        StopCoroutine("ApplyStun");
        if(anim)
            anim.SetTrigger("death");
        GetComponent<SpriteRenderer>().color = default_color;
        dead = true;
        gameObject.layer = 14;

        if (transform.parent != null && transform.parent.tag == "Fight Zone")
            transform.parent.GetComponent<FightZoneLockScript>().ReportDead();
    }

    //Run functions specific to enemy type that need to be reset for stuns
    public virtual void SpecificStunCancel(){}

    IEnumerator ApplyStun()
    {
        SpecificStunCancel();
        if(anim)
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
        CancelInvoke("EndAttackDelay");
        if(anim)
            anim.SetTrigger("stunRecovery");
        stunned = false;
        attackDelay = true;
        Invoke("EndAttackDelay", ATTACK_DELAY_AFTER_STUN_TIME);
    }

    void EndAttackDelay()
    {
        attackDelay = false;
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

    /*
     * TARGET DETECTION FUNCTIONS 
     */

    //Sets the target player
    public void SetTarget(GameObject tar)
    {
        target = tar;
    }
    
    public void AddTargetList(GameObject tar)
    {
        if (tar == target)
            CancelInvoke("RemoveTarget");
        if (playerList == null)
            playerList = tar.transform.parent.gameObject;
        if (!playersInView.Contains(tar))
            playersInView.Add(tar);
    }

    public void RemoveTargetList(GameObject tar)
    {
        if (playersInView.Contains(tar))
            playersInView.Remove(tar);
        if(target == tar)
        {
            if (playersInView.Count > 0)
            {
                foreach (GameObject newTarget in playersInView)
                {
                    target = newTarget;
                    break;
                }
            }
            else
            {
                if (!target.GetComponent<PlayerProperties>().alive)
                    target = null;
                else
                    Invoke("RemoveTarget", END_PURSUIT_TIME);
            }
        }
    }

    void RemoveTarget()
    {
        target = null;
    }

    /*
     * END TARGET DETECTION FUNCTIONS 
     */

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

    public virtual void Bounce(float forceY = 15000f)
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
