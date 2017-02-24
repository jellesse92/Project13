using UnityEngine;
using System.Collections;

[System.Serializable]
public class GunnerStats
{
    public float heavyAttackKnockBackForce = 10f;
    public int heavyTier1ChargeDamage = 25;
    public int heavyTier2ChargeDamage = 40;
    public int heavyTier1SplashDamage = 10;
    public int heavyTier2SplashDamage = 20;
}


public class GunnerProperties : PlayerProperties{

    public GunnerStats gunnerStats;

    public GunnerStats GetGunnerStats()
    {
        return gunnerStats;
    }


}
