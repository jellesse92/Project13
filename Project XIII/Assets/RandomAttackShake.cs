using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttackShake : MonoBehaviour {
    CamShakeScript shake;

    public float shakeMag;
    public float shakeDur;
    public float weightModifierBetweenShake;

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
            secondToWait = Mathf.Clamp(Random.value * weightModifierBetweenShake, 5, weightModifierBetweenShake);
            yield return new WaitForSeconds(secondToWait);
            shake.StartShake(shakeMag, shakeDur);
            GetComponent<AudioSource>().Play();
            rockFragment.GetComponent<ParticleSystem>().Play();
        }
    }
}
