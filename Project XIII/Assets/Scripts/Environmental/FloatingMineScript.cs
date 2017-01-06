using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMineScript : MonoBehaviour {

    const float OUTER_DETECTION_FLASH_RATE = 2.5f;
    const float MIDDLE_DETECTION_FLASH_RATE = 5f;
    const float INNER_DETECTION_FLASH_RATE = 10f;

    const int DAMAGE = 20;

    public ParticleSystem explosion;                //Particle for explosion
    public Transform detectionRadii;                //Parent of detection radius trigger zones
    public AudioClip explosionSoundEffect;

    private int currentRadiusLevel = 3;             //How close player is to mine. 3 is the farthest level
    private float[] flashRates = new float[]{ INNER_DETECTION_FLASH_RATE, MIDDLE_DETECTION_FLASH_RATE, OUTER_DETECTION_FLASH_RATE };

    private Animator myAnimator;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            StopAllCoroutines();
            explosion.Play();
            mainCamera.GetComponent<CamShakeScript>().StartShake(.1f);

            GetComponent<AnimationEventSound>().playSound(explosionSoundEffect);
            col.gameObject.GetComponent<PlayerProperties>().TakeDamage(DAMAGE);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void Reset()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    } 

    public void StopFlashing()
    {
        DetectionFlashEffect(1);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void RadiusReport(int radiusLevel, bool entering)
    {
        if (entering)
        {
            //Prioritze flashing based on the closest player
            if(currentRadiusLevel > radiusLevel)
            {
                StopFlashing();
                DetectionFlashEffect(flashRates[radiusLevel]);
                currentRadiusLevel = radiusLevel;
            }
        }
        else
        {
            //If the radius which is being exited is the closest detected zone and is being exited
            //Check if there are still players in the zone excluding the player exiting
            if(currentRadiusLevel == radiusLevel)
            {
                if (!detectionRadii.GetChild(radiusLevel).GetComponent<MineDetectionScript>().detectsPlayer())
                {
                    StopFlashing();
                    currentRadiusLevel++;
                    if (currentRadiusLevel < 3)
                    {
                        DetectionFlashEffect(flashRates[radiusLevel]);
                    }
                }
            }
        }
    }

    void DetectionFlashEffect(float rate)
    {
        myAnimator.speed = rate;
    }
}
