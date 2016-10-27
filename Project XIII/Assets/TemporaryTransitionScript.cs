using UnityEngine;
using System.Collections;

public class TemporaryTransitionScript : MonoBehaviour {

    public int sceneIndex = 0;

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Application.LoadLevel(sceneIndex);
        }
    }
}
