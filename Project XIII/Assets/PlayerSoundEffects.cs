using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayerSoundEffects : MonoBehaviour {

    public AudioClip[] playerLastAliveDeathVoices;                    //Array of death voices for characters
    public AudioClip hitSparkAudio;                           //audio clip at enemy hit contact

    AudioSource myAudio;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayPlayerLastAliveDeathVoices()
    {
        myAudio.PlayOneShot(playerLastAliveDeathVoices[Random.Range(0, playerLastAliveDeathVoices.Length)]);
    }
}
