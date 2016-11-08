using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AnimationEventSound : MonoBehaviour {
    private AudioSource myAudio;
    
    void Start () {
        myAudio = GetComponent<AudioSource>();
	}
	
    public void playSound(AudioClip clip)
    {
        myAudio.PlayOneShot(clip);
    }
}
