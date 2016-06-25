using UnityEngine;
using System.Collections;
using System;

public class Gun : Weapon
{
    [SerializeField]
    private Projectile projectilePrefab;

    private const int MAX_NUM_UGPRADES= 4;

    public bool RotateProjectileToDirection = true;
    public bool ApplyStartPos = true;

    public int DamageUpgradeCount = 0;
    public int SpeedUpgradeCount = 0;
    public int ShotUpgradeCount = 0;

    protected override void Shoot(Vector2 startPos, Vector2 direction, ColorKey color)
    {
        if (projectilePrefab)
        {
            Projectile projectile = InstantiateProjectile(projectilePrefab);

            if (ApplyStartPos)
                projectile.transform.position = startPos;

            //if (RotateProjectileToDirection)
                //projectile.transform.forward = direction;

            projectile.Init(startPos, direction, color, IsPlayerWeapon);
        }
    }

}
