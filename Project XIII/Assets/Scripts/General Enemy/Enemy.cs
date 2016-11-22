using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class to be inherited by all enemy scripts
public class Enemy : MonoBehaviour {

    protected const int ALLOWED_BOUNCES = 3;            //Bounces allowed starting at 3 to allow 1 distinct bounce

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

    protected bool dead;                                //Determines if enemy is dead
    protected bool stunned;                             //Determines if enemy is stunned
    protected bool isFrozen = false;                    //Determines if enemy is debuff frozen
    protected bool frozen;                              //Determines if enemy is not meant to move
    float currentStunMultiplier;                        //Current stun time multiplier to be applied

    //Detection and Pursuit Variables
    protected bool isVisible;                           //Determine if enemy is visible on screen
    protected bool inPursuit;                           //Determine if enemy is in pursuit of player character
    protected GameObject target;                        //Target to chase

    //Attack Variables
    protected bool inAttackRange;                       //Detects if in range to begin attacking

    //Ground detection
    float distToGround;                                 //Distance from the ground
    int layerMask;                                      //Layers to check for ground

    //Juggling Variables
    protected bool isBouncing = false;                  //Determines if enemy is bouncing
    protected int bounceCount = 0;                      //Times bounced
    protected int comboCount = 0;                       //Number of times enemy has been hit in a consistent comb

    Color default_color;

    // Use this for initialization
    void Awake()
    {
        Reset();
        frozen = false;
        isVisible = true;
        anim = GetComponent<Animator>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
        layerMask = (LayerMask.GetMask("Default"));
        default_color = GetComponent<SpriteRenderer>().color;
        fullHealth = health;

    }

    // Call when enemy becomes visible on screen
    void OnBecameVisible()
    {
        isVisible = true;
    }

    //Call when enemy becomes no longer visible on screen
    void OnBecameInvisible()
    {
        /*
        isVisible = false;
        inPursuit = false;
        target = null;
        inAttackRange = false;
        */
    }

    //Call when enter a trigger field. If entering player trigger field and visible, activate pursuit status
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Detection Field")
        {
            inPursuit = true;
            target = col.transform.parent.gameObject;
        }
    }

    //Resets position and alert status
    public void Reset()
    {
        isVisible = false;
        inPursuit = false;
        target = null;
        inAttackRange = false;
        fullHealth = health;
        gameObject.layer = 9;
    }

    //Damage script to be applied when enemy takes damage
    public void Damage(int damage, float stunMultiplier = 0f)
    {
        transform.GetChild(2).GetComponent<ParticleSystem>().Play();

        if (dead)
            return;
        health -= damage;

        if(!isFrozen)
            StartCoroutine("ApplyDamageColor");

        if(health <= 0)
        {
            StopAllCoroutines();
            PlayDeath();
        }
        else if (stunnable && stunMultiplier > 0)
        {
            currentStunMultiplier = stunMultiplier;
            StopCoroutine(ApplyStun());
            StartCoroutine(ApplyStun());
        }
    }

    IEnumerator ApplyDamageColor()
    {
        GetComponent<SpriteRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(.075f);
        GetComponent<SpriteRenderer>().material.color = default_color;
    }

    void PlayDeath()
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
        if (Physics2D.Raycast(transform.position, -Vector3.up, distToGround - 1f, layerMask))
        {
            return true;
        }
        return false;
    }

    public void Bounce()
    {
        isBouncing = true;
        bounceCount = 0;
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

}
