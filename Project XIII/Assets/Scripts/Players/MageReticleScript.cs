using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageReticleScript : MonoBehaviour {

    const float SPEED = .5f;
    public bool freeze_y = false;
    public float screenShakeAmt = 0f;
    public float shakeDuration = .5f;

    PlayerInput input;
    GameObject master; 

    //For setting position indepent of parent
    Vector3 localPosition;
    bool checkedLastPos;

    bool attackActive = false;

    HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();
    int damage = 10;
    float stunDuration = 1f;

    void Start()
    {
        checkedLastPos = false;
    }

    void FixedUpdate()
    {
        if (master != null)
        {
            float x = (Mathf.Abs(input.getKeyPress().horizontalAxisValue) > 0.05) ? input.getKeyPress().horizontalAxisValue : 0f;
            float y = 0f;
            
            if(!freeze_y)
                y = (Mathf.Abs(input.getKeyPress().verticalAxisValue) > 0.05) ? input.getKeyPress().verticalAxisValue : 0f;

            Vector3 moveDir = new Vector3(x * SPEED, y * SPEED, 0f);
            transform.position = transform.position + moveDir;

            SetPosition();
        }
        else
            Debug.Log("no master?");
    }

    void LateUpdate()
    {
        if(!GetComponent<SpriteRenderer>().enabled)
            transform.position = localPosition;
    }

    //Sets position so as to not follow parent while active
    void SetPosition()
    {
        if (GetComponent<SpriteRenderer>().enabled)
        {
            checkedLastPos = false;
            localPosition = transform.position;
        }

        else
            if (!checkedLastPos)
                checkedLastPos = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Enemy" )
        {
            if (!attackActive)
                enemiesInRange.Add(col.gameObject);
            else
                ApplyQuickAttack(col.gameObject);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (!attackActive)
        {
            if (col.tag == "Enemy")
            {
                if (col.tag == "Enemy" && enemiesInRange.Contains(col.gameObject))
                {
                    enemiesInRange.Remove(col.gameObject);
                }
            }
        }
    }

    public void ReleaseQuickAttack()
    {
        attackActive = true;
        foreach(GameObject enemy in enemiesInRange)
        {
            ApplyQuickAttack(enemy);
        }
    }

    public void ExtinguishAttack()
    {
        attackActive = false;
        Reset();
    }

    void ApplyQuickAttack(GameObject enemy)
    {
        float xDistance = enemy.transform.position.x - transform.position.x;
        float yDistance = enemy.transform.position.y - transform.position.y;

        if (xDistance < .5f)
            xDistance = .5f;

        if (yDistance < .5f)
            yDistance = .5f;

        float yMulti = 3000f;

        Vector3 dir = (this.transform.position - enemy.transform.position).normalized;


        if (dir.y < 0f)
            yMulti = 5000f;

            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(3000f * -dir.x * (5f/xDistance), yMulti * -dir.y * (5f/yDistance)));


        ApplyDamage(enemy);
    }

    public void ReleaseHeavyAttack()
    {
        attackActive = true;
        foreach (GameObject enemy in enemiesInRange)
        {
            ApplyHeavyAttack(enemy);
        }
    }

    void ApplyHeavyAttack(GameObject enemy)
    {
        float xDistance = enemy.transform.position.x - transform.position.x;
        float i = 0f;

        if (xDistance > 0)
            i = 1f;
        else if (xDistance < 0)
            i = -1f;

        xDistance = Mathf.Abs(xDistance);

        if (xDistance < .1f)
            xDistance = .1f;

        enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f * i* (5f / xDistance), 50000f));

        ApplyDamage(enemy);
        
    }

    void ApplyDamage(GameObject enemy)
    {
        if (master.transform.parent != null)
            master.transform.parent.GetComponent<PlayerEffectsManager>().ScreenShake(screenShakeAmt,shakeDuration);
        enemy.GetComponent<Enemy>().Damage(damage, stunDuration);
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetStunDuration(float dur)
    {
        stunDuration = dur;
    }

    public void Reset()
    {
        enemiesInRange = new HashSet<GameObject>();
        attackActive = false;
    }

    public void SetMaster(GameObject m)
    {
        master = m;
        input = master.GetComponent<PlayerInput>();
    }
}
