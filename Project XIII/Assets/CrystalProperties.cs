    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalProperties : MonoBehaviour {

    public int crystalNumber = 0;
    public AudioClip crystalHit;
    AudioSource myAudio;
    SpriteRenderer mySprite;
    ParticleSystem hitParticle;

    // Use this for initialization
    void Start () {
        mySprite = GetComponent<SpriteRenderer>();
        myAudio = GetComponent<AudioSource>();
        hitParticle = transform.FindChild("HitParticle").gameObject.GetComponent<ParticleSystem>();
    }

    public void SwitchColor()
    {
        myAudio.PlayOneShot(crystalHit);
        hitParticle.Play();
        if (mySprite.color == Color.red)
            mySprite.color = Color.green;
        else if (mySprite.color == Color.green)
            mySprite.color = Color.blue;
        else if (mySprite.color == Color.blue)
            mySprite.color = Color.red;
        else
            mySprite.color = Color.red;
    }

    public Color getColor()
    {
        return mySprite.color;
    }

    public int getCrystalNumber()
    {
        return crystalNumber;
    }
}
