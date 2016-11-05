using UnityEngine;
using System.Collections;

public class BulletSourceScript : MonoBehaviour {

    const float LIGHT_STUN_MULTI = 1f;              //Multiplier for how long enemies should be stunned after light light
    const float HEAVY_STUN_MULTI = 1f;              //Multiplier for how long enemies should be stunned after heavy attack
    const float HIT_DISCREP = .5f;                  //Discrepency from target location allowed for air juggle

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
        hit[1] = Physics2D.Raycast(transform.position + new Vector3(0, HIT_DISCREP, 0), transform.right * transform.parent.localScale.x, 50, layermask);
        hit[2] = Physics2D.Raycast(transform.position + new Vector3(0, HIT_DISCREP * -1f, 0), transform.right * transform.parent.localScale.x, 50, layermask);

        for(int i = 0; i < 3; i++)
        {
            //make a spark at the hit.point
            //hit[0].point;
            if(hit[i].collider != null)
            {
                ApplyQuickDamage(hit[i].collider.gameObject, damage);
                break;
            }

        }

        Color color = Color.green;

        Debug.DrawRay(transform.position, transform.right * (60f * transform.parent.localScale.x), color);
        Debug.DrawRay(transform.position + new Vector3(0, .5f, 0), transform.right * (60f * transform.parent.localScale.x), color);
        Debug.DrawRay(transform.position + new Vector3(0, -.5f, 0), transform.right * (60f * transform.parent.localScale.x), color);

    }

    void ApplyQuickDamage(GameObject target, int damage)
    {
        if(target.tag == "Enemy")
        {
            target.GetComponent<Enemy>().Damage(damage, LIGHT_STUN_MULTI);
            if (!target.GetComponent<Enemy>().IsGrounded())
            {
                target.GetComponent<Rigidbody2D>().AddForce(new Vector2(100f, 20000f));
            }

        }

    }

    public void HeavyShot(int damage)
    {
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
            target.GetComponent<Rigidbody2D>().AddForce(new Vector2(100f * transform.parent.localScale.x, 6000f));
            target.GetComponent<Enemy>().Damage(damage, HEAVY_STUN_MULTI);
        }
    }
}
