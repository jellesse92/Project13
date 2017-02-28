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
    public float timeBeforeFirstAttack = 2f;
    public TimeRange timeRange;

    public override void OnTriggerEnter2D(Collider2D triggerObject)
    {
        if (triggerObject.CompareTag("Player"))
            DamagePlayer(triggerObject.gameObject);
    }

    void DamagePlayer(GameObject player)
    {
        player.GetComponent<PlayerProperties>().TakeDamage(damage);
    }
}
