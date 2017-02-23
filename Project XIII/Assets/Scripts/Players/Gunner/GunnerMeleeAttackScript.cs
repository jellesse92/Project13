using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerMeleeAttackScript : MonoBehaviour {

    const float QUICK_AIR_FORCE_X = 23000;
    const float QUICK_AIR_FORCE_Y = 7000;
    const float QUICK_STUN_DURATION = .1f;

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


    // Update is called once per frame
        void Update () {
		
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
                damage = playerProp.GetPlayerStats().heavyAirAttackStrengh;
                break;
            default: attack = ""; break;
        }
    }

    void TriggerQuickAir(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(damage, QUICK_STUN_DURATION);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(QUICK_AIR_FORCE_X * transform.parent.localScale.x, QUICK_AIR_FORCE_Y));

            //Play particle effects here?
        }
    }

    void TriggerHeavyAir(Collider2D col)
    {

    } 
}
