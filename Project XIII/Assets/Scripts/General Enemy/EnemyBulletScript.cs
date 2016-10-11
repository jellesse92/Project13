using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Enemy")
        {
            transform.parent.parent.GetComponent<BasicRangeEnemy>().ReloadAmmo();
            if (col.tag == "Player")
            {
                //Apply damage to player
            }
            gameObject.SetActive(false);
        }


    }
}
