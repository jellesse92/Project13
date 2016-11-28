using UnityEngine;
using System.Collections;

public class MechPhysics : PlayerPhysics{

    public GameObject comboAttackBox;           //Collider for dealing combo attacks
    public GameObject airComboAttackBox;        //Collider for dealing air combo attacks

    bool inCombo = false;                       //Checks if mech able to combo

    const float HEAVY_AIR_ATTACK_DURATION = .7f;

    public GameObject barrage;                               //Object for rocket barrage (temporarily represented as blizzard)
    bool heavyAirAttackActive = false;                      //Returns if heavy air attack is on cooldown or not

    public GameObject rocket;                               //object for homing rocket attack;

    public override void ClassSpecificStart()
    {
        comboAttackBox.GetComponent<SwordsmanMelee>().SetDamage(GetComponent<PlayerProperties>().GetPhysicStats().quickAttackStrength);
        airComboAttackBox.GetComponent<SwordsmanAirMelee>().SetDamage(GetComponent<PlayerProperties>().GetPhysicStats().quickAirAttackStrength);
        barrage = (GameObject)Instantiate(barrage);
        barrage.GetComponent<MechRocketBarrage>().SetMaster(this.gameObject);
        rocket = (GameObject)Instantiate(rocket);
        rocket.SetActive(false);
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

    void ExecuteHeavyAirAttack()
    {
        if (heavyAirAttackActive)
            return;
        barrage.transform.position = transform.position;
        barrage.transform.rotation = Quaternion.Euler(0, 0, 20 * transform.localScale.x);
        barrage.GetComponent<MechRocketBarrage>().ActivateAttack();
        Invoke("EndAirHeavyAttack", HEAVY_AIR_ATTACK_DURATION);
        heavyAirAttackActive = true;
    }

    void EndAirHeavyAttack()
    {
        barrage.GetComponent<MechRocketBarrage>().Reset();
        heavyAirAttackActive = false;
    }

    void ExecuteHeavyGroundAttack()
    {
        if (rocket.active == false)
        {
            rocket.transform.position = transform.position;
            rocket.SetActive(true);
            GameObject target = GameObject.FindGameObjectWithTag("Enemy");
            rocket.GetComponent<RocketBehavior>().SetTarget(target);
            rocket.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
