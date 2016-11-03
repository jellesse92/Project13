using UnityEngine;
using System.Collections;

public class MusicTriggerScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            transform.parent.GetComponent<MusicManager>().ActivateNextClip();
    }
}
