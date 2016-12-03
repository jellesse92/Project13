using UnityEngine;
using System.Collections;


public class MechWallBehavior : MonoBehaviour {

    float duration = 8f;
    public float counter = 0;


	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        this.GetComponent<Animator>().SetBool("raising", false);
        if (counter >= duration)
        {
            this.gameObject.SetActive(false);
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
	}
}
