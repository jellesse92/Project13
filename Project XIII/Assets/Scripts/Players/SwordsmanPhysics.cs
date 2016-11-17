using UnityEngine;
using System.Collections;

public class SwordsmanPhysics : PlayerPhysics{

    bool inCombo = false;                       //Checks if swordsman able to combo

    public override void ClassSpecificStart()
    {

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
}
