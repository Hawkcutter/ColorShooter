using UnityEngine;
using System.Collections;
using System;

public class Gun : Weapon
{
    [SerializeField]
    private Projectile projectilePrefab;

    public bool RotateProjectileToDirection = true;
    public bool ApplyStartPos = true;

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
