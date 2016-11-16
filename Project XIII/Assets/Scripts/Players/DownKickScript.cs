using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DownKickScript : MonoBehaviour {

    const float X_OFFSET = 2.4f;
    const float Y_OFFSET = -2.5f;

    public GameObject kickSparkEffect;

    HashSet<GameObject> enemy = new HashSet<GameObject>();
    int damage = 5;
    
    void Awake()
    {
        kickSparkEffect = Instantiate(kickSparkEffect);
        kickSparkEffect.transform.parent = transform.parent;
    }

    void FixedUpdate()
    {
        foreach(GameObject target in enemy)
        {
            float x_left_off = 0f;
            if (transform.parent.localScale.x < 0f)
                x_left_off = 1.5f;
            Vector2 legPos = new Vector2((transform.parent.position.x + X_OFFSET * transform.parent.localScale.x + x_left_off) , transform.parent.position.y + Y_OFFSET);
            target.transform.position = legPos;
        }
    }   

    public void ApplyDamageEffect()
    {
        if (enemy.Count > 0)
        {
            if (transform.parent.parent != null)
                transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.01f);
            kickSparkEffect.GetComponent<ParticleSystem>().Play();
            foreach (GameObject target in enemy)
                target.GetComponent<Enemy>().Damage(transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().heavyAirAttackStrengh, .2f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy"){
            if (!enemy.Contains(col.gameObject)){
                enemy.Add(col.gameObject);
                col.gameObject.layer = LayerMask.NameToLayer("Juggled Enemy");
            }
        }
    }

    public void Reset()
    {
        enemy = new HashSet<GameObject>();
    }

}
