using UnityEngine;
using System.Collections;

public class MagePhysics : PlayerPhysics
{
    const float QUICK_ORIGIN_X = 5f;
    const float QUICK_ORIGIN_Y = 2f;
    const float QUICK_ATTACK_DURATION = .5f;

    const float HEAVY_ORIGIN_X = 5f;
    const float HEAVY_ORIGIN_Y = 0f;
    const float HEAVY_ATTACK_DURATION = 2f;

    public GameObject quickAttackReticle;                   //Reticle for applying quick attack
    public GameObject heavyAttackReticle;                   //Reticle for applying heavy attack
    bool quickAttackActive = false;                         //Returns if light ground attack is running
    bool heavyAttackActive = false;                         //Returns if heavy ground attack is running


    public override void ClassSpecificStart()
    {
        quickAttackReticle = (GameObject)Instantiate(quickAttackReticle);
        quickAttackReticle.GetComponent<MageReticleScript>().SetMaster(this.gameObject);
        heavyAttackReticle = (GameObject)Instantiate(heavyAttackReticle);
        heavyAttackReticle.GetComponent<MageReticleScript>().SetMaster(this.gameObject);
    }

    

    void ActivateQuickReticle()
    {
        if (!quickAttackActive)
        {
            ActivateReticle(quickAttackReticle, QUICK_ORIGIN_X, QUICK_ORIGIN_Y, ref quickAttackActive);
        }
    }

    void ActivateHeavyReticle()
    {
        if (!heavyAttackActive)
        {
            ActivateReticle(heavyAttackReticle, HEAVY_ORIGIN_X, HEAVY_ORIGIN_Y, ref heavyAttackActive);
        }
    }

    protected void ActivateReticle(GameObject reticle, float x, float y, ref bool attackActiveState)
    {
        reticle.transform.position = transform.position + new Vector3(x * transform.localScale.x, y, transform.position.z);
        reticle.GetComponent<SpriteRenderer>().enabled = true;
        reticle.transform.GetChild(0).gameObject.SetActive(false);      //Deactivate particle effect before activating parent again to appear
        reticle.SetActive(true);
        attackActiveState = true;
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

    void ExecuteHeavyAttack()
    {
        if (!heavyAttackReticle.transform.GetChild(0).gameObject.activeSelf)
        {
            heavyAttackReticle.GetComponent<SpriteRenderer>().enabled = false;
            heavyAttackReticle.transform.GetChild(0).gameObject.SetActive(true); //ACTIVATING PARTICLE EFFECT
            heavyAttackReticle.GetComponent<MageReticleScript>().ReleaseHeavyAttack();
            Invoke("EndHeavyAttack", HEAVY_ATTACK_DURATION);
        }

    }

    void EndAttack(GameObject reticle, ref bool activeState)
    {
        reticle.GetComponent<MageReticleScript>().ExtinguishAttack();
        reticle.SetActive(false);
        activeState = false;
    }

    void EndHeavyAttack()
    {
        EndAttack(heavyAttackReticle, ref heavyAttackActive);
    }

    void EndQuickAttack()
    {
        EndAttack(quickAttackReticle, ref quickAttackActive);
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
