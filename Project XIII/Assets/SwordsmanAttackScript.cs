using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanAttackScript : MonoBehaviour {

    //Constants for heavy attack variables 
    const float HEAVY_DRAG_ENEMY_TIME = .1f;                        //Time after initial enemy hit to keep dragging out attack
    const float HEAVY_X_LAUNCH_FORCE_MULTIPLIER = 18000f;           //Multiplier for how much to push enemy at the end of hit in the X direction
    const float HEAVY_Y_LAUNCH_FORCE_MULTIPLIER = 18000f;           //Multiplier for how much to push enemy at the end of hit in the Y direction
    const float HEAVY_X_OFFSET = 4f;                                //Where enemy is dragged relative to the swordsman
    


    HashSet<GameObject> enemyHash;

    //Heavy attack variables
    float forceMulti = 1f;                                          //Force based on how much swordsman charged attack

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
            default: break;
        }
    }

    void FixedUpdate()
    {
        switch (attack)
        {
            case "heavy": UpdateHeavyAttack(); break;
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
            default: break;
        }
    }

    public void Reset()
    {
        attack = "";
        enemyHash = new HashSet<GameObject>();
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
            target.GetComponent<Enemy>().Damage(damage, .1f);
        }
        transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.2f, .05f);
    }

    /*
     *  END HEAVY ATTACK FUNCTIONS
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

            col.GetComponent<Enemy>().Damage(damage, 1f);
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(400f * transform.parent.localScale.x, 5000f));
        }
    }

    /*
     *  END QUICK ATTACK FUNCTIONS
     */
}
