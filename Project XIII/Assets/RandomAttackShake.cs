using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttackShake : MonoBehaviour {
    CamShakeScript shake;

    public float shakeMag;
    public float shakeDur;
    public float minWait;
    public float maxWait;
    public AudioClip[] soundEffects;

    GameObject rockFragment;
    float secondToWait;
	// Use this for initialization
	void Start () {
        shake = Camera.main.GetComponent<CamShakeScript>();
        StartCoroutine(randomShake());
        rockFragment = transform.GetChild(0).gameObject;
    }
	
	// Update is called once per frame
	void LateUpdate () {
		
	}

    IEnumerator randomShake()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            shake.StartShake(shakeMag, shakeDur);
            GetComponent<AudioSource>().PlayOneShot(soundEffects[Random.Range(0, soundEffects.Length)]);

            GetComponent<AudioSource>().Play();
            rockFragment.GetComponent<ParticleSystem>().Play();
        }
    }
}
