using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEffects : MonoBehaviour {
    public AudioClip[] receiveDamageVoiceList;
    public int xOut10ToSaySomething = 4;

    AudioSource myAudio;
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayRecieveDamageVoice()
    {
        if (Random.Range(0, 10) < xOut10ToSaySomething)
            myAudio.PlayOneShot(receiveDamageVoiceList[Random.Range(0, receiveDamageVoiceList.Length)]);
    }

    public void playSound(AudioClip clip)
    {
        myAudio.PlayOneShot(clip);
    }
}
