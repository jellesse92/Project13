using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MageSoundEffects : MonoBehaviour {
    private AudioSource myAudio;
    public AudioClip[] mageQuickAttackVoiceList;
    public AudioClip[] mageHeavyAttackVoiceList;
    public AudioClip[] mageDamageVoiceList;
    public AudioClip[] mageDeathVoiceList;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void mageQuickAttackVoice()
    {
        myAudio.clip = mageQuickAttackVoiceList[Random.Range(0, mageQuickAttackVoiceList.Length)];
        myAudio.Play();
    }

    public void mageHeavyAttackVoice()
    {
        myAudio.clip = mageHeavyAttackVoiceList[Random.Range(0, mageHeavyAttackVoiceList.Length)];
        myAudio.Play();
    }

    public void mageDamageVoice()
    {
        myAudio.clip = mageDamageVoiceList[Random.Range(0, mageDamageVoiceList.Length)];
        myAudio.Play();
    }

    public void mageDeathVoice()
    {
        myAudio.clip = mageDeathVoiceList[Random.Range(0, mageDeathVoiceList.Length)];
        myAudio.Play();
    }
}
