using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

    public int HitPoints;
    public bool isDestroyed;
    public int BridgeFallTime;

    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    public void TakeDamage(int dmg)
    {
        anim.SetTrigger("wasHit");
        HitPoints -= dmg;
        if(HitPoints <= 0)
        {
            isDestroyed = true;
            anim.SetTrigger("wasDestroyed");
        }
    }

}
