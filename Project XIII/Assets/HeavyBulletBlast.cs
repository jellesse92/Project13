using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBulletBlast : MonoBehaviour {

    const int BLAST_DAMAGE = 10;
    const float BLAST_STUN = .2f;

    HashSet<GameObject> enemyHash = new HashSet<GameObject>();

    public GameObject TEMP_SPRITE;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !enemyHash.Contains(collision.gameObject))
            enemyHash.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && enemyHash.Contains(collision.gameObject))
            enemyHash.Remove(collision.gameObject);
    }

    public void Reset()
    {
        enemyHash = new HashSet<GameObject>();
    }

    public void ApplyDamage(int damage)
    {
        Invoke("TEMP_INVOKE", .1f);
        TEMP_SPRITE.SetActive(true);
        foreach(GameObject target in enemyHash)
        {
            if (target.tag == "Enemy")
                target.GetComponent<Enemy>().Damage(damage, BLAST_STUN);
        }
        GetComponent<Collider2D>().enabled = false;
    }

    void TEMP_INVOKE()
    {
        TEMP_SPRITE.SetActive(false);
    }
}
