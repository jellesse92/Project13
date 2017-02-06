using UnityEngine;
using System.Collections;

public class SwordsmanAirMelee : MonoBehaviour {

    int damage = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<Enemy>().Damage(damage, 1f);
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(400f * transform.parent.localScale.x, 10000f));
        }

    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
