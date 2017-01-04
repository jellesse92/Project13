using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMineScript : MonoBehaviour {

    const int DAMAGE = 20;

    public ParticleSystem explosion;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            explosion.Play();
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
}
