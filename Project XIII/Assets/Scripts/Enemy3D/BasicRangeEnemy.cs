using UnityEngine;
using System.Collections;

public class BasicRangeEnemy : Enemy {

    Animator anim;

    //Projectile Variables
    public Transform projectileList;                        //Transform containing list of available bullets
    int ammo;                                        //Keeps track of ammo available
    public float fireRate;                                  //Rate at which enemy can fire
    public float bulletSpeed;                               //Speed at which bullet should move
    bool fireReady;                                         //Determines if able to fire bullet again


    // Use this for initialization
    void Start()
    {
        health = 100;
        anim = GetComponent<Animator>();
        fireReady = true;
        ammo = projectileList.childCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetVisibleState() && GetPursuitState())
        {
            if (!inAttackRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, GetTarget().transform.position, .01f);
                RunApproachAnim();
            }
            else
            {
                if(ammo != 0 && fireReady)
                {
                    FireProjectile();
                }
                //Should begin attack animation
                anim.SetInteger("x", 0);
                anim.SetInteger("y", 0);
                anim.SetBool("isIdle", true);
            }
        }
        else
            anim.SetBool("isIdle", true);
    }

    //Checks what animation to play
    void RunApproachAnim()
    {
        int x = 0;
        int y = 0;

        if (GetTarget().transform.position.y > transform.position.y)
            y = 1;
        else if (GetTarget().transform.position.y < transform.position.y)
            y = -1;
        else if (GetTarget().transform.position.x > transform.position.x)
            x = 1;
        else if (GetTarget().transform.position.x < transform.position.x)
            x = -1;

        if (y == 0 && x == 0)
        {
            anim.SetBool("isIdle", true);
        }
        else
        {
            anim.SetBool("isIdle", false);
        }

        anim.SetInteger("x", x);
        anim.SetInteger("y", y);

    }

    void FireProjectile()
    {
        ammo--;
        foreach(Transform child in projectileList)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.position = transform.position;
                child.GetComponent<Rigidbody2D>().velocity = -(transform.position - GetTarget().transform.position).normalized * bulletSpeed;
                StartCoroutine("FireRateRegulator");
                return;
            }
        }
    }

    //Regulates rate of fire
    IEnumerator FireRateRegulator()
    {
        fireReady = false;
        yield return new WaitForSeconds(fireRate);
        fireReady = true;
    }

    //Reloads ammo. Public for access by bullets to restock ammo
    public void ReloadAmmo()
    {
        ammo++;
    }
}
