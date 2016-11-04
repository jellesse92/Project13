using UnityEngine;
using System.Collections;

public class BulletSourceScript : MonoBehaviour {

    const float stunMultiplier = 1f;                //Multiplier for how long enemies should be stunned after hit

    LayerMask layermask;                            //Prevent raycast from hitting unimportant layers
    RaycastHit2D hit;                               //What was hit by raycast


    // Use this for initialization
    void Start () {
        layermask = (LayerMask.GetMask("Default","Enemy"));
	}

    //Cast out quick shot ray to apply damage
    public void QuickShot(int damage)
    {

        hit = Physics2D.Raycast(transform.position, transform.right * transform.parent.localScale.x, 50, layermask);
        if (hit.collider != null)
        {
            //make a spark at the hit.point
            if (hit.collider.tag == "Enemy")
                hit.collider.GetComponent<Enemy>().Damage(damage, stunMultiplier);

            //Color color = hit ? Color.green : Color.red;
            //Debug.DrawRay(transform.position, transform.right * (50f * transform.parent.localScale.x), color);
        }
    }
}
