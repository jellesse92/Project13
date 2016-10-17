using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {

    private bool isPiercing = false;
    private bool isFriendlyFireOn = false;
    private int damageAmnt = 0;
    private int knockbackAmnt = 0;
    private float xOrigin = 0;
    
    void Awake()
    {
        xOrigin = transform.position.x;
        gameObject.SetActive(false);
    }

    void Update()
    {
    }

    public void SetKnockBackAmnt(int knockBack)
    {
        knockbackAmnt = knockBack;
    }

    public void SetDamageAmount(int dmg)
    {
        damageAmnt = dmg;
    }

    public void SetFriendlyFire(bool status)
    {
        isFriendlyFireOn = status;
    }

    void SetPiercing()
    {
        isPiercing = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(damageAmnt);
            gameObject.SetActive(false);
        } else if (col.collider.tag == "Player" && isFriendlyFireOn)
        {
            col.gameObject.GetComponent<Player>().TakeDamage(damageAmnt, knockbackAmnt);
            gameObject.SetActive(false);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	

}
