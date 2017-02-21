using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {
    ParticleSystem pickup;
    ParticleSystem myParticleSystem;
    AudioSource myAudio;
    int coinValue = 10;

    void Awake () {
        myParticleSystem = GetComponent<ParticleSystem>();
        pickup = transform.GetChild(0).GetComponent<ParticleSystem>();
        myAudio = GetComponent<AudioSource>();

        int i = 0;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            myParticleSystem.trigger.SetCollider(i, player.GetComponent<BoxCollider2D>());
            i++;
        }
    }

    void OnParticleTrigger() {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = myParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            pickup.transform.position = p.position;
            pickup.Play();
            p.remainingLifetime = 0;
            myAudio.Play();
            GameData.current.player1.souls += coinValue;
            enter[i] = p;
        }
        myParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }

}
