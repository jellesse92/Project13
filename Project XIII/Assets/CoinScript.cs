using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {
    ParticleSystem pickup;
    ParticleSystem myParticleSystem;
    AudioSource myAudio;
    AudioClip audioClip;

    // Use this for initialization
    void Start () {
        myParticleSystem = GetComponent<ParticleSystem>();
        pickup = transform.GetChild(0).GetComponent<ParticleSystem>();
        myAudio = GetComponent<AudioSource>();
        audioClip = myAudio.clip;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnParticleTrigger() {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = myParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            pickup.transform.localPosition = p.position;
            pickup.Play();
            p.remainingLifetime = 0;
            myAudio.PlayOneShot(audioClip);

            enter[i] = p;
        }
        myParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        Debug.Log("trigger");
    }

}
