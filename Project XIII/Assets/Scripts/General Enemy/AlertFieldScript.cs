using UnityEngine;
using System.Collections;

public class AlertFieldScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if(transform.parent.GetComponent<Enemy>().inPursuit)
            if (col.tag == "Enemy" && col.gameObject.GetComponent<Enemy>().isVisible)
                col.gameObject.GetComponent<Enemy>().inPursuit = true;
    }
}
