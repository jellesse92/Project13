using UnityEngine;
using System.Collections;

public class SelectableCharacterScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

        //Can't make it more efficient for some reason. Resizes collider 
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(
        gameObject.GetComponent<RectTransform>().sizeDelta.x,
        gameObject.GetComponent<RectTransform>().sizeDelta.y
        );
    }
}
