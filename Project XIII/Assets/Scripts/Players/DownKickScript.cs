using UnityEngine;
using System.Collections;

public class DownKickScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            Debug.Log("col with " + col.name);

                col.gameObject.layer = LayerMask.NameToLayer("Juggled Enemy");
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(15000f, -10000f));



        }
    }

}
