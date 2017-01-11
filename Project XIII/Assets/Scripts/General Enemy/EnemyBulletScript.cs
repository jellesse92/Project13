using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

    int damage = 10;
    
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Enemy")
        {
            //transform.parent.parent.GetComponent<BasicRangeEnemy>().ReloadAmmo();
            if (col.tag == "Player")
            {
                col.GetComponent<PlayerProperties>().TakeDamage(damage);
            }
            gameObject.SetActive(false);
        }
    }
    
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
