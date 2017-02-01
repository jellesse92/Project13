using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : ItemHitTrigger {
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;  
    public bool testMode = false;

    public GameObject coins;
    public int coinAmountAtOpen = 10;
    public int coinAmountAtHit = 5;

    public int hitsToOpen = 3;
    
    Vector3 oldPosition;
    // Use this for initialization
    void Start ()
    {
        coins = Instantiate(coins);
        coins.transform.position = transform.position;
        coins.transform.parent = transform;
        coins.transform.localScale = Vector3.one;


        oldPosition = transform.position;
        if (testMode)
            PlayShake();
    }
	
    public override void ItemHit()
    {
        PlayShake();

        if (hitsToOpen == 0)
            return;
        else if(hitsToOpen > 1)
            coins.GetComponent<ParticleSystem>().Emit(coinAmountAtHit);
        else if (hitsToOpen == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
            coins.GetComponent<ParticleSystem>().Emit(coinAmountAtOpen);
            return;
        }
        hitsToOpen--;
    }
    void PlayShake()
    {

        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    IEnumerator Shake()
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
