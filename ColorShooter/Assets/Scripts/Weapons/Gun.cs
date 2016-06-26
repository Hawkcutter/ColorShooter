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
    public int numShotUpgradeCount = 0;

    private int numProjectiles = 1;

    [SerializeField]
    private float perShotDmgReduce;

    public int[] DamageAtLevel;
    public float[] CooldownAtLevel;
    public float[] SpeedAtLevel;
    public GameObject[] ProjectileSpawnPoints;

    void Start()
    {
        if(IsPlayerWeapon)
            cooldown = CooldownAtLevel[speedUpgradeCount];
    }

    public void Reset()
    {
        if (IsPlayerWeapon)
        {
            damageUpgradeCount = 0;
            speedUpgradeCount = 0;
            numShotUpgradeCount = 0;
            cooldown = CooldownAtLevel[speedUpgradeCount];

            numProjectiles = numShotUpgradeCount + 1;

            if (numProjectiles == 4)
                numProjectiles = 5;
        }
    }

    protected override void Shoot(Vector2 startPos, Vector2 direction, ColorKey color)
    {
        if (projectilePrefab)
        {

            if(IsPlayerWeapon)
            {
                if (numShotUpgradeCount == 0)
                {
                    MyShoot(ProjectileSpawnPoints[0].transform.position, ProjectileSpawnPoints[0].transform.up, color);
                }

                else if (numShotUpgradeCount == 1)
                {
                    MyShoot(ProjectileSpawnPoints[1].transform.position, ProjectileSpawnPoints[1].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[2].transform.position, ProjectileSpawnPoints[2].transform.up, color);
                }

                else if (numShotUpgradeCount == 2)
                {
                    MyShoot(ProjectileSpawnPoints[0].transform.position, ProjectileSpawnPoints[0].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[1].transform.position, ProjectileSpawnPoints[1].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[2].transform.position, ProjectileSpawnPoints[2].transform.up, color);
                }

                else if (numShotUpgradeCount == 3)
                {
                    MyShoot(ProjectileSpawnPoints[0].transform.position, ProjectileSpawnPoints[0].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[1].transform.position, ProjectileSpawnPoints[1].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[2].transform.position, ProjectileSpawnPoints[2].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[3].transform.position, ProjectileSpawnPoints[3].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[4].transform.position, ProjectileSpawnPoints[4].transform.up, color);
                }
            }

            else
            {
                MyShoot(startPos, direction, color);
            }



        }
    }

    private void MyShoot(Vector2 startPos, Vector2 direction, ColorKey color)
    {
         LinearProjectile projectile = null;

         if (IsPlayerWeapon)
         {
             projectile = InstantiateProjectile(ColorKey.GetProjectileFromColor(color.Key)) as LinearProjectile;
             projectile.Speed = SpeedAtLevel[speedUpgradeCount];


             int oneShotDmg = Mathf.Max((int)(DamageAtLevel[damageUpgradeCount] * perShotDmgReduce * (numProjectiles - 1)), DamageAtLevel[damageUpgradeCount]);

             int perShotDmg = (int)Mathf.Ceil((float)oneShotDmg / numProjectiles);

             projectile.Damage = perShotDmg;
         }


         else
             projectile = InstantiateProjectile(projectilePrefab) as LinearProjectile;

        if (ApplyStartPos)
            projectile.transform.position = startPos;

        if (RotateProjectileToDirection)
            projectile.transform.up = direction;

        projectile.Init(startPos, direction, color, IsPlayerWeapon);
    }

    public void UpgradeAttackDamage()
    {
        damageUpgradeCount = Mathf.Clamp(damageUpgradeCount + 1, 0, DamageAtLevel.Length - 1);
    }

    public void DegradeAttackDamage()
    {
        damageUpgradeCount = Mathf.Clamp(damageUpgradeCount - 1, 0, DamageAtLevel.Length - 1);
    }

    public void UpgradeAttackSpeed()
    {
        speedUpgradeCount = Mathf.Clamp(speedUpgradeCount + 1, 0, SpeedAtLevel.Length - 1);

        cooldown = CooldownAtLevel[speedUpgradeCount];
    }

    public void DowngradeAttackSpeed()
    {
        speedUpgradeCount = Mathf.Clamp(speedUpgradeCount - 1, 0, SpeedAtLevel.Length - 1);

        cooldown = CooldownAtLevel[speedUpgradeCount];
    }


    public void UpgradeNumAttacks()
    {
        numShotUpgradeCount = Mathf.Clamp(numShotUpgradeCount + 1, 0, 3);


        numProjectiles = numShotUpgradeCount + 1;

        if (numProjectiles == 4)
            numProjectiles = 5;
    }

    public void DowngradeNumAttacks()
    {
        numShotUpgradeCount = Mathf.Clamp(numShotUpgradeCount + 1, 0, 3);

        numProjectiles = numShotUpgradeCount + 1;

        if (numProjectiles == 4)
            numProjectiles = 5;
    }

}
