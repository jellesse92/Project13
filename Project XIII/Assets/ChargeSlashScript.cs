using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChargeSlashScript : MonoBehaviour {

    const float DRAG_ENEMY_TIME = .1f;                      //Time after initial enemy hit to keep dragging out attack
    const float X_LAUNCH_FORCE_MULTIPLIER = 18000f;          //Multiplier for how much to push enemy at the end of hit in the X direction
    const float Y_LAUNCH_FORCE_MULTIPLIER = 18000f;          //Multiplier for how much to push enemy at the end of hit in the Y direction

    const float X_OFFSET = 4f;                             //Where enemy is dragged relative to the swordsman

    HashSet<GameObject> enemyHash;

    float forceMulti = 1f;                                  //Force based on how much swordsman charged attack

    private void Start()
    {
        enemyHash = new HashSet<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy" && !enemyHash.Contains(col.gameObject))
        {
            enemyHash.Add(col.gameObject);
            Invoke("StopMomentum", DRAG_ENEMY_TIME);
        }
    }

    private void FixedUpdate()
    {
        foreach (GameObject target in enemyHash)
        {
            target.transform.position = new Vector3(transform.position.x + X_OFFSET*transform.parent.localScale.x, target.transform.position.y, target.transform.position.z);
            target.GetComponent<Enemy>().Damage(0, .2f);
        }
    }

    void StopMomentum()
    {
        transform.parent.GetComponent<PlayerPhysics>().VelocityX(0);
    }

    public void Reset()
    {
        enemyHash = new HashSet<GameObject>();
    }

    public void SetForceMulti(float force)
    {
        forceMulti = force;
    }

    public void Launch()
    {
        foreach(GameObject target in enemyHash)
        {
            target.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceMulti * X_LAUNCH_FORCE_MULTIPLIER *transform.parent.localScale.x, forceMulti * Y_LAUNCH_FORCE_MULTIPLIER));
            target.GetComponent<Enemy>().Damage(transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().heavyAttackStrength, .1f);
        }
    }



}
