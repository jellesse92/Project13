using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpriteScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Activation Field")
        {
            transform.parent.GetComponent<SquishBlockScript>().VisibleFunc();
        }

        if (col.name == "Kill Zone Activate")
        {
            transform.parent.GetComponent<SquishBlockScript>().killZone.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Activation Field")
        {
            transform.parent.GetComponent<SquishBlockScript>().InvisFunc();
        }
    }


}
