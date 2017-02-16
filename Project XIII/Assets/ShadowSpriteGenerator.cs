using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpriteGenerator : MonoBehaviour {

    public float centerToShadowOffSet = 2.76f;
    public float shadowSpriteOffSet = 5.6f;
    public bool changingSprite = true;
    public bool enableShadowSprite = true;
    public bool changingHeight = true;
    public bool lightInteraction = true;

    public GameObject shadow;
    public GameObject shadowSprite;
    float magnitudeShadowChange = 1f;
    Vector3 shadowScale;
    int layerMask;
    GameObject spriteToCopy;
    Transform lightSource;

	void Start () {
        layerMask = (LayerMask.GetMask("Default", "Item"));
        spriteToCopy = transform.parent.gameObject;

        if (!enableShadowSprite)
            shadowSprite.SetActive(false);

        shadowScale = shadow.transform.localScale;
        shadowSprite.GetComponent<Renderer>().sharedMaterial.SetFloat("_HorizontalSkew", -0.7f);
    }

    void FixedUpdate(){
        if(changingHeight)
            RayCastShadow();

        if (enableShadowSprite && changingSprite)
            shadowSprite.GetComponent<SpriteRenderer>().sprite = spriteToCopy.GetComponent<SpriteRenderer>().sprite;

        if (lightSource && enableShadowSprite)
            InteractLightSource();
    }

    void RayCastShadow(){
        float scaleChange;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 40, layerMask);

        if (hit && hit.distance > centerToShadowOffSet)
        {
            scaleChange = (1 / Mathf.Clamp(Mathf.Log(hit.distance - centerToShadowOffSet), 1, 30));
            shadow.transform.position = hit.point;
            shadow.transform.localScale = new Vector3(shadowScale.x * scaleChange, shadowScale.y * scaleChange, shadowScale.z);
        }
       
        if (lightSource && enableShadowSprite && hit.distance > centerToShadowOffSet)
        {
            Vector3 newPosition = shadowSprite.transform.position;
            newPosition.y = transform.position.y - (hit.distance - centerToShadowOffSet) * 2 - shadowSpriteOffSet;
            shadowSprite.transform.position = newPosition;
        }
    }


    void InteractLightSource()
    {        
        float distanceDifference = transform.parent.position.x - lightSource.position.x;
        float characterFacing = transform.parent.localScale.x / Mathf.Abs(transform.parent.localScale.x);
        float horizontalShear = distanceDifference * characterFacing * magnitudeShadowChange;

        Vector3 newPosition = shadowSprite.transform.localPosition;
        newPosition.x = horizontalShear * 3;

        shadowSprite.transform.localPosition = newPosition;
        shadowSprite.GetComponent<Renderer>().sharedMaterial.SetFloat("_HorizontalSkew", horizontalShear);

        Color newColor = shadowSprite.GetComponent<SpriteRenderer>().color;
        float newAlpha = Mathf.Abs(1 - (Mathf.Clamp(Mathf.Abs(horizontalShear),0,5) / 5));
        newColor.a = Mathf.Clamp01(newAlpha) * 0.7f;
        shadowSprite.GetComponent<SpriteRenderer>().color = newColor;
    }
    public void SendLightSource(Transform newlightSource)
    {
        lightSource = newlightSource;        
    }

    public void FadeShadow()
    {
        GetComponent<Animator>().SetTrigger("Fade");
    }
}
