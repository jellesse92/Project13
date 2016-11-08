using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageReticleScript : MonoBehaviour {

    const float SPEED = .5f;

    PlayerInput input;
    GameObject master; 

    //For setting position indepent of parent
    Vector3 localPosition;
    bool checkedLastPos;

    bool quickAttackActive = false;

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
            float y = (Mathf.Abs(input.getKeyPress().verticalAxisValue) > 0.05) ? input.getKeyPress().verticalAxisValue : 0f;

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
            if (!quickAttackActive)
                enemiesInRange.Add(col.gameObject);
            else
                if(!enemiesInRange.Contains(col.gameObject))
                    ApplyQuickAttack(col.gameObject);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (!quickAttackActive)
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
        quickAttackActive = true;
        foreach(GameObject enemy in enemiesInRange)
        {
            ApplyQuickAttack(enemy);
        }
    }

    public void ExtinguishAttack()
    {
        quickAttackActive = false;
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

    void ApplyDamage(GameObject enemy)
    {
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
        quickAttackActive = false;
    }

    public void SetMaster(GameObject m)
    {
        master = m;
        input = master.GetComponent<PlayerInput>();
    }
}
