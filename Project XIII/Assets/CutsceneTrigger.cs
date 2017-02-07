using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour {

    public int cutsceneToTrigger = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CutsceneManager script = transform.parent.parent.GetComponent<CutscenePropManager>().GetScript();
            script.ActivateCutscene(cutsceneToTrigger);
        }

    }
}
