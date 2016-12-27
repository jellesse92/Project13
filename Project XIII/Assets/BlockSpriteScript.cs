using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpriteScript : MonoBehaviour {

    void OnBecameVisible()
    {
        transform.parent.GetComponent<SquishBlockScript>().VisibleFunc();
    }

    void OnBecameInvisible()
    {
        transform.parent.GetComponent<SquishBlockScript>().InvisFunc();
    }
}
