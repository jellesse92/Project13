using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchObject : ItemHitTrigger{   
    public GameObject torchDestroyed;
    public List<GameObject> shadows;

    protected override void ClassSpecificStart()
    {
        //Need to make a namespace for instatiation so I don't have to repeat these
        torchDestroyed = Instantiate(torchDestroyed);
        torchDestroyed.transform.position = transform.position;
        torchDestroyed.transform.parent = transform;
    }

    public override void ItemHit()
    {
        torchDestroyed.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
        GetComponent<BoxCollider2D>().enabled = false;

        Color newColor = new Color();
        newColor.a = 0;
        GetComponent<SpriteRenderer>().color = newColor;

        transform.GetChild(0).GetComponent<ParticleSystem>().Clear();
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        foreach (GameObject shadow in shadows)
            shadow.GetComponent<ShadowSpriteGenerator>().SendLightSource(null);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ShadowSpriteGenerator shadow;
        foreach (Transform child in other.transform)
        {
            if (child.name == "Shadow") {
                shadow = child.GetComponent<ShadowSpriteGenerator>();
                shadow.SendLightSource(transform);

                if (!shadows.Contains(child.gameObject))
                    shadows.Add(child.gameObject);
            }
        }
    }
}
