using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerMeleeAttackScript : MonoBehaviour {

    //Constants for quick air attack
    const float QUICK_AIR_FORCE_X = 23000;
    const float QUICK_AIR_FORCE_Y = 7000;
    const float QUICK_STUN_DURATION = .1f;

    //Constants for heavy air attack
    const float X_OFFSET = 2.4f;
    const float Y_OFFSET = -4f;

    PlayerProperties playerProp;

    HashSet<GameObject> enemyHash = new HashSet<GameObject>();

    int damage = 0;
    string attack = "";

	// Use this for initialization
	void Start () {
        playerProp = transform.parent.GetComponent<PlayerProperties>();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (attack)
        {
            case "quickAir": TriggerQuickAir(col); break;
            case "heavyAir": TriggerHeavyAir(col); break;
            default: break;
        }
    }

    void FixedUpdate () {
        switch (attack)
        {
            case "quickAir": break;
            case "heavyAir": UpdateHeavyAir();  break;
            default: break;
        }
	}

    public void Reset()
    {
        attack = "";
    }

    public void SetAttackType(string type)
    {
        attack = type;

        switch (type)
        {
            case "quickAir":
                damage = playerProp.GetPlayerStats().quickAirAttackStrength;
                break;
            case "heavyAir":
                enemyHash = new HashSet<GameObject>();
                this.enabled = true;
                damage = playerProp.GetPlayerStats().heavyAirAttackStrengh;
                break;
            default: attack = ""; break;
        }
    }

    void TriggerQuickAir(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(damage, QUICK_STUN_DURATION, QUICK_AIR_FORCE_X * transform.parent.localScale.x, QUICK_AIR_FORCE_Y);
            //Play particle effects here?
        }
    }

    //BEGIN HEAVY AIR ATTACK FUNCTIONS

    void TriggerHeavyAir(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            Debug.Log("test");
            if (!enemyHash.Contains(col.gameObject))
            {
                enemyHash.Add(col.gameObject);
                col.gameObject.GetComponent<Enemy>().Bounce();
            }
        }
    } 

    void UpdateHeavyAir()
    {
        foreach (GameObject target in enemyHash)
        {
            float x_left_off = 0f;
            if (transform.parent.localScale.x < 0f)
                x_left_off = 1.5f;
            Vector2 legPos = new Vector2((transform.parent.position.x + X_OFFSET * transform.parent.localScale.x + x_left_off), transform.parent.position.y + Y_OFFSET);
            target.transform.position = legPos;
            Debug.Log("Test");
        }
    }

    public void ApplyDamageEffect()
    {
        if (enemyHash.Count > 0)
        {
            if (transform.parent.parent != null)
                transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(1f);

            //Play particle effects

            foreach (GameObject target in enemyHash)
                target.GetComponent<Enemy>().Damage(damage, .2f);
        }
    }

    public void ApplyBounce()
    {
        foreach (GameObject target in enemyHash)
        {
            target.GetComponent<Enemy>().Bounce();
        }
    }

    //END HEAVY AIR ATTACK FUNCTIONS
}
