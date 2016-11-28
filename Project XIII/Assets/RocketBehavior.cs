using UnityEngine;
using System.Collections;

public class RocketBehavior : MonoBehaviour {

    int damage = 20;
    public GameObject target;

	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, .01f);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(damage);
            
        }
    }

    public void SetTarget(GameObject t)
    {
        target = t;
    }
}
       
