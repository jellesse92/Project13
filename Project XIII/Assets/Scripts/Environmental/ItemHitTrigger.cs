using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHitTrigger : MonoBehaviour {
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public bool testMode = false;

    Vector3 oldPosition;

    protected void Start()
    {
        oldPosition = transform.position;
        if (testMode)
            PlayShake();
        ClassSpecificStart();
    }

    protected virtual void ClassSpecificStart()
    {

    }

    public virtual void ItemHit()
    {
        PlayShake();
    }

    public void PlayShake()
    {

        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    protected IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = 1 - (elapsed / shakeDuration);

            float quakeAmtY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
            float quakeAmtX = Random.value * shakeMagnitude * 2 - shakeMagnitude;

            Vector3 newPosition = transform.position;
            newPosition.y += quakeAmtY;
            newPosition.x += quakeAmtX;

            newPosition.y = Mathf.Clamp(newPosition.y, oldPosition.y - shakeMagnitude * percentComplete, oldPosition.y + shakeMagnitude * percentComplete);
            newPosition.x = Mathf.Clamp(newPosition.x, oldPosition.x - shakeMagnitude * percentComplete, oldPosition.x + shakeMagnitude * percentComplete);

            //newPosition.x += Mathf.Sin(Time.time * speed);
            transform.position = newPosition;
            yield return null;
        }
        transform.position = oldPosition;

    }
}
