using UnityEngine;
using System.Collections;

public class DownKickScript : MonoBehaviour {

    public float xForce = 15000f;
    public float yForce = -10000;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {

                col.gameObject.layer = LayerMask.NameToLayer("Juggled Enemy");
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, -yForce));

        }
    }

}
