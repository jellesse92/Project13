using UnityEngine;
using System.Collections;

public class RocketBehavior : MonoBehaviour {

    int damage = 20;
    public float counter = 0f;
    float delay = 0.75f;
    public GameObject target;
    int direction;


	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if (counter < delay)
        {
            if (direction == 1)
            {
                transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
            }
            else if (direction == 2)
            {
                transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y + 0.1f, transform.position.z);
            }
            else if (direction == 3)
            {
                transform.position = new Vector3(transform.position.x + 0.06f, transform.position.y + 0.14f, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x - 0.06f, transform.position.y + 0.14f, transform.position.z);
            }
        }
        if (counter >= delay)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.GetComponent<Renderer>().bounds.center, .3f);
        }
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
        counter = 0;
    }

    public void SetDirection(int n)
    {
        direction = n;
    }
}
       
