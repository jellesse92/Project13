using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicStartLoop : MonoBehaviour {
    public AudioClip engineStartClip;
    public AudioClip engineLoopClip;
    
    AudioSource myAudio;
    // Use this for initialization

    void Start () {
        myAudio = GetComponent<AudioSource>();
        GetComponent<AudioSource>().loop = true;
        StartCoroutine(playEngineSound());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator playEngineSound()
    {
        myAudio.clip = engineStartClip;
        myAudio.Play();
        yield return new WaitForSeconds(myAudio.clip.length);
        myAudio.clip = engineLoopClip;
        myAudio.Play();
    }
}