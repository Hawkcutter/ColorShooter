using UnityEngine;
using System.Collections;
using System;

public class Gun : Weapon
{
    [SerializeField]
    private LinearProjectile projectilePrefab;

    public bool RotateProjectileToDirection = true;
    public bool ApplyStartPos = true;

    [SerializeField]
    public int damageUpgradeCount = 0;
    [SerializeField]
    public int speedUpgradeCount = 0;
    [SerializeField]
    private int numShotUpgradeCount = 0;

    public int[] DamageAtLevel;
    public float[] CooldownAtLevel;
    public float[] SpeedAtLevel;
    public GameObject[] ProjectileSpawnPoint;

    void Start()
    {
        cooldown = CooldownAtLevel[speedUpgradeCount];
    }

    protected override void Shoot(Vector2 startPos, Vector2 direction, ColorKey color)
    {
        if (projectilePrefab)
        {

            if (numShotUpgradeCount == 0)
            {
                MyShoot(ProjectileSpawnPoint[0].transform.position, ProjectileSpawnPoint[0].transform.up, color);
            }

            else if (numShotUpgradeCount == 1)
            {
                MyShoot(ProjectileSpawnPoint[1].transform.position, ProjectileSpawnPoint[1].transform.up, color);

                MyShoot(ProjectileSpawnPoint[2].transform.position, ProjectileSpawnPoint[2].transform.up, color);
            }

            else if (numShotUpgradeCount == 2)
            {
                MyShoot(ProjectileSpawnPoint[0].transform.position, ProjectileSpawnPoint[0].transform.up, color);

                MyShoot(ProjectileSpawnPoint[1].transform.position, ProjectileSpawnPoint[1].transform.up, color);

                MyShoot(ProjectileSpawnPoint[2].transform.position, ProjectileSpawnPoint[2].transform.up, color);
            }

            else if (numShotUpgradeCount == 3)
            {
                MyShoot(ProjectileSpawnPoint[0].transform.position, ProjectileSpawnPoint[0].transform.up, color);

                MyShoot(ProjectileSpawnPoint[1].transform.position, ProjectileSpawnPoint[1].transform.up, color);

                MyShoot(ProjectileSpawnPoint[2].transform.position, ProjectileSpawnPoint[2].transform.up, color);

                MyShoot(ProjectileSpawnPoint[3].transform.position, ProjectileSpawnPoint[3].transform.up, color);

                MyShoot(ProjectileSpawnPoint[4].transform.position, ProjectileSpawnPoint[4].transform.up, color);
            }




        }
    }

    private void MyShoot(Vector2 startPos, Vector2 direction, ColorKey color)
    {
         LinearProjectile projectile = null;

        if(IsPlayerWeapon)
            projectile = InstantiateProjectile(ColorKey.GetProjectileFromColor(color.Key)) as LinearProjectile;

        else
            projectile = InstantiateProjectile(projectilePrefab) as LinearProjectile;

        projectile.Speed = SpeedAtLevel[speedUpgradeCount];
        projectile.Damage = DamageAtLevel[damageUpgradeCount];


        if (ApplyStartPos)
            projectile.transform.position = startPos;

        //TODO:
        if (RotateProjectileToDirection)
            projectile.transform.up = direction;

        projectile.Init(startPos, direction, color, IsPlayerWeapon);
    }

    public void UpgradeDamage()
    {
        damageUpgradeCount = Mathf.Clamp(damageUpgradeCount + 1, 0, DamageAtLevel.Length - 1);


    }

    public void UpgradeSpeed()
    {
        speedUpgradeCount = Mathf.Clamp(speedUpgradeCount + 1, 0, SpeedAtLevel.Length - 1);
    }

    public void UpgradeNumShots()
    {
        speedUpgradeCount = Mathf.Clamp(speedUpgradeCount + 1, 0, CooldownAtLevel.Length - 1);

        cooldown = CooldownAtLevel[speedUpgradeCount];
    }

}
