using UnityEngine;
using System.Collections;

public class GunnerPhysics : PlayerPhysics{

    GunnerStats gunnerStat;
    BulletProjectile bulletScript;
    Vector3 gunPoint;
    Vector2 velocity;
    float bulletSpeed;

    public GameObject bulletSource;
    public GameObject meleeAttackBox;
    public DownKickScript downKickScript;

    public override void ClassSpecificStart()
    {
        gunnerStat = GetComponent<GunnerProperties>().GetGunnerStats();
        meleeAttackBox.GetComponent<MeleeAttackScript>().SetAttackStrength(GetComponent<PlayerProperties>().GetPhysicStats().quickAirAttackStrength);
        downKickScript.enabled = false;
    }

    void ShootQuickBullet()
    {
        bulletSource.GetComponent<BulletSourceScript>().QuickShot(physicStats.quickAttackStrength);
    }

    void ShootHeavyBullet()
    {
        KnockBack(gunnerStat.heavyAttackKnockBackForce);
        bulletSource.GetComponent<BulletSourceScript>().HeavyShot(physicStats.heavyAttackStrength);
    }

    void ExecuteDownKick()
    {
        downKickScript.Reset();
        downKickScript.enabled = true;
        downKickScript.InvokeRepeating("ApplyDamageEffect",0f,.1f);
        GetComponent<PlayerProperties>().SetStunnableState(false);
    }

    void CancelDownKick()
    {
        downKickScript.CancelInvoke("ApplyDamageEffect");
        downKickScript.enabled = false;
        GetComponent<PlayerProperties>().SetStunnableState(true);
    }

    void ApplyBounce()
    {
        downKickScript.ApplyBounce();
    }
}
