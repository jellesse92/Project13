using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpriteGenerator : MonoBehaviour {

    public float centerToShadowOffSet = 2.76f;
    public float shadowSpriteOffSet = 5.6f;
    public bool changingSprite = true;
    public bool enableShadowSprite = true;
    public bool changingHeight = true;

    public GameObject shadow;
    public GameObject shadowSprite;

    Vector3 shadowScale;
    int layerMask;
    GameObject spriteToCopy;


	void Start () {
        layerMask = (LayerMask.GetMask("Default", "Item"));
        spriteToCopy = transform.parent.gameObject;

        if (!enableShadowSprite)
            shadowSprite.SetActive(false);

        shadowScale = shadow.transform.localScale;
    }

    void FixedUpdate(){
        if(changingHeight)
            RayCastShadow();

        if (changingSprite)
            shadowSprite.GetComponent<SpriteRenderer>().sprite = spriteToCopy.GetComponent<SpriteRenderer>().sprite;
    }

    void RayCastShadow(){
        float scaleChange;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 40, layerMask);
        //Debug.Log(hit.distance);
        if (hit && hit.distance > centerToShadowOffSet)
        {
            scaleChange = (1 / Mathf.Clamp(Mathf.Log(hit.distance - centerToShadowOffSet), 1, 30));
            shadow.transform.position = hit.point;
            shadow.transform.localScale = new Vector3(shadowScale.x * scaleChange, shadowScale.y * scaleChange, shadowScale.z);
        }
        
        if (enableShadowSprite && hit.distance > centerToShadowOffSet)
        {
            Vector3 newPosition = shadowSprite.transform.position;
            newPosition.y = transform.position.y - (hit.distance - centerToShadowOffSet) * 2 - shadowSpriteOffSet;
            shadowSprite.transform.position = newPosition;
        }
        
    }
}
