using UnityEngine;
using System.Collections;

public class AnimationEventSound : MonoBehaviour {
    private AudioSource myAudio;
    
    void Start () {
        myAudio = GetComponent<AudioSource>();
	}
	
    public void playSound(AudioClip clip)
    {
        myAudio.clip = clip;
        myAudio.Play();
    }
}
