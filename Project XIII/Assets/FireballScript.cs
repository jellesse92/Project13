using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour
{
    string FIREBALL_NAME = "Fireball";
    string IMPACT_NAME = "Impact";

    public AudioClip impactAudio;
    public float delay;
    public float force;
    public int damage = 10;
    ParticleSystem fireballParticle;
    ParticleSystem impactParticle;
    Vector3 stopPosition;

    AudioSource myAudio;
    bool stopped;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        stopPosition = transform.position;
        if (transform.Find(FIREBALL_NAME))
            fireballParticle = transform.Find(FIREBALL_NAME).GetComponent<ParticleSystem>();
        if (transform.Find(IMPACT_NAME))
            impactParticle = transform.Find(IMPACT_NAME).GetComponent<ParticleSystem>();

        StartCoroutine(loop());

    }

    void LateUpdate()
    {
        if (!stopped && stopPosition.y >= transform.position.y)
        {
            stopped = true;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
    IEnumerator loop()
    {
        while (true)
        {
            if (stopped)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
                GetComponent<Rigidbody2D>().gravityScale = 1;
                stopped = false;
            }
            yield return new WaitForSeconds(delay);

        }
    }

    public void PlayParticle(bool play)
    {
        if (play)
            fireballParticle.Play();
        else
        {
            fireballParticle.Stop();
            impactParticle.Play();
            myAudio.PlayOneShot(impactAudio);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Enemy")
        {
            if (col.tag == "Player")
                col.GetComponent<PlayerProperties>().TakeDamage(damage);
        }
    }
}
