using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleRock : ItemHitTrigger
{
    public GameObject rockFragments;
    protected override void ClassSpecificStart()
    {
        rockFragments = Instantiate(rockFragments);
        rockFragments.transform.position = transform.position;
        rockFragments.transform.parent = transform;
    }
    public override void ItemHit()
    {
        PlayShake();
        GetComponent<AudioSource>().Play();
        rockFragments.GetComponent<ParticleSystem>().Play();
    }
}
