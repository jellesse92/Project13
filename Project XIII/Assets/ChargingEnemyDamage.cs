using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemyDamage : MonoBehaviour {

    public float knockBackForceX = 10f;
    public float knockBackForceY = 10f;
    public float stunDuration = 0;
    int damage = 10;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        knockBackForceX = gameObject.GetComponentInParent<ChargingEnemy>().knockBackForceX;
        knockBackForceY = gameObject.GetComponentInParent<ChargingEnemy>().knockBackForceY;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        float xdir = gameObject.transform.parent.localPosition.x < 0 ? -1 : 1;
        //print(xdir);
        float ydir = gameObject.transform.parent.localPosition.y;
        print("attacking");
        if (col.tag == "Player" && col.gameObject.GetComponent<PlayerProperties>().alive)
            col.GetComponent<PlayerProperties>().TakeDamage(transform.parent.GetComponent<Enemy>().attackPower, knockBackForceX, knockBackForceY, stunDuration);
    }
}
