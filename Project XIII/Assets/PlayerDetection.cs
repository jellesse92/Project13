using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (Transform child in other.transform)
        {
            if (child.name == "Shadow")
                child.GetComponent<ShadowSpriteGenerator>().SendLightSource(transform);
        }        
    }
}
