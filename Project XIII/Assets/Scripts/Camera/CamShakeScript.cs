using UnityEngine;
using System.Collections;

public class CamShakeScript : MonoBehaviour {
    const float FREQUENCY = 0.2f;

    public float duration = 1f;
    public float magnitude = 2f;
    public bool debugTest = false;

    public void PlayShake()
    {
        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    void Start()
    {
        if (debugTest)
        {
            PlayShake();
        }
    }

    public void StopShake()
    {
        StopAllCoroutines();
    }

    public void StartShake(float newMagnitude = 2f, float newDuration = 1f)
    {
        duration = newDuration;
        magnitude = newMagnitude;
        PlayShake();
    }


    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        float seed = Time.time + 0.1f; //Use time as seed

        Vector3 newCamPos;
        Vector3 oldCamPos;

        float percentComplete;
        float damper;
        float x, y;

        oldCamPos = transform.parent.position;
        while (elapsed < duration)
        {
            oldCamPos = transform.parent.position;
            newCamPos = transform.position;
            elapsed += Time.deltaTime;
            percentComplete = elapsed / duration;
            
            damper = 1 - percentComplete; // Damper increase to 1 as it gets more complete
            
            seed += FREQUENCY; //higher the frequency the more change to the perlin

            // use seed to generte a number from perlin noise (* 2 - 1) is use to get a negative number
            x = Mathf.PerlinNoise(seed, 0.0f) * 2 - 1;
            y = Mathf.PerlinNoise(0.0f, seed) * 2 - 1;
            Debug.Log(x);
            x *= magnitude * damper;
            y *= magnitude * damper;

            newCamPos.x += x;
            newCamPos.y += y;
            
            newCamPos.x = Mathf.Clamp(newCamPos.x, oldCamPos.x - Mathf.Abs(x), oldCamPos.x + Mathf.Abs(x));
            newCamPos.y = Mathf.Clamp(newCamPos.y, oldCamPos.y - Mathf.Abs(y), oldCamPos.y + Mathf.Abs(y));

            transform.position = newCamPos;
            yield return null;
        }
        transform.position = oldCamPos;
        //transform.position = transform.parent.position;


    }
}
