using UnityEngine;
using System.Collections;

public class ShieldBurstScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            {
                float xDistance = col.transform.position.x - transform.position.x;
                float yDistance = col.transform.position.y - transform.position.y;

                if (Mathf.Abs(xDistance) < .1f)
                    xDistance = .5f;

                if (Mathf.Abs(yDistance) < .1f )
                    yDistance = .5f;

                float yMulti = 0f;

                if (col.transform.position.y > transform.position.y)
                    yMulti = 4000f;
                else
                    yMulti = -2000f;
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000f * (5f / xDistance), yMulti * (5f / yDistance)));
                col.GetComponent<Enemy>().Damage(10, .5f);
            }
        }
    }
}
