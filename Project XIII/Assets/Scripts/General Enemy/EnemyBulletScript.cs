using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

    int damage = 10;
    
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Enemy")
        {
            if (col.tag == "Player")
                col.GetComponent<PlayerProperties>().TakeDamage(damage);
            
            if (!col.GetComponent<Collider2D>().isTrigger)
            {
                gameObject.SetActive(false);
                transform.parent.parent.GetComponent<Enemy>().ReloadAmmo();
            }

        }
    }
    
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
