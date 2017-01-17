using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanAttackScript : MonoBehaviour {

    //Constant for last combo hit damage additive
    const int FINISHER_DMG_BONUS = 5;

    //Constants for heavy attack variables 
    const float HEAVY_DRAG_ENEMY_TIME = .1f;                        //Time after initial enemy hit to keep dragging out attack
    const float HEAVY_X_LAUNCH_FORCE_MULTIPLIER = 18000f;           //Multiplier for how much to push enemy at the end of hit in the X direction
    const float HEAVY_Y_LAUNCH_FORCE_MULTIPLIER = 18000f;           //Multiplier for how much to push enemy at the end of hit in the Y direction
    const float HEAVY_X_OFFSET = 4f;                                //Where enemy is dragged relative to the swordsman

    //Constants for heavy air attack variables
    const float HEAVY_AIR_X_FORCE = 7000f;                          //X force to be applied on hit for air heavy attack
    const float HEAVY_AIR_Y_FORCE = -19000f;                        //Y force to be applied on hit for air heavy attack

    //Constants for quick attack variables
    const float QUICK_X_FORCE = 400f;                               //X force to be applied on hit for quick attack
    const float QUICK_Y_FORCE = 5000f;                              //Y force to be applied on hit for quick attack

    //Constants for quick air attack variables
    const float QUICK_AIR_X_FORCE = 400f;                           //X force to be applied on hit for air quick attack
    const float QUICK_AIR_Y_FORCE = 12000f;                         //Y force to be applied on hit for air quick attack

    //Constants for drag attack variables
    const float DRAG_REPEAT_DMG_APPLY_ST = .03f;                     //Time to start applying repeated damage as character performs drag attack
    const float DRAG_REPEAT_DMG_RATE = .1f;                         //Rate at which damage is applied after invoke started for drag attack
    const int DRAG_DAMAGE = 1;

    //Constants for stun duration
    const float HEAVY_STUN_MULTI = 1f;                              //Stun duration multiplier for heavy attack
    const float HEAVY_AIR_STUN_MULTI = 6f;                          //Stun duration multiplier for air heavy attack
    const float QUICK_STUN_MULTI = 1f;                              //Stun duration multiplier for quick atack
    const float QUICK_AIR_STUN_MULTI = 1f;                          //Stun duration multiplier for air quick attack 
    const float DRAG_STUN_MULTI = 1f;                               //Stun duration multiplier for drag attack
    
    HashSet<GameObject> enemyHash;

    //Heavy attack variables
    float forceMulti = 1f;                                          //Force based on how much swordsman charged attack

    //Combo finisher variable
    bool finishEffectPlayed = false;                                //Finisher effect has been played

    string attack = "";
    int damage;

    PlayerSoundEffects playerSoundEffects;
    PlayerParticleEffects playerParticleEffects;
    PlayerProperties playProp;

    void Awake()
    {
        enemyHash = new HashSet<GameObject>();
    }

    void Start()
    {
        playerSoundEffects = transform.parent.GetComponent<PlayerSoundEffects>();
        playerParticleEffects = transform.parent.GetComponent<PlayerParticleEffects>();
        playProp = transform.parent.GetComponent<PlayerProperties>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (attack)
        {
            case "heavy": TriggerHeavyAttack(col); break;
            case "quick": TriggerQuickAttack(col); break;
            case "quickAir": TriggerAirQuickAttack(col); break;
            case "heavyAir": TriggerAirHeavyAttack(col); break;
            case "drag": TriggerDragAttack(col);  break;
            case "finisher": TriggerFinisherAttack(col); break;
            default: break;
        }
    }

    void FixedUpdate()
    {
        switch (attack)
        {
            case "heavy": UpdateHeavyAttack(); break;
            case "quick": break;
            case "quickAir": break;
            case "heavyAir": break;
            case "drag": UpdateDragAttack(); break;
            case "finisher": break;
            default: break;
        }
    }

    public void SetAttackType(string type)
    {
        attack = type;

        switch (attack)
        {
            case ("heavy"): damage = playProp.GetPlayerStats().heavyAttackStrength; break;
            case ("quick"): damage = playProp.GetPlayerStats().quickAttackStrength; break;
            case ("quickAir"): damage = playProp.GetPlayerStats().quickAirAttackStrength; break;
            case ("heavyAir"): damage = playProp.GetPlayerStats().heavyAirAttackStrengh; break;
            case ("drag"):
                damage = playProp.GetPlayerStats().quickAttackStrength;
                InvokeRepeating("DragAttackApplyDamage", DRAG_REPEAT_DMG_APPLY_ST, DRAG_REPEAT_DMG_RATE);
                break;
            case "finisher":
                damage = playProp.GetPlayerStats().quickAttackStrength + FINISHER_DMG_BONUS;
                finishEffectPlayed = false;
                break;
            default: attack = ""; break;
        }
    }

    public void Reset()
    {
        attack = "";
        enemyHash = new HashSet<GameObject>();
    }

    public void ResetDrag()
    {
        Reset();
        CancelInvoke("DragAttackApplyDamage");
    }

    /*
     *  HEAVY ATTACK FUNCTIONS
     */
      
    void TriggerHeavyAttack(Collider2D col)
    {
        if (col.tag == "Enemy" && !enemyHash.Contains(col.gameObject))
        {
            enemyHash.Add(col.gameObject);
            Invoke("StopMomentum", HEAVY_DRAG_ENEMY_TIME);
        }
    }

    void UpdateHeavyAttack()
    {
        foreach (GameObject target in enemyHash)
        {
            target.transform.position = new Vector3(transform.position.x + HEAVY_X_OFFSET * transform.parent.localScale.x, target.transform.position.y, target.transform.position.z);

            //HEAVY ATTACK PARTICLE STUFF
            playerParticleEffects.PlayHitSpark(target.GetComponent<Enemy>().GetCenter());
            playerSoundEffects.PlayHitSpark();

            target.GetComponent<Enemy>().Damage(0, .2f);
        }
    }

    void StopMomentum()
    {
        transform.parent.GetComponent<PlayerPhysics>().VelocityX(0);
    }

    public void SetForceMulti(float force)
    {
        forceMulti = force;
    }

    public void Launch()
    {
        foreach (GameObject target in enemyHash)
        {
            target.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceMulti * HEAVY_X_LAUNCH_FORCE_MULTIPLIER * transform.parent.localScale.x, forceMulti * HEAVY_Y_LAUNCH_FORCE_MULTIPLIER));
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(target.GetComponent<Enemy>().GetCenter());
            target.GetComponent<Enemy>().Damage(damage, HEAVY_STUN_MULTI);
        }
        transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.2f, .05f);
    }

    /*
     *  END HEAVY ATTACK FUNCTIONS
     */

    /*
     *  HEAVY AIR ATTACK FUNCTIONS
     */

    void TriggerAirHeavyAttack(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            //HEAVY ATTACK AIR EFFECTS STUFF
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(col.GetComponent<Enemy>().GetCenter());

            col.gameObject.GetComponent<Enemy>().Damage(damage, HEAVY_AIR_STUN_MULTI);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(HEAVY_AIR_X_FORCE * transform.parent.localScale.x, HEAVY_AIR_Y_FORCE));
        }
    }

    /*
     *  END AIR HEAVY ATTACK FUNCTIONS
     */

    /*
     *  QUICK ATTACK FUNCTIONS
     */

    void TriggerQuickAttack(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            //QUICK ATTACK EFFECTS STUFF
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(col.GetComponent<Enemy>().GetCenter());

            col.GetComponent<Enemy>().Damage(damage, QUICK_STUN_MULTI);
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(QUICK_X_FORCE * transform.parent.localScale.x, QUICK_Y_FORCE));
        }
    }

    void TriggerAirQuickAttack(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            //QUICK ATTACK AIR EFFECTS STUFF
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(col.GetComponent<Enemy>().GetCenter());

            col.GetComponent<Enemy>().Damage(damage, QUICK_AIR_STUN_MULTI);
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(QUICK_AIR_X_FORCE * transform.parent.localScale.x, QUICK_AIR_Y_FORCE));
        }
    }

    /*
     *  END QUICK ATTACK FUNCTIONS
     */

    /*
     *  DRAG ATTACK FUNCTIONS
     */

    void TriggerDragAttack(Collider2D col)
    {
        if (col.tag == "Enemy")
            if (!enemyHash.Contains(col.gameObject))
            {
                //DRAG ATTACK EFFECTS STUFF
                playerSoundEffects.PlayHitSpark();
                playerParticleEffects.PlayHitSpark(col.GetComponent<Enemy>().GetCenter());

                enemyHash.Add(col.gameObject);
                if (transform.parent.parent != null)
                    transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.01f);
                col.GetComponent<Enemy>().Damage(damage, DRAG_STUN_MULTI);
            }
    }

    void UpdateDragAttack()
    {
        foreach (GameObject target in enemyHash)
            target.transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
    }

    void DragAttackApplyDamage()
    {
        foreach(GameObject target in enemyHash)
        {
            //DRAG ATTACK EFFECTS STUFF
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(target.GetComponent<Enemy>().GetCenter());

            target.GetComponent<Enemy>().Damage(DRAG_DAMAGE, DRAG_STUN_MULTI);
        }
    }

    public void CancelDragAttackApplyDamage()
    {
        CancelInvoke("DragAttackApplyDamage");
    }


    /*
     *  END DRAG ATTACK FUNCTIONS
     */

    /*
     *  BEGIN FINISHER FUNCTIONS
     */

    void TriggerFinisherAttack(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            //HEAVY ATTACK EFFECTS STUFF
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(col.GetComponent<Enemy>().GetCenter());

            if (!finishEffectPlayed)
            {
                finishEffectPlayed = true;
                transform.parent.parent.GetComponent<PlayerEffectsManager>().FlashScreen();
                transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(0.2f);

            }

            col.GetComponent<Enemy>().Damage(damage, QUICK_STUN_MULTI);
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(QUICK_X_FORCE * transform.parent.localScale.x, QUICK_Y_FORCE));

        }
    }

    /*
     *  END FINISHER FUNCTIONS
     */

}
