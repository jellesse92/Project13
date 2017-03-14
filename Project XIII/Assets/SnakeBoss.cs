using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : Boss {
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
    public GameObject attackBox;

    bool explosionPlaying = false;                           

    void Start()
    {
        frozen = true;                                                  //Boss should not attack until unfrozen to allow cutscene to play
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
    }

    IEnumerator AttackWaitTime(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (!frozen)
            {
                Debug.Log(frozen);
                GetComponentInParent<Animator>().SetTrigger("attack");
                waitTime = Random.Range(timeRange.minWait, timeRange.maxWait);
            }
        }
        
    }
    
}
