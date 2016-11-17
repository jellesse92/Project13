using UnityEngine;
using System.Collections;

public class SwordsmanPhysics : PlayerPhysics{

    public GameObject comboAttackBox;           //Collider for dealing combo attack damage
    public GameObject dragAttackBox;            //Collider for dragging enemies with sword swing up or down

    bool inCombo = false;                       //Checks if swordsman able to combo

    public override void ClassSpecificStart()
    {
        comboAttackBox.GetComponent<SwordsmanMelee>().SetDamage(GetComponent<PlayerProperties>().GetPhysicStats().quickAttackStrength);
        dragAttackBox.GetComponent<SwordsmanDragAttackScript>().enabled = false;
    }

    public override void ClassSpecificUpdate()
    {
        if (inCombo)
            WatchForCombo();
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
}
