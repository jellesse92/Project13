//Credit http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html

using UnityEngine;
using System.Collections;

public class CamShakeScript : MonoBehaviour {
    public float duration = 0.5f;
    public float speed = 20f;
    public float magnitude = 0.5f;

    public bool useUnityPerlin = false;
    public bool test = false;
    public bool shakeParent = false;

    public void PlayShake()
    {

        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    void Update()
    {
        if (test)
        {
            test = false;
            PlayShake();
        }
    }

    public void StopShake()
    {
        StopAllCoroutines();

    }

    public void StartShake(float newMagnitude = 0.5f, float newDuration = 1f, float newSpeed = 20f)
    {
        duration = newDuration;
        speed = newSpeed;
        magnitude = newMagnitude;
        PlayShake();
    }


    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        
        float randomStart = Random.Range(-1000.0f, 1000.0f);


        while (elapsed < duration)
        {
            //Vector3 originalCamPos = transform.position;
            Vector3 newCamPos = transform.position;

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;

            // We want to reduce the shake from full power to 0 starting half way through
            float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

            // Calculate the noise parameter starting randomly and going as fast as speed allows
            float alpha = randomStart + speed * percentComplete;

            // map noise in perlin noise
            float x, y;
            if (useUnityPerlin)
            {
                x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
                y = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;
            }
            else
            {
                x = Util.Noise.GetNoise(alpha, 0.0f, 0.0f) * 2.0f - 1.0f;
                y = Util.Noise.GetNoise(0.0f, alpha, 0.0f) * 2.0f - 1.0f;
            }

            x *= magnitude * damper;
            y *= magnitude * damper;
            newCamPos.x += x;
            newCamPos.y += y;
            if(shakeParent)
                transform.parent.position = newCamPos;
            else
                transform.position = newCamPos;
            //transform.position = originalCamPos;

            yield return null;
        }

        
    }
}
