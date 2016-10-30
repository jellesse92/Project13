using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
    private bool isPiercing = false;
    private bool isFriendlyFireOn = false;
    private bool isDamageFading = false;
    private int maxDistance = 0;
    private int damageAmnt = 0;
    private float reduceBy = .15f;
    private int knockbackAmnt = 0;
    private float xOrigin = 0;
    
    void Awake()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        xOrigin = gameObject.transform.position.x;
    }
    void Update()
    {
        if(isDamageFading)
        {
            if(Mathf.Abs(transform.position.x - xOrigin) > .01f)
            {
                
                damageAmnt -= (int)(damageAmnt * reduceBy);

            }
        }
        
        if (maxDistance != 0 && Mathf.Abs(transform.position.x - xOrigin) >= maxDistance)
        {
            gameObject.SetActive(false);
        }
    }
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    public void DamageFadeActive(bool status, float reduceby = .15f)
    {
        isDamageFading = status;
        reduceBy = reduceby;
    }
    public void SetMaxDistance(int distance)
    {
        maxDistance = distance;
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
        bool isHit = false;
        if(col.collider.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(damageAmnt,0,4f);
            isHit = true;
        } else if (col.collider.tag == "Player" && isFriendlyFireOn)
        {
            col.gameObject.GetComponent<JazzPlayer>().TakeDamage(damageAmnt, knockbackAmnt);
            isHit = true;
        }
        if (!isPiercing && isHit)
        {
            gameObject.SetActive(false);
        }

    }
	// Use this for initialization
	void Start () {
       
    }
	
   
}
