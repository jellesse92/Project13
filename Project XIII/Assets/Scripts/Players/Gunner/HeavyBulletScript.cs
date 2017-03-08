using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBulletScript : MonoBehaviour {

    const float HEAVY_BULLET_STUN = .2f;

    public GameObject blastSplashZone;
    public SpriteRenderer bulletSprite;
   
    float dir = 1f;                                     //Get direction blast zone should splash
    int damage = 0;
    int splash_damage = 0;

	// Use this for initialization
	void Start () {
        bulletSprite.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        blastSplashZone.GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Enemy>().Damage(damage, HEAVY_BULLET_STUN);
        if (collision.CompareTag("Item"))
            collision.GetComponent<ItemHitTrigger>().ItemHit();
        Explode();
    }

    void Explode()
    {
        Debug.Log("EXPLODE PARTICLES");
        CancelInvoke("SelfDestruct");
        bulletSprite.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        blastSplashZone.GetComponent<HeavyBulletBlast>().ApplyDamage(splash_damage);
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    public void Initialize()
    {
        bulletSprite.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        blastSplashZone.GetComponent<Collider2D>().enabled = true;
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    public void SelfDestruct()
    {
        Debug.Log("SELF DESTRUCT PARTICLES");
        bulletSprite.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        blastSplashZone.GetComponent<Collider2D>().enabled = false;
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    public void SetDirection(float f)
    {
        dir = f;
        Transform temp = blastSplashZone.transform;
        temp.localScale = new Vector3(f, temp.position.y, temp.position.z);
    }

    public void SetDamage(int bulletDmg, int splashDmg)
    {
        damage = bulletDmg;
        splash_damage = splashDmg;
    }
}
