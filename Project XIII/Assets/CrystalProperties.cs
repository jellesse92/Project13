    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorChoice
{
    red, blue, green
}

public class CrystalProperties : ItemHitTrigger {

    public AudioClip crystalHit;
    public ColorChoice correctColor;

    AudioSource myAudio;
    SpriteRenderer mySprite;
    ParticleSystem hitParticle;
    PuzzleManager puzzleManager;

    static bool puzzleSolved = false;

    void Start () {
        mySprite = GetComponent<SpriteRenderer>();
        myAudio = GetComponent<AudioSource>();
        hitParticle = transform.FindChild("HitParticle").gameObject.GetComponent<ParticleSystem>();
        puzzleManager = transform.parent.GetComponent<PuzzleManager>();
    }

    public override void ItemHit()
    {
        myAudio.PlayOneShot(crystalHit);
        hitParticle.Play();

        if (!puzzleSolved)
        {

            if (mySprite.color == Color.red)
                mySprite.color = Color.green;
            else if (mySprite.color == Color.green)
                mySprite.color = Color.blue;
            else if (mySprite.color == Color.blue)
                mySprite.color = Color.red;
            else
                mySprite.color = Color.red;

            puzzleSolved = puzzleManager.executeIfCorrect();
        }
    }

    public Color getColor()
    {
        return mySprite.color;
    }

    public bool isColorCorrect()
    {
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
