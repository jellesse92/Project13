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

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public virtual void SetTargetPosition(float x, float y)
    {

    }

}
