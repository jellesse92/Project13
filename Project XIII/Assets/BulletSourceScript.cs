using UnityEngine;
using System.Collections;

public class BulletSourceScript : MonoBehaviour {

    const float LIGHT_STUN_MULTI = 1f;              //Multiplier for how long enemies should be stunned after light light
    const float HEAVY_STUN_MULTI = 1f;              //Multiplier for how long enemies should be stunned after heavy attack

    LayerMask layermask;                            //Prevent raycast from hitting unimportant layers
    RaycastHit2D[] hit = new RaycastHit2D[5];       //What was hit by raycast


    // Use this for initialization
    void Start () {
        layermask = (LayerMask.GetMask("Default","Enemy"));
	}

    void Update()
    {

        /* HEAVY SHOT STUFF. LOOK AT THIS IF YOU WANNA KNOW WHERE THE EFFECT SHOULD PLAY
        hit[0] = Physics2D.Raycast(transform.position, transform.right * transform.parent.localScale.x, 5f,layermask);
        hit[1] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, .5f, 0), 5, layermask);
        hit[2] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, -.5f, 0), 5, layermask);
        hit[3] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, -.25f, 0), 5, layermask);
        hit[4] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, .25f, 0), 5, layermask);

        Color color = Color.red;

        for (int i = 0; i <5; i++)
        {
            if (hit[i])
            {
                Debug.Log(hit[i].collider.name);
                color = Color.green;
                break;
            }
        }

        Debug.DrawRay(transform.position, transform.right * (5f * transform.parent.localScale.x), color);
        Debug.DrawRay(transform.position, new Vector3(1* transform.parent.localScale.x,.5f,0) * 5f, color);
        Debug.DrawRay(transform.position, new Vector3(1 * transform.parent.localScale.x, -.5f, 0) * 5f, color);
        Debug.DrawRay(transform.position, new Vector3(1 * transform.parent.localScale.x, -.25f, 0) * 5f, color);
        Debug.DrawRay(transform.position, new Vector3(1 * transform.parent.localScale.x, .25f, 0) * 5f, color);
        */
    }

    //Cast out quick shot ray to apply damage
    public void QuickShot(int damage)
    {

        hit[0] = Physics2D.Raycast(transform.position, transform.right * transform.parent.localScale.x, 50, layermask);

        if (hit[0].collider != null)
        {
            Debug.Log(hit[0].collider.name);
            //make a spark at the hit.point
            //hit[0].point;
            if (hit[0].collider.tag == "Enemy")
            {
                ApplyQuickDamage(hit[0].collider.gameObject, damage);
            }

            //Color color = hit ? Color.green : Color.red;
            //Debug.DrawRay(transform.position, transform.right * (50f * transform.parent.localScale.x), color);
        }
    }

    void ApplyQuickDamage(GameObject target, int damage)
    {
        if(target.tag == "Enemy")
        {
            target.GetComponent<Enemy>().Damage(damage, LIGHT_STUN_MULTI);
            //target.GetComponent<Rigidbody2D>().AddForce(new Vector2(5000f * transform.parent.localScale.x, 20000f));
        }
    }

    public void HeavyShot(int damage)
    {
        //hit[index].distance for multiplier my distance

        hit[0] = Physics2D.Raycast(transform.position, transform.right * transform.parent.localScale.x, 5f, layermask);
        hit[1] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, .5f, 0), 5, layermask);
        hit[2] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, -.5f, 0), 5, layermask);
        hit[3] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, -.25f, 0), 5, layermask);
        hit[4] = Physics2D.Raycast(transform.position, new Vector3(1 * transform.parent.localScale.x, .25f, 0), 5, layermask);

        for(int i = 0; i < 5; i++)
        {
            if (hit[i])
            {
                ApplyHeavyDamage(hit[i].collider.gameObject, damage/4, hit[i].distance);
            }
        }

    }

    void ApplyHeavyDamage(GameObject target, int damage, float distance)
    {
        if(target.tag == "Enemy")
        {
            target.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f * transform.parent.localScale.x, 5000f));
            target.GetComponent<Enemy>().Damage(damage, HEAVY_STUN_MULTI);
        }
    }
}
