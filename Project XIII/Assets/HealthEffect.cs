using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthEffect : MonoBehaviour {

    public Vector2 animationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";

    Vector2 offset = Vector2.zero;

    void LateUpdate()
    {
        offset += (animationRate * Time.deltaTime);
        if (GetComponent<Image>().enabled)
        {
            GetComponent<Image>().material.SetTextureOffset(textureName, offset);
        }
    }
}
