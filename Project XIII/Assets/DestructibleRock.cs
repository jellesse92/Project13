using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleRock : ItemHitTrigger
{
    public GameObject rockFragments;
    public GameObject rockDestroyed;
    public AudioClip rockDestroyedSound;

    int hp = 6; //need to make into actual hp that takes damage info from player
    protected override void ClassSpecificStart()
    {
        //Need to make a namespace for instatiation so I don't have to repeat these
        rockFragments = Instantiate(rockFragments);
        rockFragments.transform.position = transform.position;
        rockFragments.transform.parent = transform;

        rockDestroyed = Instantiate(rockDestroyed);
        rockDestroyed.transform.position = transform.position;
        rockDestroyed.transform.parent = transform;
    }


    public override void ItemHit()
    {
        if (hp <= 0)
            return;
        if (hp == 1)
            Destroyed();
        PlayShake();
        GetComponent<AudioSource>().Play();
        rockFragments.GetComponent<ParticleSystem>().Play();
        hp--;
    }

    void Destroyed()
    {
        rockDestroyed.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().PlayOneShot(rockDestroyedSound);
        GetComponent<BoxCollider2D>().enabled = false;

        Color newColor = new Color();
        newColor.a = 0;
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
