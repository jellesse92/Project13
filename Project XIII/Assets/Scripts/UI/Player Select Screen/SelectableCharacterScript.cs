using UnityEngine;
using System.Collections;

public class SelectableCharacterScript : MonoBehaviour {

    public bool unlocked = true;

    private void Start()
    {
        if (!unlocked)
        {
            this.enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
	void FixedUpdate () {

        //Can't make it more efficient for some reason. Resizes collider 
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(
        gameObject.GetComponent<RectTransform>().sizeDelta.x,
        gameObject.GetComponent<RectTransform>().sizeDelta.y
        );
    }
}
