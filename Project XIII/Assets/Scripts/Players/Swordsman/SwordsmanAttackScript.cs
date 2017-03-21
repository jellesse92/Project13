using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanAttackScript : MonoBehaviour {

    //Constant for last combo hit damage additive
    const int FINISHER_DMG_BONUS = 2;
    const int QUICK_2_DMG_BONUS = 5;

    //Constants for heavy attack variables 
    const float HEAVY_DRAG_ENEMY_TIME = .1f;                        //Time after initial enemy hit to keep dragging out attack
    const float HEAVY_X_LAUNCH_FORCE = 18000f;                      //Launce force for heavy attack
    const float HEAVY_Y_LAUNCH_FORCE = 30000f;
    const float HEAVY_X_OFFSET = 4f;                                //Where enemy is dragged relative to the swordsman
    const int HEAVY_FINISHER_DPH = 15;                               //Heavy finisher's damage per hit

    //Constants for heavy air attack variables
    const float HEAVY_AIR_X_FORCE = 9000f;                          //X force to be applied on hit for air heavy attack
    const float HEAVY_AIR_Y_FORCE = -24000f;                        //Y force to be applied on hit for air heavy attack

    //Constants for quick attack variables
    const float QUICK_X_FORCE = 400f;                               //X force to be applied on hit for quick attack
    const float QUICK_Y_FORCE = 5000f;                              //Y force to be applied on hit for quick attack

    //Constants for quick air attack variables
    const float QUICK_AIR_X_FORCE = 400f;                           //X force to be applied on hit for air quick attack
    const float QUICK_AIR_Y_FORCE = 12000f;                         //Y force to be applied on hit for air quick attack

    //Constants for drag attack variables
    const float DRAG_REPEAT_DMG_APPLY_ST = .03f;                     //Time to start applying repeated damage as character performs drag attack
    const float DRAG_REPEAT_DMG_RATE = .1f;                         //Rate at which damage is applied after invoke started for drag attack
    const float DRAG_ATTACK_END_FORCE = 3000f;                      //Amount of force to apply at end of drag attack
    const int DRAG_DAMAGE = 1;

    //Constants for stun duration
    const float HEAVY_STUN_MULTI = 2f;                              //Stun duration multiplier for heavy attack
    const float HEAVY_AIR_STUN_MULTI = 6f;                          //Stun duration multiplier for air heavy attack
    const float QUICK_STUN_MULTI = 1f;                              //Stun duration multiplier for quick atack
    const float QUICK_AIR_STUN_MULTI = 1f;                          //Stun duration multiplier for air quick attack 
    const float DRAG_STUN_MULTI = 1f;                               //Stun duration multiplier for drag attack

    HashSet<GameObject> enemyHash;                                  //Hash of enemies that should be hit

    //Heavy attack variables
    float forceMulti = 1f;                                          //Force based on how much swordsman charged attack
    bool stopInvoked = false;
    HashSet<GameObject> enemyInHeavyFinisher = new HashSet<GameObject>();
    int heavyTier = 0;

    //Combo finisher variable
    bool finishEffectPlayed = false;                                //Finisher effect has been played

    string attack = "";
    int damage;

    enum HitType {normal, finisher, item};
    PlayerSoundEffects playerSoundEffects;
    SwordsmanParticleEffects playerParticleEffects;
    PlayerProperties playProp;

    public float magShakeLaunch = 1.5f;
    public float durShakeLaunch = 0.7f;
    public float magShakeDragAttack = 1.5f;
    public float durShakeDragAttack = 0.7f;
    public float magShakefinisherAttack = 1.5f;
    public float durShakefinisherAttack = 1.5f;

    void Awake()
    {
        enemyHash = new HashSet<GameObject>();
    }

    void Start()
    {
        playerSoundEffects = transform.parent.GetComponent<SwordsmanSoundEffects>();
        playerParticleEffects = transform.parent.GetComponent<SwordsmanParticleEffects>();
        playProp = transform.parent.GetComponent<PlayerProperties>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (attack)
        {
            case "heavy": TriggerHeavyAttack(col); break;
            case "quick": TriggerQuickAttack(col); break;
            case ("quick2"): TriggerQuickAttack2(col); break;
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
            case "quick2": break;
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
            case ("heavy"):
                damage = playProp.GetPlayerStats().heavyAttackStrength;
                stopInvoked = false;
                break;
            case ("quick"): damage = playProp.GetPlayerStats().quickAttackStrength; break;
            case ("quickAir"): damage = playProp.GetPlayerStats().quickAirAttackStrength; break;
            case ("heavyAir"): damage = playProp.GetPlayerStats().heavyAirAttackStrengh; break;
            case ("drag"):
                damage = playProp.GetPlayerStats().quickAttackStrength;
                InvokeRepeating("DragAttackApplyDamage", DRAG_REPEAT_DMG_APPLY_ST, DRAG_REPEAT_DMG_RATE);
                break;
            case ("quick2"):
                damage = playProp.GetPlayerStats().quickAttackStrength + QUICK_2_DMG_BONUS;
                //Effect stuff??
                break;
            case "finisher":
                damage = playProp.GetPlayerStats().quickAttackStrength * FINISHER_DMG_BONUS;
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

    bool CheckAcceptTag(Collider2D col)
    {
        return (col.CompareTag("Enemy") || col.CompareTag("Item") || col.CompareTag("Destructible Projectile"));    }

    /*
     *  HEAVY ATTACK FUNCTIONS
     */
      
    void TriggerHeavyAttack(Collider2D col)
    {
        if (CheckAcceptTag(col) && !enemyHash.Contains(col.gameObject))
        {
            enemyHash.Add(col.gameObject);
            if (col.tag != "Destructible Projectile" && !stopInvoked)
            {
                stopInvoked = true;
                Invoke("StopMomentum", HEAVY_DRAG_ENEMY_TIME);
            }
        }

        OtherHitsManage(col);
    }

    void UpdateHeavyAttack()
    {
        foreach (GameObject target in enemyHash)
        {
            if (target.CompareTag("Enemy"))
            {
                target.transform.position = new Vector3(transform.position.x + HEAVY_X_OFFSET * transform.parent.localScale.x, target.transform.position.y, target.transform.position.z);
                target.GetComponent<Enemy>().Damage(0, .2f);
            }

        }
    }

    void StopMomentum()
    {
        transform.parent.GetComponent<PlayerPhysics>().VelocityX(0);
    }

    public void SetHeavyTier(int tier)
    {
        heavyTier = tier;
    }

    public void Launch()
    {
        foreach (GameObject target in enemyHash)
        {
            if (target.CompareTag("Enemy"))
            {
                HitEffect(HitType.normal, target.GetComponent<Enemy>().GetCenter());
                target.GetComponent<Enemy>().Damage(damage, HEAVY_STUN_MULTI,HEAVY_X_LAUNCH_FORCE * transform.parent.localScale.x, HEAVY_Y_LAUNCH_FORCE);
            }
        }

        enemyInHeavyFinisher = enemyHash;

        if (heavyTier > 0)
        {
            InvokeRepeating("HeavyFinisher", .05f, .1f);
            if (heavyTier == 1)
                Invoke("EndHeavyFinisher", .35f);
            else if (heavyTier == 2)
                Invoke("EndHeavyFinisher", .65f);
        }

        transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(magShakeLaunch, durShakeLaunch);
    }

    void EndHeavyFinisher()
    {
        CancelInvoke("HeavyFinisher");
        foreach(GameObject target in enemyInHeavyFinisher)
        {
            if (target.CompareTag("Enemy"))
                target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void HeavyFinisher()
    {
        foreach(GameObject target in enemyInHeavyFinisher)
        {
            HeavyFinisherDamage(target);
        }
    }

    void HeavyFinisherDamage(GameObject target)
    {
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<Enemy>().Damage(HEAVY_FINISHER_DPH, 1f,0,3000f);
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            //Special effects stuff
            HitEffect(HitType.normal, target.GetComponent<Enemy>().GetCenter());
        }
        else
            OtherHitsManage(target.GetComponent<Collider2D>());


    }

    /*
     *  END HEAVY ATTACK FUNCTIONS
     */

    /*
     *  HEAVY AIR ATTACK FUNCTIONS
     */

    void TriggerAirHeavyAttack(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //HEAVY ATTACK AIR EFFECTS STUFF
            HitEffect(HitType.normal, col.GetComponent<Enemy>().GetCenter());

            col.gameObject.GetComponent<Enemy>().Damage(damage, HEAVY_AIR_STUN_MULTI,HEAVY_AIR_X_FORCE * transform.parent.localScale.x, HEAVY_AIR_Y_FORCE);
        }
        OtherHitsManage(col);
    }

    /*
     *  END AIR HEAVY ATTACK FUNCTIONS
     */

    /*
     *  QUICK ATTACK FUNCTIONS
     */

    void TriggerQuickAttack(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //QUICK ATTACK EFFECTS STUFF
            HitEffect(HitType.normal, col.GetComponent<Enemy>().GetCenter());

            col.GetComponent<Enemy>().Damage(damage, QUICK_STUN_MULTI, QUICK_X_FORCE * transform.parent.localScale.x, QUICK_Y_FORCE);
        }
        OtherHitsManage(col);
    }

    void TriggerQuickAttack2(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //QUICK ATTACK 2 EFFECTS STUFF
            HitEffect(HitType.normal, col.GetComponent<Enemy>().GetCenter());

            col.GetComponent<Enemy>().Damage(damage, QUICK_STUN_MULTI, QUICK_X_FORCE * transform.parent.localScale.x, QUICK_Y_FORCE);
        }
        OtherHitsManage(col);
    }

    void TriggerAirQuickAttack(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //QUICK ATTACK AIR EFFECTS STUFF
            HitEffect(HitType.normal, col.GetComponent<Enemy>().GetCenter());
            
            col.GetComponent<Enemy>().Damage(damage, QUICK_AIR_STUN_MULTI, QUICK_AIR_X_FORCE * transform.parent.localScale.x, QUICK_AIR_Y_FORCE);
        }
        OtherHitsManage(col);
    }

    /*
     *  END QUICK ATTACK FUNCTIONS
     */

    /*
     *  DRAG ATTACK FUNCTIONS
     */

    void TriggerDragAttack(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
            if (!enemyHash.Contains(col.gameObject))
            {
                //DRAG ATTACK EFFECTS STUFF
                HitEffect(HitType.normal, col.GetComponent<Enemy>().GetCenter());

                enemyHash.Add(col.gameObject);
                if (transform.parent.parent != null)
                    transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(magShakeDragAttack, durShakeDragAttack);
                col.GetComponent<Enemy>().Damage(damage, DRAG_STUN_MULTI);
            }
        OtherHitsManage(col);
    }

    void UpdateDragAttack()
    {
        foreach (GameObject target in enemyHash)
            if (target.CompareTag("Enemy"))
                target.GetComponent<Enemy>().SetPos(target.transform.position.x, target.transform.position.y);
    }

    void DragAttackApplyDamage()
    {
        foreach(GameObject target in enemyHash)
        {
            if (target.CompareTag("Enemy"))
            {
                //DRAG ATTACK EFFECTS STUFF
                HitEffect(HitType.normal, target.GetComponent<Enemy>().GetCenter());

                target.GetComponent<Enemy>().Damage(DRAG_DAMAGE, DRAG_STUN_MULTI);
            }
        }
    }

    public void CancelDragAttackApplyDamage()
    {
        CancelInvoke("DragAttackApplyDamage");
    }

    public void DragAttackEnd()
    {
        foreach(GameObject target in enemyHash)
            if (target.CompareTag("Enemy"))
                if (target.GetComponent<Rigidbody2D>())
                    target.GetComponent<Enemy>().Damage(0, 0, 0, DRAG_ATTACK_END_FORCE);
    }

    /*
     *  END DRAG ATTACK FUNCTIONS
     */

    /*
     *  BEGIN FINISHER FUNCTIONS
     */

    void TriggerFinisherAttack(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //HEAVY ATTACK EFFECTS STUFF
            //Debug.Log("attack");

            HitEffect(HitType.finisher, col.GetComponent<Enemy>().GetCenter());
            transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(magShakefinisherAttack, durShakefinisherAttack);
            transform.parent.parent.GetComponent<PlayerEffectsManager>().FlashScreen();

            col.GetComponent<Enemy>().Damage(damage, QUICK_STUN_MULTI, QUICK_X_FORCE * transform.parent.localScale.x, QUICK_Y_FORCE);
        }
        OtherHitsManage(col);        
    }



    void HitEffect(HitType hitType, Vector3 position)
    {
        if(hitType == HitType.normal)
        {
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(position);
        }
        else if(hitType == HitType.finisher)
        {
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayFinisherHitSpark(position);
        }
        else if(hitType == HitType.item)
        {
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(position);
        }

    }

    void OtherHitsManage(Collider2D collider)
    {
        ItemHit(collider);
        ProjectileHit(collider);
    }

    void ItemHit(Collider2D collider)
    {
        if(collider.CompareTag("Item"))
        {
            HitEffect(HitType.item, collider.transform.position);
            collider.GetComponent<ItemHitTrigger>().ItemHit();
        }
    }

    void ProjectileHit(Collider2D collider)
    {
        if(collider.CompareTag("Destructible Projectile"))
        {
            if(collider.GetComponent<EnemyBulletScript>())
                collider.GetComponent<EnemyBulletScript>().Destroy();
        }
    }

    /*
     *  END FINISHER FUNCTIONS
     */

}
