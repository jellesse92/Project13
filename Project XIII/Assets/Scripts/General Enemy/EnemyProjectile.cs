using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    protected Enemy enemyScript;
    protected int damage = 10;

    public virtual void Start()
    {
        enemyScript = transform.parent.parent.GetComponentInChildren<Enemy>();
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public virtual void Fire(float x, float y){}

}
