using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

    int damage = 10;
    
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Enemy")
        {
            Debug.Log(col.tag);
            if (col.tag == "Player")
            {
                col.GetComponent<PlayerProperties>().TakeDamage(damage);
            }
            
            gameObject.SetActive(false);
            transform.parent.parent.GetComponent<Enemy>().ReloadAmmo();
        }
    }
    
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
