using UnityEngine;
using System.Collections;

public class MagePhysics : PlayerPhysics
{
    const float QUICK_ORIGIN_X = 5f;
    const float QUICK_ORIGIN_Y = 2f;
    const float QUICK_ATTACK_DURATION = 0;

    const float HEAVY_ORIGIN_X = 5f;
    const float HEAVY_ORIGIN_Y = 0f;
    const float HEAVY_ATTACK_DURATION = 2f;

    const float HEAVY_AIR_ORIGIN_X = 5f;                    //Heavy air attack local X position
    const float HEAVY_AIR_ORIGIN_Y = 3f;                    //Heavy air attack local Y position
    const float HEAVY_AIR_ATTACK_DURATION = 3f;             //Duration for heavy air attack

    const float TELEPORT_DISTANCE = 10f;                    //Distance mage teleports when teleporting                 
    const float TELEPORT_CD_TIME = 2f;                      //Time that teleport is on cooldown

    public GameObject quickAttackReticle;                   //Reticle for applying quick attack
    public GameObject heavyAttackReticle;                   //Reticle for applying heavy attack
    bool quickAttackActive = false;                         //Returns if light ground attack is running
    bool heavyAttackActive = false;                         //Returns if heavy ground attack is running

    public GameObject shieldParticle;                       //Particle effect and collider for shield burst

    public GameObject blizzard;                             //Object for blizzard
    bool heavyAirAttackActive = false;                      //Returns if heavy air attack is on cool down or not

    //Teleport variable
    //public ParticleSystem teleportDust;
    float xInputAxis = 0f;
    float yInputAxis = 0f;
    bool teleportOnCD = false;                              //Indicates teleport is on cooldown

    public override void ClassSpecificStart()
    {
        quickAttackReticle = (GameObject)Instantiate(quickAttackReticle);
        quickAttackReticle.GetComponent<MageReticleScript>().SetMaster(this.gameObject);
        heavyAttackReticle = (GameObject)Instantiate(heavyAttackReticle);
        heavyAttackReticle.GetComponent<MageReticleScript>().SetMaster(this.gameObject);
        blizzard = (GameObject)Instantiate(blizzard);
        blizzard.GetComponent<MageBlizzardScript>().SetMaster(this.gameObject);
    }

    public override void MovementSkill(float xMove, float yMove)
    {
        base.MovementSkill(xMove, yMove);

        xInputAxis = xMove;
        yInputAxis = yMove;

        if(!teleportOnCD)
            GetComponent<Animator>().SetTrigger("moveSkill");
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

    void ExecuteHeavyAirAttack()
    {
        if (heavyAirAttackActive)
            return;

        blizzard.transform.position = transform.position + new Vector3(HEAVY_AIR_ORIGIN_X * transform.localScale.x, HEAVY_AIR_ORIGIN_Y, transform.position.z);
        blizzard.transform.rotation = Quaternion.Euler(0, 0, 20 * transform.localScale.x);
        blizzard. GetComponent<MageBlizzardScript>().ActivateAttack();
        Invoke("EndAirHeavyAttack", HEAVY_AIR_ATTACK_DURATION);
        heavyAirAttackActive = true;
    }

    protected void ActivateReticle(GameObject reticle, float x, float y, ref bool attackActiveState)
    {
        reticle.transform.position = transform.position + new Vector3(x * transform.localScale.x, y, transform.position.z);
        reticle.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        reticle.GetComponent<SpriteRenderer>().enabled = true;
        reticle.GetComponent<Collider2D>().enabled = true;
        reticle.SetActive(true);
        attackActiveState = true;
    }

 

    void PlayChildrenParticles(GameObject obj)
    {
        obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        foreach (Transform child in obj.transform.GetChild(0))
            child.GetComponent<ParticleSystem>().Play();
    }

    void ExecuteQuickAttack()
    {
        if (quickAttackReticle.GetComponent<SpriteRenderer>().enabled)
        {
            quickAttackReticle.GetComponent<SpriteRenderer>().enabled = false;
            PlayChildrenParticles(quickAttackReticle);
            quickAttackReticle.GetComponent<MageReticleScript>().ReleaseQuickAttack();
            Invoke("EndQuickAttack", QUICK_ATTACK_DURATION);
        }

    }

    void ExecuteHeavyAttack()
    {
        if (heavyAttackReticle.GetComponent<SpriteRenderer>().enabled)
        {
            heavyAttackReticle.GetComponent<SpriteRenderer>().enabled = false;
            PlayChildrenParticles(heavyAttackReticle);
            heavyAttackReticle.GetComponent<MageReticleScript>().ReleaseHeavyAttack();
            Invoke("EndHeavyAttack", HEAVY_ATTACK_DURATION);
        }

    }

    void EndAttack(GameObject reticle, ref bool activeState)
    {
        reticle.GetComponent<MageReticleScript>().ExtinguishAttack();
        reticle.GetComponent<Collider2D>().enabled = false;
        activeState = false;
    }

    void EndHeavyAttack()
    {
        heavyAttackReticle.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        heavyAttackReticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        EndAttack(heavyAttackReticle, ref heavyAttackActive);
    }

    void EndQuickAttack()
    {
        quickAttackReticle.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        quickAttackReticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        EndAttack(quickAttackReticle, ref quickAttackActive);
    }

    void EndAirHeavyAttack()
    {
        blizzard.GetComponent<MageBlizzardScript>().Reset();
        heavyAirAttackActive = false;
    }

    void InterruptCast()
    {
        //If it's not already cast and runnning, deactivate the reticle and its children
        if (!quickAttackReticle.transform.GetChild(0).gameObject.activeSelf)
        {
            EndQuickAttack();
        }
    }

    void ActivateShield()
    {
        shieldParticle.GetComponent<ParticleSystem>().Play();
    }

    void ExecuteTeleportSkill()
    {
        teleportOnCD = true;

        RaycastHit2D hit;
        Vector2 dir = new Vector2(xInputAxis, yInputAxis).normalized;

        if (dir.x == 0f && dir.y == 0f)
            dir.x = transform.localScale.x;

        hit = Physics2D.Raycast(transform.position, dir, TELEPORT_DISTANCE, LayerMask.GetMask("Default"));

        if (hit.collider == null)
        {
            transform.position += new Vector3(dir.x * TELEPORT_DISTANCE, dir.y * TELEPORT_DISTANCE, transform.position.z);
        }
        else
        {
            float xOffset = 0f;
            float yOffset = 0f;

            transform.position = new Vector3(hit.point.x + xOffset, hit.point.y + yOffset, transform.position.z);
        }

        Invoke("FinishTeleportCD", TELEPORT_CD_TIME);
    }

    void ExitTeleportState()
    {
        if (isGrounded())
            GetComponent<Animator>().SetTrigger("exitDash");
        else
            GetComponent<Animator>().SetTrigger("heavyToAerial");
    }

    void PutTeleportOnCD()
    {
        teleportOnCD = true;
    }

    void FinishTeleportCD()
    {
        teleportOnCD = false;
    }
}
