using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordsmanDragAttackScript : MonoBehaviour {

    const float Y_OFFSET = 1f;

    HashSet<GameObject> enemy = new HashSet<GameObject>();
    int damage = 1;
    

	// Use this for initialization
	void Start () {
        damage = transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().heavyAttackStrength;
    }

    void FixedUpdate()
    {
        foreach (GameObject target in enemy)
        {
            target.transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        }
    }

    public void ApplyDamageEffect()
    {
        if (enemy.Count > 0)
        {
            if (transform.parent.parent != null)
                transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.01f);
            //kickSparkEffect.GetComponent<ParticleSystem>().Play();
            foreach (GameObject target in enemy)
                target.GetComponent<Enemy>().Damage(transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().quickAttackStrength, .2f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
            if (!enemy.Contains(col.gameObject))
            {
                enemy.Add(col.gameObject);

                if (transform.parent.parent != null)
                    transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.01f);

                col.GetComponent<Enemy>().Damage(transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().quickAttackStrength, .2f);
            }

    }

    public void Reset()
    {
        enemy = new HashSet<GameObject>();
    }
}
