    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorChoice
{
    red, blue, green
}
public class CrystalProperties : MonoBehaviour {

    public AudioClip crystalHit;
    public ColorChoice correctColor;

    AudioSource myAudio;
    SpriteRenderer mySprite;
    ParticleSystem hitParticle;
    PuzzleManager puzzleManager;
    // Use this for initialization
    void Start () {
        mySprite = GetComponent<SpriteRenderer>();
        myAudio = GetComponent<AudioSource>();
        hitParticle = transform.FindChild("HitParticle").gameObject.GetComponent<ParticleSystem>();
        puzzleManager = transform.parent.GetComponent<PuzzleManager>();
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

        puzzleManager.executeIfCorrect();
    }

    public Color getColor()
    {
        return mySprite.color;
    }

    public bool isColorCorrect()
    {
        //return true if the state of crystal is the correct color
        if (correctColor == ColorChoice.red && mySprite.color == Color.red)
            return true;
        else if (correctColor == ColorChoice.green && mySprite.color == Color.green)
            return true;
        else if (correctColor == ColorChoice.blue && mySprite.color == Color.blue)
            return true;
        else
            return false;
    }
}
