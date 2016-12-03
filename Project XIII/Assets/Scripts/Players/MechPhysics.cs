using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MechPhysics : PlayerPhysics{

    public GameObject comboAttackBox;           //Collider for dealing combo attacks
    public GameObject airComboAttackBox;        //Collider for dealing air combo attacks

    bool inCombo = false;                       //Checks if mech able to combo

    const float HEAVY_AIR_ATTACK_DURATION = .7f;

    public GameObject barrage;                               //Object for rocket barrage (temporarily represented as blizzard)
    bool heavyAirAttackActive = false;                      //Returns if heavy air attack is on cooldown or not

    public GameObject rocket;                               //object for homing rocket attack;
    public GameObject rocket2;
    public GameObject rocket3;
    public GameObject rocket4;

    public GameObject wall;
    const float WALL_ORIGIN_X = 5f;
    const float WALL_ORIGIN_Y = 0f;

    bool charging = false;
    float charge = 0;

    public override void ClassSpecificStart()
    {
        comboAttackBox.GetComponent<SwordsmanMelee>().SetDamage(GetComponent<PlayerProperties>().GetPhysicStats().quickAttackStrength);
        airComboAttackBox.GetComponent<SwordsmanAirMelee>().SetDamage(GetComponent<PlayerProperties>().GetPhysicStats().quickAirAttackStrength);
        barrage = (GameObject)Instantiate(barrage);
        barrage.GetComponent<MechRocketBarrage>().SetMaster(this.gameObject);
        rocket = (GameObject)Instantiate(rocket);
        rocket.SetActive(false);
        rocket2 = (GameObject)Instantiate(rocket);
        rocket2.SetActive(false);
        rocket3 = (GameObject)Instantiate(rocket);
        rocket3.SetActive(false);
        rocket4 = (GameObject)Instantiate(rocket);
        rocket4.SetActive(false);
        wall = (GameObject)Instantiate(wall);
        wall.SetActive(false);
        wall.GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void ClassSpecificUpdate()
    {
        if (inCombo)
            WatchForCombo();
        if(charging)
        {
            charge += Time.deltaTime;
        }
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

    void BeginChargingHeavyGroundAttack()
    {
       charging = true;
    }

    void ExecuteHeavyGroundAttack()
    {
        Debug.Log(charge);

        if (rocket.activeSelf == false)
        {
            GameObject target = GameObject.FindGameObjectWithTag("Enemy");

            rocket.transform.position = transform.position;
            rocket.SetActive(true);
            rocket.GetComponent<RocketBehavior>().SetTarget(target);
            rocket.GetComponent<BoxCollider2D>().enabled = true;
            rocket.GetComponent<RocketBehavior>().SetDirection(1);

            if (charge > 0.5)
            {
                rocket2.transform.position = transform.position;
                rocket2.SetActive(true);
                rocket2.GetComponent<RocketBehavior>().SetTarget(target);
                rocket2.GetComponent<BoxCollider2D>().enabled = true;
                rocket2.GetComponent<RocketBehavior>().SetDirection(2);
            }

            if (charge > 1)
            {
                rocket3.transform.position = transform.position;
                rocket3.SetActive(true);
                rocket3.GetComponent<RocketBehavior>().SetTarget(target);
                rocket3.GetComponent<BoxCollider2D>().enabled = true;
                rocket3.GetComponent<RocketBehavior>().SetDirection(3);
            }

            if (charge > 1.5)
            {
                rocket4.transform.position = transform.position;
                rocket4.SetActive(true);
                rocket4.GetComponent<RocketBehavior>().SetTarget(target);
                rocket4.GetComponent<BoxCollider2D>().enabled = true;
                rocket4.GetComponent<RocketBehavior>().SetDirection(4);
            }
        }

        charge = 0;
    }

    void SummonWall()
    {
        if (wall.activeSelf == false)
        {
            wall.transform.position = transform.position + new Vector3(WALL_ORIGIN_X * transform.localScale.x, -1.04f, transform.position.z);
            wall.SetActive(true);
            wall.GetComponent<BoxCollider2D>().enabled = true;
            wall.GetComponent<MechWallBehavior>().counter = 0;
            wall.GetComponent<Animator>().SetBool("raising", true);
        }
    }
}
