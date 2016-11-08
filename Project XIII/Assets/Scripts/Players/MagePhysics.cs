using UnityEngine;
using System.Collections;

public class MagePhysics : PlayerPhysics
{
    const float QUICK_ORIGIN_X = 5f;
    const float QUICK_ORIGIN_Y = 2f;
    const float QUICK_ATTACK_DURATION = .5f;

    public GameObject quickAttackReticle;                   //Reticle for applying quick attack
    bool quickAttackActive = false;


    public override void ClassSpecificStart()
    {
        quickAttackReticle = (GameObject)Instantiate(quickAttackReticle);
        quickAttackReticle.GetComponent<MageReticleScript>().SetMaster(this.gameObject);
    }

    void ActivateQuickReticle()
    {
        if (!quickAttackActive)
        {
            quickAttackReticle.transform.position = transform.position + new Vector3(QUICK_ORIGIN_X * transform.localScale.x, QUICK_ORIGIN_Y, transform.position.z);
            quickAttackReticle.GetComponent<SpriteRenderer>().enabled = true;
            quickAttackReticle.transform.GetChild(0).gameObject.SetActive(false); //DEACTIVATING PARTICLE EFFECT
            quickAttackReticle.SetActive(true);
            quickAttackActive = true;
        }
    }

    void ExecuteQuickAttack()
    {
        if (!quickAttackReticle.transform.GetChild(0).gameObject.activeSelf)
        { 
            quickAttackReticle.GetComponent<SpriteRenderer>().enabled = false;
            quickAttackReticle.transform.GetChild(0).gameObject.SetActive(true); //ACTIVATING PARTICLE EFFECT
            quickAttackReticle.GetComponent<MageReticleScript>().ReleaseQuickAttack();
            Invoke("EndQuickAttack", QUICK_ATTACK_DURATION);
        }

    }

    void EndQuickAttack()
    {
        quickAttackReticle.GetComponent<MageReticleScript>().ExtinguishAttack(); 
        quickAttackReticle.SetActive(false); //DEACTIVATES RETICLE AND IT'S CHILDREN
        quickAttackActive = false;
    }

    void InterruptCast()
    {
        //If it's not already cast and runnning, deactivate the reticle and its children
        if (!quickAttackReticle.transform.GetChild(0).gameObject.activeSelf)
        {
            EndQuickAttack();
        }

    }
}
