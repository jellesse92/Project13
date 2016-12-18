using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class PlayerSoundEffects : MonoBehaviour {
    private AudioSource myAudio;
    public AudioClip[] quickAttackVoiceList;
    public AudioClip[] heavyAttackVoiceList;
    public AudioClip[] receiveDamageVoiceList;
    public AudioClip[] deathVoiceList;
    public int xOut10ToSaySomething = 4;

    public AudioClip hitSparkAudio;                           //audio clip at enemy hit contact

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayQuickAttackVoice()
    {
        if (Random.Range(0, 10) < xOut10ToSaySomething)
            myAudio.PlayOneShot(quickAttackVoiceList[Random.Range(0, quickAttackVoiceList.Length)]);
    }

    public void PlayHeavyAttackVoice()
    {
        if (Random.Range(0, 10) < xOut10ToSaySomething)
            myAudio.PlayOneShot(heavyAttackVoiceList[Random.Range(0, heavyAttackVoiceList.Length)]);
    }

    public void PlayRecieveDamageVoice()
    {
        myAudio.PlayOneShot(receiveDamageVoiceList[Random.Range(0, receiveDamageVoiceList.Length)]);
    }

    public void PlayDeathVoice()
    {
        myAudio.PlayOneShot(deathVoiceList[Random.Range(0, deathVoiceList.Length)]);
    }

    public void playSound(AudioClip clip)
    {
        myAudio.PlayOneShot(clip);
    }
}
