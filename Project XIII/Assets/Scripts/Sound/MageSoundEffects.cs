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
        myAudio.PlayOneShot(mageQuickAttackVoiceList[Random.Range(0, mageQuickAttackVoiceList.Length)]);
    }

    public void mageHeavyAttackVoice()
    {
        myAudio.PlayOneShot(mageHeavyAttackVoiceList[Random.Range(0, mageHeavyAttackVoiceList.Length)]);
    }

    public void mageDamageVoice()
    {
        myAudio.PlayOneShot(mageDamageVoiceList[Random.Range(0, mageDamageVoiceList.Length)]);
    }

    public void mageDeathVoice()
    {
        myAudio.PlayOneShot(mageDeathVoiceList[Random.Range(0, mageDeathVoiceList.Length)]);
    }
}
