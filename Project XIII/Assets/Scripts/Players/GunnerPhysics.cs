using UnityEngine;
using System.Collections;

public class GunnerPhysics : PlayerPhysics{

    GunnerStats gunnerStat;
    GunnerBullets gunnerbullets;
    BulletProjectile bulletScript;
    Vector3 gunPoint;
    Vector2 velocity;
    float bulletSpeed;

    public GameObject bulletSource;

    public override void ClassSpecificStart()
    {
        gunnerStat = GetComponent<GunnerProperties>().GetGunnerStats();
        gunnerbullets = GetComponent<GunnerProperties>().GetGunnerBullets();
    }

    void ShootQuickBullet()
    {

        bulletSource.GetComponent<BulletSourceScript>().QuickShot(physicStats.quickAttackStrength);
        /*
        velocity = isFacingRight? Vector2.right : Vector2.left;
        bulletSpeed = gunnerStat.quickBulletSpeed * boostStats.speedBoost;

        
        foreach (Transform bullet in gunnerbullets.quickBullets.transform)
        {
            
            if (!bullet.gameObject.activeSelf)
            {
                bullet.gameObject.SetActive(true);

                bulletScript = bullet.gameObject.GetComponent<BulletProjectile>();
                bulletScript.SetDamageAmount(physicStats.quickAttackStrength);

                gunPoint = transform.position;
                gunPoint.x += isFacingRight ? 1.5f : -1.5f;
                gunPoint.y += .7f;

                bullet.position = gunPoint;
                bullet.GetComponent<Rigidbody2D>().velocity = velocity * bulletSpeed;
                return;
            }
        }
        */
    }

    void ShootHeavyBullet()
    {

        KnockBack(gunnerStat.heavyAttackKnockBackForce);
        bulletSource.GetComponent<BulletSourceScript>().HeavyShot(physicStats.quickAttackStrength);
        /*
        velocity = isFacingRight? Vector2.right : Vector2.left;
        bulletSpeed = gunnerStat.quickBulletSpeed * boostStats.speedBoost;

        foreach (Transform bullet in gunnerbullets.heavyBullets.transform)
        {
            if (!bullet.gameObject.activeSelf)
            {
                bullet.gameObject.transform.position = gameObject.transform.position;
                bullet.gameObject.SetActive(true);
                knockBack(gunnerStat.heavyAttackKnockBackForce);
                bulletScript = bullet.gameObject.GetComponent<BulletProjectile>();
                bulletScript.SetDamageAmount(physicStats.quickAttackStrength);
                bulletScript.DamageFadeActive(true);
                bulletScript.SetMaxDistance(5);

                gunPoint = transform.position;
                gunPoint.x += isFacingRight ? 1.5f : -1.5f;
                gunPoint.y += .7f;

                bullet.position = gunPoint;
                bullet.GetComponent<Rigidbody2D>().velocity = velocity * bulletSpeed;
                return;
            }
                }
            */


    }
}
