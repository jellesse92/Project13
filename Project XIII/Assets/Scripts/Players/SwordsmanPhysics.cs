using UnityEngine;
using System.Collections;

public class SwordsmanPhysics : PlayerPhysics{

    //Constants for managing quick dashing skill
    const float DASH_DISTANCE = 10f;            //Distance of dasj
    const float DASH_RECOVERY_TIME = 1f;       //Time it takes to recover dashes
    const float MAX_CHAIN_DASH = 3;             //Max amount of dashes that can be chained

    public GameObject comboAttackBox;           //Collider for dealing combo attacks
    public GameObject dragAttackBox;            //Collider for dragging enemies with sword swing up or down
    public GameObject airComboAttackBox;        //Collider for dealing air combo attacks
    public GameObject heavyAirAttackBox;        //Collider for dealing with heavy air attack

    float xInputAxis = 0f;                                     
    float yInputAxis = 0f;

    bool inCombo = false;                       //Checks if swordsman able to combo

    //Dash skill variables
    int dashCount = 0;                          //Checks how many dashes have been chained
    bool checkGroundForDash = false;            //Bool that determines to check for grounded before resetting dash count

    public override void ClassSpecificStart()
    {
        comboAttackBox.GetComponent<SwordsmanMelee>().SetDamage(GetComponent<PlayerProperties>().GetPhysicStats().quickAttackStrength);
        airComboAttackBox.GetComponent<SwordsmanAirMelee>().SetDamage(GetComponent<PlayerProperties>().GetPhysicStats().quickAirAttackStrength);
        heavyAirAttackBox.GetComponent<MeleeAttackScript>().SetAttackStrength(GetComponent<PlayerProperties>().GetPhysicStats().heavyAirAttackStrengh);
        dragAttackBox.GetComponent<SwordsmanDragAttackScript>().enabled = false;
    }

    public override void ClassSpecificUpdate()
    {
        if (inCombo)
            WatchForCombo();
        if (checkGroundForDash)
        {
            ResetDashCount();
        }
    }

    public override void MovementSkill(float xMove, float yMove)
    {
        base.MovementSkill(xMove,yMove);

        xInputAxis = xMove;
        yInputAxis = yMove;

        //EVENTUALLY ADD A DASH COUNTER
        if(dashCount < MAX_CHAIN_DASH)
            GetComponent<Animator>().SetTrigger("moveSkill");
    }

    public void WatchForCombo()
    {
        if (GetComponent<PlayerInput>().getKeyPress().quickAttackPress)
        {
            inCombo = false;
            GetComponent<Animator>().SetTrigger("combo");
        }
    }

    public void StartCombo()
    {
        inCombo = true;
    }

    public void FinishCombo()
    {
        inCombo = false;
    }

    public void HeavyTransistionToAir()
    {
        GetComponent<Animator>().SetTrigger("heavyToAerial");
    }

    public void ExecuteDragAttack()
    {
        dragAttackBox.GetComponent<SwordsmanDragAttackScript>().Reset();
        dragAttackBox.GetComponent<SwordsmanDragAttackScript>().enabled = true;
    }

    public void EndDragAttack()
    {
        dragAttackBox.GetComponent<Collider2D>().enabled = false;
        dragAttackBox.GetComponent<SwordsmanDragAttackScript>().Reset();
        dragAttackBox.GetComponent<SwordsmanDragAttackScript>().enabled = false;
    }

    public void ExecuteDashSkill()
    {
        RaycastHit2D hit;
        Vector2 dir = new Vector2(xInputAxis, yInputAxis).normalized;

        dashCount++;

        if (dir.x == 0f && dir.y == 0f)
            dir.x = transform.localScale.x;

        hit = Physics2D.Raycast(transform.position, dir, DASH_DISTANCE, LayerMask.GetMask("Default"));

        if (hit.collider == null)
            StartCoroutine(Dashing(transform.position + new Vector3(dir.x * DASH_DISTANCE, dir.y * DASH_DISTANCE, transform.position.z)));
        else
        {
            float xOffset = 0f;
            float yOffset = 0f;

            StartCoroutine(Dashing(new Vector3(hit.point.x + xOffset, hit.point.y + yOffset, transform.position.z)));
        }
    }

    IEnumerator Dashing(Vector3 destination)
    {
        float gravity = GetComponent<Rigidbody2D>().gravityScale;

        DeactivateAttackMovementJump();
        VelocityY(0f);
        VelocityX(0f);

        GetComponent<Rigidbody2D>().gravityScale = 0f;

        transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE / 5f);

        for (int i = 0; i < 4; i++)
        {
            if (transform.position.x == destination.x && transform.position.y == destination.y)
                break;
            yield return new WaitForSeconds(.01f);
            transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE / 5f);
        }

        GetComponent<Rigidbody2D>().gravityScale = gravity;

        if (isGrounded())
            GetComponent<Animator>().SetTrigger("idle");
        else
            GetComponent<Animator>().SetTrigger("heavyToAerial");

        ActivateAttackMovementJump();

        Invoke("ResetDashCount", DASH_RECOVERY_TIME);

    }

    void ResetDashCount()
    {
        if (isGrounded())
        {
            dashCount = 0;
            checkGroundForDash = false;
        }
        else
            checkGroundForDash = true;

        
    }


}
