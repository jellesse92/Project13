using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour {
    public int damage;
    public bool scroll;
    public float autoScrollSpeed;
    public bool followCamera;
    Transform cameraTransform;
    float lastCameraY;
    float deltaY;
    float lastCameraX;
    float deltaX;

    Vector3 velocity = Vector3.zero;
    Vector3 newPosition;
    // Use this for initialization
    void Start () {
        cameraTransform = Camera.main.transform;

    }

    // Update is called once per frame
    void LateUpdate () {
        if(scroll)
            transform.position += Vector3.right * (Time.deltaTime * autoScrollSpeed);

        if (followCamera)
        {
            deltaY = cameraTransform.position.y - lastCameraY;
            transform.position += Vector3.up * (deltaY * 1);
            lastCameraY = cameraTransform.position.y;

            if (cameraTransform.position.x - transform.position.x > 40)
            {
                newPosition = new Vector3(cameraTransform.position.x - 40, transform.position.y, 0);
                transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.15f);

            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<Enemy>().Damage(damage);
            col.GetComponent<EnemyParticleEffects>().PlayParticle(col.GetComponent<EnemyParticleEffects>().fireDamage);
        }
        else if (col.tag == "Player")
        {
            col.GetComponent<PlayerProperties>().TakeDamage(damage);
            col.GetComponent<PlayerParticleEffects>().PlayParticle(col.GetComponent<PlayerParticleEffects>().fireDamage);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Enemy")
            col.GetComponent<Enemy>().Damage(damage);
        else if (col.tag == "Player")
            col.GetComponent<PlayerProperties>().TakeDamage(damage);
    }
}
