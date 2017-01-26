using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour {
    public int damage;
    public bool scroll;
    public float autoScrollSpeed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(scroll)
            transform.position += Vector3.right * (Time.deltaTime * autoScrollSpeed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Enemy")
        {
            if (col.tag == "Player")
                col.GetComponent<PlayerProperties>().TakeDamage(damage);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag != "Enemy")
        {
            if (col.tag == "Player")
                col.GetComponent<PlayerProperties>().TakeDamage(damage);
        }
    }
}
