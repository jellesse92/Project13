using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class PlayerSoundEffects : MonoBehaviour {
    public int attackFrequencyPercentage = 40;
    public AudioClip[] quickAttackVoiceList;
    public AudioClip[] heavyAttackVoiceList;
    public AudioClip[] receiveDamageVoiceList;
    public AudioClip[] deathVoiceList;

    public AudioClip[] hitSparkList;                           //audio clip at enemy hit contact
    public AudioClip heal;
    AudioSource myAudio;
    AudioClip myClip;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayQuickAttackVoice()
    {
        if (Random.Range(0, 100) < attackFrequencyPercentage)
            myAudio.PlayOneShot(quickAttackVoiceList[Random.Range(0, quickAttackVoiceList.Length)]);
    }

    public void PlayHeavyAttackVoice()
    {
        if (Random.Range(0, 100) < attackFrequencyPercentage)
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

    public void PlayHitSpark()
    {
        myAudio.PlayOneShot(hitSparkList[Random.Range(0, hitSparkList.Length)]);
    }

    public void playSound(AudioClip clip)
    {
        myAudio.PlayOneShot(clip);
    }

    public void PlayHeal()
    {
        if (!(GetComponent<PlayerProperties>().currentHealth == GetComponent<PlayerProperties>().maxHealth))
            myAudio.PlayOneShot(heal);
    }
}
