using UnityEngine;
using System.Collections;

public class RocketBehavior : MonoBehaviour {

    int damage = 20;
    public GameObject target;

	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.GetComponent<Renderer>().bounds.center, .2f);
	}

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Enemy")
        {
            Debug.Log("hit");
            col.gameObject.GetComponent<Enemy>().Damage(damage);
            this.gameObject.SetActive(false);
            this.GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    public void SetTarget(GameObject t)
    {
        target = t;
    }
}
       
