using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : Enemy {
    [System.Serializable]
    public class TimeRange
    {
        public float minWait = 3f;
        public float maxWait = 5f;
    }

    public int damage = 50;
    public float timeBeforeFirstAttack = 0.5f;
    public TimeRange timeRange;
    public ParticleSystem explosion;

    bool explosionPlaying = false;
    void Start()
    {
        StartCoroutine(AttackWaitTime(timeBeforeFirstAttack));
    }

    public override void FixedUpdate()
    {
        if (dead & !explosionPlaying)
        {
            explosion.Play();
            explosionPlaying = true;
            StopAllCoroutines();
        }
        gameObject.layer = 12;
    }

    IEnumerator AttackWaitTime(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            GetComponentInParent<Animator>().SetTrigger("attack");
            waitTime = Random.Range(timeRange.minWait, timeRange.maxWait);
        }
        
    }
    public override void OnTriggerEnter2D(Collider2D triggerObject)
    {
        Debug.Log(triggerObject.tag);
        if (triggerObject.tag == "Player")
        {          
            triggerObject.GetComponent<PlayerProperties>().TakeDamage(damage);
        }
    }
    
}
