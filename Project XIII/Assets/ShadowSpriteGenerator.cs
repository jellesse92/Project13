using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpriteGenerator : MonoBehaviour {

    public float centerToShadowOffset = 2.76f;

    public GameObject shadow;
    public GameObject shadowSprite;

    Vector3 shadowScale;
    int layerMask;
    GameObject spriteToCopy;
	void Start () {
        layerMask = (LayerMask.GetMask("Default", "Item"));
        spriteToCopy = transform.parent.gameObject;
        if (shadow)
            shadowScale = shadow.transform.localScale;
    }

    void FixedUpdate(){
        if (shadow)
            HandleShadow();
    }

    void HandleShadow(){
        float scaleChange;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 30, layerMask);
        if (hit && hit.distance > centerToShadowOffset)
        {
            scaleChange = (1 / Mathf.Clamp(Mathf.Log(hit.distance - centerToShadowOffset), 1, 30));
            shadow.transform.position = hit.point;
            shadow.transform.localScale = new Vector3(shadowScale.x * scaleChange, shadowScale.y * scaleChange, shadowScale.z);
        }

        if (shadowSprite && hit.distance > centerToShadowOffset)
        {
            shadowSprite.GetComponent<SpriteRenderer>().sprite = spriteToCopy.GetComponent<SpriteRenderer>().sprite;
            Vector3 newPosition = shadowSprite.transform.position;
            newPosition.y = transform.position.y - (hit.distance - centerToShadowOffset) * 2 - 5.6f;
            shadowSprite.transform.position = newPosition;
        }
    }
}
