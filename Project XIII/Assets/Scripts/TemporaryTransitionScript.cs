using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TemporaryTransitionScript : MonoBehaviour {

    public int sceneIndex = 0;

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
