using UnityEngine;
using System.Collections;

public class SwordsmanPhysics : PlayerPhysics{

    const float DASH_DISTANCE = 20f;

    public GameObject comboAttackBox;           //Collider for dealing combo attacks
    public GameObject dragAttackBox;            //Collider for dragging enemies with sword swing up or down
    public GameObject airComboAttackBox;        //Collider for dealing air combo attacks
    public GameObject heavyAirAttackBox;        //Collider for dealing with heavy air attack

    float xAxis = 0f;                       
    float yAxis = 0f;

    bool inCombo = false;                       //Checks if swordsman able to combo

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
    }

    public override void MovementSkill(float xMove, float yMove)
    {
        base.MovementSkill(xMove,yMove);

        xAxis = xMove;
        yAxis = yMove;

        //EVENTUALLY ADD A DASH COUNTER
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
        Vector2 dir = new Vector2(xAxis, yAxis).normalized;
        
        hit = Physics2D.Raycast(transform.position, dir, DASH_DISTANCE, LayerMask.GetMask("Default"));
        
        if(dir.x == 0f && dir.y == 0f)
            dir.x = transform.localScale.x;


        if(hit.collider == null)
            StartCoroutine(Dashing(transform.position + new Vector3(dir.x * DASH_DISTANCE, dir.y * DASH_DISTANCE, transform.position.z)));
    }

    IEnumerator Dashing(Vector3 destination)
    {
        float gravity = GetComponent<Rigidbody2D>().gravityScale;

        GetComponent<Rigidbody2D>().gravityScale = 0f;

        transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE/10f);
        yield return new WaitForSeconds(.01f);
        transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE/10f);
        yield return new WaitForSeconds(.01f);
        transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE/10f);
        yield return new WaitForSeconds(.01f);
        transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE /10f);
        yield return new WaitForSeconds(.01f);
        transform.position = Vector3.MoveTowards(transform.position, destination, DASH_DISTANCE/10f);
        yield return new WaitForSeconds(.01f);


        GetComponent<Rigidbody2D>().gravityScale = gravity;
    }


}
